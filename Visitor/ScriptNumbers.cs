using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCFBuilder
{
    public partial class ScriptVisitor
    {
        //TODO: plus equal...
        public override object? VisitAdditiveExpression(MCFBuilderParser.AdditiveExpressionContext context)
        {
            var left = Visit(context.expression(0));
            var right = Visit(context.expression(1));

            var op = context.addOp().GetText();

            return op switch
            {
                "+" => Add(left, right),
                "-" => Subtract(left, right),
                _ => throw new NotImplementedException()
            };
        }

        public override object? VisitMultiplicativeExpression(MCFBuilderParser.MultiplicativeExpressionContext context)
        {
            var left = Visit(context.expression(0));
            var right = Visit(context.expression(1));

            var op = context.multOp().GetText();

            return op switch
            {
                "*" => Multiply(left, right),
                "/" => Divide(left, right),
                "%" => Remainder(left, right),
                _ => throw new NotImplementedException()
            };
        }


        #region Numbers
        private object? Add(object? left, object? right)
        {
            if (left is int l && right is int r) return l + r;
            if (left is float l2 && right is int r2) return l2 + r2;
            if (left is int l3 && right is float r3) return l3 + r3;
            if (left is float l4 && right is float r4) return l4 + r4;

            if (left is string || right is string)
                return $"{left}{right}";

            throw new Exception($"Cannot add values of types {left?.GetType()} and {right?.GetType()}");
        }

        private object? Subtract(object? left, object? right)
        {
            if (left is int l && right is int r) return l - r;
            if (left is float l2 && right is int r2) return l2 - r2;
            if (left is int l3 && right is float r3) return l3 - r3;
            if (left is float l4 && right is float r4) return l4 - r4;

            throw new Exception($"Cannot add values of types {left?.GetType()} and {right?.GetType()}");
        }

        private object? Multiply(object? left, object? right)
        {
            if (left is int l && right is int r) return l * r;
            if (left is float l2 && right is int r2) return l2 * r2;
            if (left is int l3 && right is float r3) return l3 * r3;
            if (left is float l4 && right is float r4) return l4 * r4;

            throw new Exception($"Cannot add values of types {left?.GetType()} and {right?.GetType()}");
        }

        private object? Remainder(object? left, object? right)
        {
            if (left is int l && right is int r) return l % r;
            if (left is float l2 && right is int r2) return l2 % r2;
            if (left is int l3 && right is float r3) return l3 % r3;
            if (left is float l4 && right is float r4) return l4 % r4;

            throw new Exception($"Cannot add values of types {left?.GetType()} and {right?.GetType()}");
        }

        private object? Divide(object? left, object? right)
        {
            if (left is int l && right is int r) return l / r;
            if (left is float l2 && right is int r2) return l2 / r2;
            if (left is int l3 && right is float r3) return l3 / r3;
            if (left is float l4 && right is float r4) return l4 / r4;

            throw new Exception($"Cannot add values of types {left?.GetType()} and {right?.GetType()}");
        }
        #endregion
    }
}
