using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCFBuilder;

namespace MCFBuilder.Utility
{
    public static class AssignmentHelper
    {
        public static int? AssignOp(string op, int? left, int? right)
        {
            return op switch
            {
                "=" => right,
                "+=" => (int?)ScriptVisitor.Add(left, right),
                "-=" => (int?)ScriptVisitor.Subtract(left, right),
                "*=" => (int?)ScriptVisitor.Multiply(left, right),
                "/=" => (int?)ScriptVisitor.Divide(left, right),
                "%=" => (int?)ScriptVisitor.Remainder(left, right),
                _ => throw new NotImplementedException()
            };
        }

        public static int? ScoreAssignOp(string op, int? left, int? right, Scoreboard scoreboard, string? selector)
        {
            switch (op)
            {
                case "=":
                    scoreboard.Set(right, selector);
                    return right;
                case "+=":
                    scoreboard.Add(right,selector);
                    return (int?)ScriptVisitor.Add(left, right);
                case "-=":
                    scoreboard.Subtract(right, selector);
                    return (int?)ScriptVisitor.Subtract(left, right);
                case "*=":
                    scoreboard.Multiply(right, selector);
                    return (int?)ScriptVisitor.Multiply(left, right);
                case "/=":
                    scoreboard.Divide(right, selector);
                    return (int?)ScriptVisitor.Divide(left, right);
                case "%=":
                    scoreboard.Remainder(right, selector);
                    return (int?)ScriptVisitor.Remainder(left, right);
                default:
                    throw new NotImplementedException();
            }
        }
        public static int? ScoreAssignOp(string op, int? left, int? right, Scoreboard scoreboard, Scoreboard scoreboard2 ,string? selector, string? selector2)
        {
            if (selector != null && selector2 != null)
            {
                switch (op)
                {
                    case "=":
                        scoreboard.Set(scoreboard2.ScoreboardValues.Name, selector, selector2);
                        return right;

                    case "+=":
                        scoreboard.Add(scoreboard2.ScoreboardValues.Name, selector, selector2);
#pragma warning disable CS8629 // 可為 Null 的實值型別可為 Null。
                        return (int?)ScriptVisitor.Add(scoreboard.ScoreboardValues.Value[selector].Value
                            , scoreboard2.ScoreboardValues.Value[selector2].Value);


                    case "-=":
                        scoreboard.Subtract(scoreboard2.ScoreboardValues.Name, selector, selector2);
                        return (int?)ScriptVisitor.Subtract(scoreboard.ScoreboardValues.Value[selector].Value
                            , scoreboard2.ScoreboardValues.Value[selector2].Value);

                    case "*=":
                        scoreboard.Multiply(scoreboard2.ScoreboardValues.Name, selector, selector2);
                        return (int?)ScriptVisitor.Multiply(scoreboard.ScoreboardValues.Value[selector].Value
                            , scoreboard2.ScoreboardValues.Value[selector2].Value);

                    case "/=":
                        scoreboard.Divide(scoreboard2.ScoreboardValues.Name, selector, selector2);
                        return (int?)ScriptVisitor.Divide(scoreboard.ScoreboardValues.Value[selector].Value
                            , scoreboard2.ScoreboardValues.Value[selector2].Value);

                    case "%=":
                        scoreboard.Remainder(scoreboard2.ScoreboardValues.Name, selector, selector2);
                        return (int?)ScriptVisitor.Remainder(scoreboard.ScoreboardValues.Value[selector].Value
                            , scoreboard2.ScoreboardValues.Value[selector2].Value);

                    default:
                        throw new NotImplementedException();
                }
            }
            else
            {
                throw new NotImplementedException();
            }
#pragma warning restore CS8629 // 可為 Null 的實值型別可為 Null。
        }
    }
}
