using Antlr4.Runtime.Misc;
using MCFBuilder.Type.Compiler;
using MCFBuilder.Utility;
using MCFBuilder.Utility.BuiltIn;
using MCFBuilder.Utility.BuiltIn.Class;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MCFBuilder
{
    partial class ScriptVisitor
    {
        public override object? VisitIfConditionalExpression([NotNull] MCFBuilderParser.IfConditionalExpressionContext context)
        {
            var blocks = context.ifConditionalBlock().Select(Visit).Select(Convert.ToInt32).Reverse().ToArray();
            //foreach (var block in blocks)
            //{
            //    CommandAttribute.IsContainElse = false;
            //    int currentNum = block;
            //    Visit(context.block());

            //    if (context.elseIfBlock() != null)
            //    {
            //        CommandAttribute.IsContainElse = true;
            //        CommandAttribute.Attributes[currentNum].AttributeType = (CommandAttribute.Attributes[currentNum].AttributeType == AttributeType.IF) ? AttributeType.UNLESS : AttributeType.IF;
            //        Visit(context.elseIfBlock());
            //    }

            //    IfConditionHandler.Remove(currentNum);
            //}
            CommandAttribute.IsContainElse = false;
            Visit(context.block());
            foreach (var block in blocks)
            {
                if (context.elseIfBlock() != null)
                {
                    CommandAttribute.Attributes[block].AttributeType = (CommandAttribute.Attributes[block].AttributeType == AttributeType.IF) ? AttributeType.UNLESS : AttributeType.IF;
                }
            }
            if (context.elseIfBlock() != null)
            {
                CommandAttribute.IsContainElse = true;
                Visit(context.elseIfBlock());
            }

            foreach (var block in blocks)
            {
                IfConditionHandler.Remove(block);
            }

            return null;
        }


        public override object? VisitIfStandardExpression([NotNull] MCFBuilderParser.IfStandardExpressionContext context)
        {
            var exp = Visit(context.expression());

            CommandAttribute.IsContainElse = false;

            if (IsTrue(exp))
            {
                Visit(context.block());
            }

            if (context.elseIfBlock() != null)
            {
                CommandAttribute.IsContainElse = true;
                Visit(context.elseIfBlock());
            }

            return null;
        }

        public override object? VisitIfEntityExpression([NotNull] MCFBuilderParser.IfEntityExpressionContext context)
        {
            var selector = Visit(context.selector());

            return IfConditionHandler.Add((string?)selector, "entity");
        }

        public override object? VisitIfScoreMatchesCompareExpression([NotNull] MCFBuilderParser.IfScoreMatchesCompareExpressionContext context)
        {
            var selector = (string?)Visit(context.selector());
            var varName = context.IDENTIFIER().GetText();
            var numValue = Visit(context.expression());
            var compareOp = context.compareOp().GetText();

            if (selector[0..2] == "@a")
            {
                throw new Exception();
            }

            if (scoreboards.Where(v => v.ScoreboardValues.Name == varName).Any())
            {
                var value = scoreboards.Where(v => v.ScoreboardValues.Name == varName).ToList().First().ScoreboardValues.Value;
                var scoreboard = (from i in value where i.Key == selector select new { Name = i.Key, Value = i.Value }).First();
                if (numValue is int num)
                {
                    return IfConditionHandler.Add(varName ,scoreboard.Name , compareOp, num);
                }
            }

            if (ProgramVariables.ScoreboardObjects.Where(v => v.ScoreboardValues.Name == varName).Any())
            {
                var value = ProgramVariables.ScoreboardObjects.Where(v => v.ScoreboardValues.Name == varName).ElementAt(0).ScoreboardValues.Value;
                var scoreboard = (from i in value where i.Key == selector select new { Name = i.Key, Value = i.Value }).First();
                if (numValue is int num)
                {
                    return IfConditionHandler.Add(varName, scoreboard.Name, compareOp, num);
                }
            }

            return null;
        }

        public override object? VisitIfScoreCompareExpression([Antlr4.Runtime.Misc.NotNull] MCFBuilderParser.IfScoreCompareExpressionContext context)
        {
            //selector
            var selectorLeft = (string?)Visit(context.selector(0));
            var selectorRight = (string?)Visit(context.selector(1));

            //varName
            var varNameLeft = context.IDENTIFIER(0).GetText();
            var varNameRight = context.IDENTIFIER(1).GetText();

            var compareOp = context.compareOp().GetText();

            var scbLeft = scoreboards.Where(v => v.ScoreboardValues.Name == varNameLeft).FirstOrDefault();
            var scbRight = scoreboards.Where(v => v.ScoreboardValues.Name == varNameRight).FirstOrDefault();

            if (scbLeft != null && scbRight != null)
            {
                var valueLeft = (from i in scbLeft.ScoreboardValues.Value where i.Key == selectorLeft select new { Name = i.Key, Value = i.Value }).First();
                var valueRight = (from i in scbLeft.ScoreboardValues.Value where i.Key == selectorRight select new { Name = i.Key, Value = i.Value }).First();
                return IfConditionHandler.Add(varNameLeft, valueLeft.Name, varNameRight, valueRight.Name, compareOp);
            }
            else
                throw new ArgumentException();
        }

        public override object? VisitIfPredicateExpression([NotNull] MCFBuilderParser.IfPredicateExpressionContext context)
        {
            var _namespace = Visit(context.expression(0));
            var path = Visit(context.expression(1));

            return IfConditionHandler.Add($"{_namespace}:{path}", "predicate");
        }

        public override object? VisitIfBlockExpression([NotNull] MCFBuilderParser.IfBlockExpressionContext context)
        {
            var vector = Visit(context.vector());
            var block = Visit(context.expression());
            if (block is not string)
            {
                throw new ArgumentException();
            }

            return IfConditionHandler.Add($"{vector} {block}", "block");
        }

        public override object? VisitIfBlocksExpression([NotNull] MCFBuilderParser.IfBlocksExpressionContext context)
        {
            var vec1 = Visit(context.vector(0));
            var vec2 = Visit(context.vector(1));
            var vec3 = Visit(context.vector(2));

            return IfConditionHandler.Add($"{vec1} {vec2} {vec3} {context.IFBLOCKSTYPE().GetText()}", "blocks");
        }

        public override object? VisitIfDataBlockExpression([NotNull] MCFBuilderParser.IfDataBlockExpressionContext context)
        {
            var vec = Visit(context.vector());
            var exp = Visit(context.expression());
            if (exp is not string)
                throw new ArgumentException();

            return IfConditionHandler.Add($"{vec} {exp}", "data block");
        }

        public override object? VisitIfDataEntityExpression([NotNull] MCFBuilderParser.IfDataEntityExpressionContext context)
        {
            var selector = Visit(context.selector());
            var exp = Visit(context.expression());
            if (exp is not string)
                throw new ArgumentException();

            return IfConditionHandler.Add($"{selector} {exp}", "data entity");
        }

        public override object? VisitIfDataStorageExpression([NotNull] MCFBuilderParser.IfDataStorageExpressionContext context)
        {
            var source = Visit(context.expression(0));
            var exp = Visit(context.expression(1));
            if (exp is not string || source is not string)
                throw new ArgumentException();

            return IfConditionHandler.Add($"{source} {exp}", "data storage");
        }
    }
}
