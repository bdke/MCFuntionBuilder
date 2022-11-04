using Antlr4.Runtime.Misc;
using MCFBuilder.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using MCFBuilder.Type;
using MCFBuilder.Utility.BuiltIn;

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
            var name = context.IDENTIFIER(0).GetText();
            var args = context.expression().Select(Visit).ToArray();
            var name2 = context.IDENTIFIER(1);

            if (name2 == null)
            {
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
            else
            {
                if (Variables.ContainsKey(name))
                {
                    var _class = (BuiltInClass?)Variables[context.IDENTIFIER(0).GetText()];

                    var method = _class.Methods.FirstOrDefault(v => v.Name == name2.GetText());

                    if (method.Func == null)
                    {
                        throw new InvalidOperationException();
                    }

                    return method.Func.Invoke(_class,new object[] { args });

                }
                else if (ProgramVariables.GlobalVariables.ContainsKey(name))
                {
                    var _class = (BuiltInClass?)ProgramVariables.GlobalVariables[context.IDENTIFIER(0).GetText()];

                    var method = _class.Methods.FirstOrDefault(v => v.Name == name2.GetText());

                    if (method.Func == null)
                    {
                        throw new InvalidOperationException();
                    }

                    return method.Func.Invoke(_class, new object[] { args });

                }
                else if (ProgramVariables.BuiltInClasses.ContainsKey(name))
                {
                    System.Type type = ProgramVariables.BuiltInClasses[name];
                    return type.GetMethod(name2.GetText()).Invoke(null, new object?[] { args });
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        public override object? VisitCreateClassExpression(MCFBuilderParser.CreateClassExpressionContext context)
        {
            var className = context.createClass().IDENTIFIER().GetText();
            var values = context.createClass().expression().Select(Visit).ToArray();

            if (ProgramVariables.BuiltInClasses.Where(v => v.Key == className).Any())
            {
                System.Type type = ProgramVariables.BuiltInClasses.Where(v => v.Key == className).FirstOrDefault().Value;

                var _class = Activator.CreateInstance(type, values);
                Variables.Add(className, _class);
                return _class;
            }

            throw new NotImplementedException();
        }

        public override object? VisitClassVariablesExpression(MCFBuilderParser.ClassVariablesExpressionContext context)
        {
            var _class = (BuiltInClass?)Visit(context.classVariables());
            var name = context.classVariables().IDENTIFIER(1).GetText();

            if (_class != null)
            {
                return _class.GetValue(name);
            }

            return null;
        }

        public override object? VisitClassVariables(MCFBuilderParser.ClassVariablesContext context)
        {
            if (Variables.ContainsKey(context.IDENTIFIER()[0].GetText()))
            {
                return Variables[context.IDENTIFIER()[0].GetText()];
            }

            return null;
        }
    }
}
