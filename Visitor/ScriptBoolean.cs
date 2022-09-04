using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCFBuilder
{
    public partial class ScriptVisitor
    {
        #region Comparator
        private object? LessThanOrEqual(object? left, object? right)
        {
            if (left is int l && right is int r) return l <= r;
            if (left is float l2 && right is int r2) return l2 <= r2;
            if (left is int l3 && right is float r3) return l3 <= r3;
            if (left is float l4 && right is float r4) return l4 <= r4;

            throw new Exception($"Cannot compare values of types {left?.GetType()} and {right?.GetType()}");
        }

        private object? GreaterThanOrEqual(object? left, object? right)
        {
            if (left is int l && right is int r) return l >= r;
            if (left is float l2 && right is int r2) return l2 >= r2;
            if (left is int l3 && right is float r3) return l3 >= r3;
            if (left is float l4 && right is float r4) return l4 >= r4;

            throw new Exception($"Cannot compare values of types {left?.GetType()} and {right?.GetType()}");
        }

        private object? GreaterThan(object? left, object? right)
        {
            if (left is int l && right is int r) return l > r;
            if (left is float l2 && right is int r2) return l2 > r2;
            if (left is int l3 && right is float r3) return l3 > r3;
            if (left is float l4 && right is float r4) return l4 > r4;

            throw new Exception($"Cannot compare values of types {left?.GetType()} and {right?.GetType()}");
        }

        private object? NotEqaul(object? left, object? right)
        {
            if (left is int l && right is int r) return l != r;
            if (left is float l2 && right is int r2) return l2 != r2;
            if (left is int l3 && right is float r3) return l3 != r3;
            if (left is float l4 && right is float r4) return l4 != r4;

            throw new Exception($"Cannot compare values of types {left?.GetType()} and {right?.GetType()}");
        }

        private object? Equal(object? left, object? right)
        {
            if (left is int l && right is int r) return l == r;
            if (left is float l2 && right is int r2) return l2 == r2;
            if (left is int l3 && right is float r3) return l3 == r3;
            if (left is float l4 && right is float r4) return l4 == r4;

            throw new Exception($"Cannot compare values of types {left?.GetType()} and {right?.GetType()}");
        }

        private object? LessThan(object? left, object? right)
        {
            if (left is int l && right is int r) return l < r;
            if (left is float l2 && right is int r2) return l2 < r2;
            if (left is int l3 && right is float r3) return l3 < r3;
            if (left is float l4 && right is float r4) return l4 < r4;

            throw new Exception($"Cannot compare values of types {left?.GetType()} and {right?.GetType()}");
        }
        #endregion

        public override object? VisitComparisionExpression(MCFBuilderParser.ComparisionExpressionContext context)
        {
            var left = Visit(context.expression(0));
            var right = Visit(context.expression(1));

            var op = context.compareOp().GetText();

            return op switch
            {
                "==" => Equal(left, right),
                "!=" => NotEqaul(left, right),
                ">" => GreaterThan(left, right),
                "<" => LessThan(left, right),
                ">=" => GreaterThanOrEqual(left, right),
                "<=" => LessThanOrEqual(left, right),
                _ => throw new NotImplementedException()
            };
        }

        private bool IsTrue(object? value)
        {
            if (value is bool b) return b;
            throw new Exception("Value is not boolean");
        }

        private bool IsFalse(object? value) => !IsTrue(value);
    }
}
