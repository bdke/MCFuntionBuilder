using Antlr4.Runtime.Misc;
using MCFBuilder.Type.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCFBuilder
{
    public partial class ScriptVisitor
    {
        public override object? VisitExecuteBlock(MCFBuilderParser.ExecuteBlockContext context)
        {
            var types = context.executeTypes();

            List<int> nums = new();

            //for (int i = 0; i < types.Length; i++)
            //{
            //    nums.Add(ExecutesHandler.Add(types[i].GetText(), selectors[i].GetText()));
            //}

            foreach (var type in types)
            {
                nums.Add(ExecutesHandler.Add((string)Visit(type)));
            }

            Visit(context.block());

            nums.Reverse();
            foreach (int num in nums)
            {
                ExecutesHandler.Remove(num);
            }


            return null;
        }

        public override object? VisitExecuteAsExpression(MCFBuilderParser.ExecuteAsExpressionContext context)
        {
            return $"as {(string?)Visit(context.selector())}";
        }

        public override object? VisitExecuteAtExpression(MCFBuilderParser.ExecuteAtExpressionContext context)
        {
            return $"at {(string?)Visit(context.selector())}";
        }

        public override object VisitExeuctePositionedExpression(MCFBuilderParser.ExeuctePositionedExpressionContext context)
        {
            var vector = (context.vector() != null) ? context.vector().GetText() : "" ;
            var selector = (context.selector != null) ? "as " + (string?)Visit(context.selector()) : "";

            return $"positioned {vector}{selector}";
        }
    }
}
