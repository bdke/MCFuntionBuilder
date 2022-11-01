using Antlr4.Runtime.Misc;
using MCFBuilder.Type.Compiler;
using MCFBuilder.Utility;
using MCFBuilder.Utility.BuiltIn;
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

            int? currentNum;

            CommandAttribute.IsContainElse = false;
            currentNum = IfConditionHandler.Add((string?)selector, "entity");
            Visit(context.block());

            if (context.elseIfBlock() != null)
            {
                CommandAttribute.IsContainElse = true;
                Visit(context.elseIfBlock());
            }

            IfConditionHandler.Remove(currentNum);

            return null;
        }

        public override object? VisitIfScoreMatchesCompareExpression([NotNull] MCFBuilderParser.IfScoreMatchesCompareExpressionContext context)
        {
            var selector = (string?)Visit(context.selector());
            var varName = context.IDENTIFIER().GetText();
            var numValue = Visit(context.expression());
            var compareOp = context.compareOp().GetText();

            int? currentNum = null;
            CommandAttribute.IsContainElse = false;

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
                    currentNum = IfConditionHandler.Add(varName ,scoreboard.Name , compareOp, num);
                    Visit(context.block());
                }
            }

            if (ProgramVariables.ScoreboardObjects.Where(v => v.ScoreboardValues.Name == varName).Any())
            {
                var value = ProgramVariables.ScoreboardObjects.Where(v => v.ScoreboardValues.Name == varName).ElementAt(0).ScoreboardValues.Value;
                var scoreboard = (from i in value where i.Key == selector select new { Name = i.Key, Value = i.Value }).First();
                if (numValue is int num)
                {
                    currentNum = IfConditionHandler.Add(varName, scoreboard.Name, compareOp, num);
                    Visit(context.block());
                }
            }

            if (context.elseIfBlock() != null)
            {
                CommandAttribute.IsContainElse = true;
                Visit(context.elseIfBlock());
            }

            IfConditionHandler.Remove(currentNum);

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

            int? currentNum = null;
            CommandAttribute.IsContainElse = false;

            if (scbLeft != null && scbRight != null)
            {
                var valueLeft = (from i in scbLeft.ScoreboardValues.Value where i.Key == selectorLeft select new { Name = i.Key, Value = i.Value }).First();
                var valueRight = (from i in scbLeft.ScoreboardValues.Value where i.Key == selectorRight select new { Name = i.Key, Value = i.Value }).First();
                currentNum = IfConditionHandler.Add(varNameLeft, valueLeft.Name, varNameRight, valueRight.Name, compareOp);
                Visit(context.block());
            }
            else
                throw new ArgumentException();

            if (context.elseIfBlock() != null)
            {
                CommandAttribute.IsContainElse = true;
                Visit(context.elseIfBlock());
            }

            IfConditionHandler.Remove(currentNum);

            return null;
        }

        public override object? VisitIfPredicateExpression([NotNull] MCFBuilderParser.IfPredicateExpressionContext context)
        {
            var _namespace = Visit(context.expression(0));
            var path = Visit(context.expression(1));

            int? currentNum = null;
            CommandAttribute.IsContainElse = false;

            currentNum = IfConditionHandler.Add($"{_namespace}:{path}", "predicate");
            Visit(context.block());

            if (context.elseIfBlock() != null)
            {
                CommandAttribute.IsContainElse = true;
                Visit(context.elseIfBlock());
            }

            IfConditionHandler.Remove(currentNum);

            return null;
        }

        public override object? VisitIfBlockExpression([NotNull] MCFBuilderParser.IfBlockExpressionContext context)
        {
            var vector = Visit(context.vector());
            var block = Visit(context.expression());
            if (block is not string)
            {
                throw new ArgumentException();
            }

            int? currentNum = null;
            CommandAttribute.IsContainElse = false;

            currentNum = IfConditionHandler.Add($"{vector} {block}", "block");
            Visit(context.block());

            if (context.elseIfBlock() != null)
            {
                CommandAttribute.IsContainElse = true;
                Visit(context.elseIfBlock());
            }

            IfConditionHandler.Remove(currentNum);

            return null;
        }

        public override object? VisitIfBlocksExpression([NotNull] MCFBuilderParser.IfBlocksExpressionContext context)
        {
            var vec1 = Visit(context.vector(0));
            var vec2 = Visit(context.vector(1));
            var vec3 = Visit(context.vector(2));

            int? currentNum;
            CommandAttribute.IsContainElse = false;

            currentNum = IfConditionHandler.Add($"{vec1} {vec2} {vec3} {context.IFBLOCKSTYPE().GetText()}", "blocks");
            Visit(context.block());

            if (context.elseIfBlock() != null)
            {
                CommandAttribute.IsContainElse = true;
                Visit(context.elseIfBlock());
            }

            IfConditionHandler.Remove(currentNum);

            return null;
        }

        public override object? VisitIfDataBlockExpression([NotNull] MCFBuilderParser.IfDataBlockExpressionContext context)
        {
            var vec = Visit(context.vector());
            var exp = Visit(context.expression());
            if (exp is not string)
                throw new ArgumentException();

            int? currentNum;
            CommandAttribute.IsContainElse = false;

            currentNum = IfConditionHandler.Add($"{vec} {exp}", "data block");
            Visit(context.block());

            if (context.elseIfBlock() != null)
            {
                CommandAttribute.IsContainElse = true;
                Visit(context.elseIfBlock());
            }

            IfConditionHandler.Remove(currentNum);

            return null;
        }

        public override object? VisitIfDataEntityExpression([NotNull] MCFBuilderParser.IfDataEntityExpressionContext context)
        {
            var selector = Visit(context.selector());
            var exp = Visit(context.expression());
            if (exp is not string)
                throw new ArgumentException();

            int? currentNum;
            CommandAttribute.IsContainElse = false;

            currentNum = IfConditionHandler.Add($"{selector} {exp}", "data entity");
            Visit(context.block());

            if (context.elseIfBlock() != null)
            {
                CommandAttribute.IsContainElse = true;
                Visit(context.elseIfBlock());
            }

            IfConditionHandler.Remove(currentNum);

            return null;
        }

        public override object? VisitIfDataStorageExpression([NotNull] MCFBuilderParser.IfDataStorageExpressionContext context)
        {
            var source = Visit(context.expression(0));
            var exp = Visit(context.expression(1));
            if (exp is not string || source is not string)
                throw new ArgumentException();

            int? currentNum;
            CommandAttribute.IsContainElse = false;

            currentNum = IfConditionHandler.Add($"{source} {exp}", "data storage");
            Visit(context.block());

            if (context.elseIfBlock() != null)
            {
                CommandAttribute.IsContainElse = true;
                Visit(context.elseIfBlock());
            }

            IfConditionHandler.Remove(currentNum);

            return null;
        }
    }
}
