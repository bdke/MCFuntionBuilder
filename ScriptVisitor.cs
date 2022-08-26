using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCFBuilder
{
    internal class ScriptVisitor : MCFBuilderBaseVisitor<object?>
    {
        Dictionary<string, object?> Variables { get; } = new();
        Dictionary<string, Dictionary<string, object?>> functionVariables { get; set; } = new();
        Dictionary<string,List<string>> tempVariables = new();
        string[] BuiltInFunctions { get; } =
        {
            "Write"
        };

        public ScriptVisitor()
        {
            Variables["Write"] = new Func<object?[], object?>(Write);
        }

        private object? Write(object?[] args)
        {
            foreach (var arg in args)
            {
                Console.WriteLine(arg);
            }

            return null;
        }
        //TODO: get the inputs

        
        public override object? VisitAssignFunction(MCFBuilderParser.AssignFunctionContext context)
        {
            var funcName = context.IDENTIFIER(0);
            Dictionary<string,object?> varArgs = new();
            foreach (var item in context.IDENTIFIER().Where(v => v != funcName))
            {
                if (Variables.ContainsKey(item.GetText()))
                    throw new Exception($"Variable '{item.GetText()}' must not be duplicated with existing variables");
                varArgs.Add(item.GetText(),null);
            }
            functionVariables[funcName.GetText()] = varArgs;
            //tempVariables = (from i in varArgs select i.Key).ToList();

            Variables[funcName.GetText()] = new Func<object?[], object?>(args => {
                if (args.Length != varArgs.Count)
                    throw new Exception("Missing Arguments");
                for (int i = 0; i < varArgs.Count; i++)
                {
                    functionVariables[funcName.GetText()][varArgs.ElementAt(i).Key] = args[i];
                }
                return Visit(context.block());
            });


            return null;
        }

        public override object? VisitFunctionCall(MCFBuilderParser.FunctionCallContext context)
        {
            var name = context.IDENTIFIER().GetText();
            var args = context.expression().Select(Visit).ToArray();

            if (!Variables.ContainsKey(name))
                throw new Exception($"Function {name} is not defined");

            if (Variables[name] is not Func<object?[], object?> func)
                throw new Exception($"Variables {name} is not a function");

            if (!BuiltInFunctions.Contains(name))
            {
                for (int i = 0; i < args.Length; i++)
                {
                    var variables = functionVariables[name];
                    functionVariables[name][variables.ElementAt(i).Key] = args[i];
                    tempVariables = new Dictionary<string, List<string>> { [name] = (from v in variables select v.Key).ToList() };
                }
            }

            return func(args);
        }

        //TODO: let user define 'local' and 'global'
        public override object? VisitAssignment(MCFBuilderParser.AssignmentContext context)
        {
            var varName = context.IDENTIFIER().GetText();
            var value = Visit(context.expression());

            

            if (BuiltInFunctions.Contains(varName))
                throw new Exception($"Unable to modify buily in function {varName}");

            Variables[varName] = value;
            return null;
        }

        public override object? VisitIdentifierExpression(MCFBuilderParser.IdentifierExpressionContext context)
        {
            var varName = context.IDENTIFIER().GetText();

            if (!Variables.ContainsKey(varName))
            {
                foreach (string key in tempVariables.Keys)
                {
                    if (tempVariables[key].Contains(varName))
                    {
                        return functionVariables[key][varName];
                    }
                }
                tempVariables.Clear();
                throw new Exception($"Variables {varName} is not defined");
            }

            return Variables[varName];
        }


        public override object? VisitConstant(MCFBuilderParser.ConstantContext context)
        {
            if (context.FLOAT() is { } f)
                return float.Parse(f.GetText());
            if (context.INTEGER() is { } i) 
                return int.Parse(i.GetText());
            if (context.STRING() is { } s)
                return s.GetText()[1..^1];
            if (context.BOOL() is { } b)
                return b.GetText() == "true";
            if (context.NULL() is { })
                return null;
            if (context.dict() is { } d)
            {
                Dictionary<string, object?> dict = new();
                for (int e = 0; e < d.STRING().Length; e++)
                {
                    dict.Add(d.STRING()[e].GetText()[1..^1]
                        , this.VisitConstant(d.constant()[e]));
                }
                return dict;
            }
            if (context.list() is { } l)
            {
                List<object?> list = new();
                foreach (var item in l.constant())
                {
                    list.Add(VisitConstant(item));
                }
                return list;
            }

            throw new NotImplementedException();
        }

        public override object? VisitAdditiveExpression(MCFBuilderParser.AdditiveExpressionContext context)
        {
            var left = Visit(context.expression(0));
            var right = Visit(context.expression(1));

            var op = context.addOp().GetText();

            return op switch
            {
                "+" => Add(left, right),
                "-" => Subtract(left, right),
                "*" => Multiply(left,right),
                "/" => Divide(left,right),
                "&" => Remainder(left,right),
                _ => throw new NotImplementedException()
            };
        }

        public override object? VisitWhileBlock(MCFBuilderParser.WhileBlockContext context)
        {
            Func<object?, bool> condition = context.WHILE().GetText() == "while" ? IsTrue : IsFalse;


            if (condition(Visit(context.expression())))
            {
                while (condition(Visit(context.expression())))
                {
                    Visit(context.block());
                }
            }

            return null;
        }

        public override object? VisitForBlock(MCFBuilderParser.ForBlockContext context)
        {
            var varName = context.IDENTIFIER(0).GetText();
            if (context.INTEGER().Length > 0)
            {

                var start = int.Parse(context.INTEGER(0).GetText());
                var end = int.Parse(context.INTEGER(1).GetText());

                Variables[varName] = context.INTEGER(0);
                for (int i = start; i < end; i++)
                {
                    Variables[varName] = i;
                    Visit(context.block());
                    
                }
            }
            if (context.dict() is { } d)
            {
                Dictionary<string, object?> dict = new();
                for (int e = 0; e < d.STRING().Length; e++)
                {
                    dict.Add(d.STRING()[e].GetText()[1..^1]
                        , this.VisitConstant(d.constant()[e]));
                }
                foreach (var item in dict.Keys)
                {
                    Variables[varName] = item;
                    Visit(context.block());
                }
            }
            if (context.list() is { } l)
            {
                List<object?> list = new();
                foreach (var item in l.constant())
                {
                    list.Add(VisitConstant(item));
                }
                foreach (var item in list)
                {
                    Variables[varName] = item;
                    Visit(context.block());
                }
            }
            if (context.IDENTIFIER(1) is { } v)
            {
                var iter = Variables[v.GetText()];
                if (iter != null)
                {
                    if (iter is Dictionary<string, object?> dIter)
                    {
                        foreach (var item in dIter.Keys)
                        {
                            Variables[varName] = item;
                            Visit(context.block());
                        }
                    }
                    if (iter is List<object?> lIter)
                    {
                        foreach (var item in lIter)
                        {
                            Variables[varName] = item;
                            Visit(context.block());
                        }
                    }
                }
            }
            return null;
        }

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

        public override object? VisitIfBlock(MCFBuilderParser.IfBlockContext context)
        {
            if (IsTrue(Visit(context.expression())))
            {
                Visit(context.block());
            }
            if (context.elseIfBlock() != null)
            {
                Visit(context.elseIfBlock());
            }
            return null;
        }

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

        private bool IsTrue(object? value)
        {
            if (value is bool b) return b;
            throw new Exception("Value is not boolean");
        }

        private bool IsFalse(object? value) => !IsTrue(value);
    }
}
