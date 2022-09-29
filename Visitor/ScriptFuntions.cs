using Antlr4.Runtime.Misc;
using MCFBuilder.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCFBuilder
{
    public partial class ScriptVisitor
    {
        public override object? VisitAssignFunction(MCFBuilderParser.AssignFunctionContext context)
        {
            var funcName = context.IDENTIFIER(0);
            Dictionary<string, object?> varArgs = new();
            foreach (var item in context.IDENTIFIER().Where(v => v != funcName))
            {
                if (Variables.ContainsKey(item.GetText()))
                    throw new Exception($"Variable '{item.GetText()}' must not be duplicated with existing variables");
                varArgs.Add(item.GetText(), null);
            }
            functionVariables[funcName.GetText()] = varArgs;

            Variables[funcName.GetText()] = new Func<object?[], object?>(args => {
                if (args.Length != varArgs.Count)
                    throw new Exception("Missing Arguments");
                for (int i = 0; i < varArgs.Count; i++)
                {
                    functionVariables[funcName.GetText()][varArgs.ElementAt(i).Key] = args[i];
                }
                Visit(context.block());
                var children = context.block().children.ToList();
                foreach (var child in children)
                {
                    if (child.GetChild(0) != null)
                    {
                        var s = child.GetChild(0).GetChild(0);
                        if (s is MCFBuilderParser.ReturnContext)
                        {
                            return Visit(s);
                        }
                    }
                }
                return null;
            });
            return null;
        }
        public override object? VisitReturn(MCFBuilderParser.ReturnContext context)
        {
            return Visit(context.expression());
        }

        public override object? VisitFunctionCall(MCFBuilderParser.FunctionCallContext context)
        {
            var name = context.IDENTIFIER().GetText();
            var args = context.expression().Select(Visit).ToArray();

            if (ProgramVariables.GlobalVariables.ContainsKey(name))
            {
                if (!ProgramVariables.GlobalVariables.ContainsKey(name))
                    throw new Exception($"'{name}' is not defined");

                if (ProgramVariables.GlobalVariables[name] is not Func<object?[], object?> func)
                    throw new Exception($"Variables {name} is not a function");

                return func(args);
            }
            else if (Variables.ContainsKey(name))
            {
                if (!Variables.ContainsKey(name))
                    throw new Exception($"'{name}' is not defined");

                if (Variables[name] is not Func<object?[], object?> func)
                    throw new Exception($"Variables {name} is not a function");

                if (!ProgramVariables.BuiltInFunctions.Contains(name))
                {
                    if (args.Length == 0)
                    {
                        tempVariables = new Dictionary<string, List<string>>() { [name] = new() };
                    }
                    for (int i = 0; i < args.Length; i++)
                    {
                        var variables = functionVariables[name];
                        functionVariables[name][variables.ElementAt(i).Key] = args[i];
                        tempVariables = new Dictionary<string, List<string>> { [name] = (from v in variables select v.Key).ToList() };
                    }
                }
                return func(args);
            }
            else
            {
                throw new ArgumentException($"{name} is not existed");
            }
            
        }

        public override object? VisitClassFunctionExpression(MCFBuilderParser.ClassFunctionExpressionContext context)
        {


            return null;
        }

        public override object? VisitCreateClassExpression(MCFBuilderParser.CreateClassExpressionContext context)
        {


            return null;
        }
    }
}
