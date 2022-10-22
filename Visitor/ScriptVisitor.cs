using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using MCFBuilder.Utility;
using MCFBuilder.Type;
using Antlr4.Runtime.Tree;
using MCFBuilder.Visitor.BuiltInFuntions;
using System.Reflection;
using MCFBuilder.Type.Compiler;
using System.Xml.Linq;

namespace MCFBuilder
{
    public partial class ScriptVisitor : MCFBuilderBaseVisitor<object?>
    {
        private static string? currentFile = null;
        public static string? CurrentFile
        {
            get
            {
                return currentFile;
            }
        }

        Dictionary<string, object?> Variables { get; } = new();
        Dictionary<string, Dictionary<string, object?>> functionVariables { get; set; } = new();
        Dictionary<string,List<string>> tempVariables = new();
        List<Scoreboard> scoreboards = new();
        List<string>? tempOperators = null;
        List<TagsType> tags = new();

        public bool Init = false;

        public ScriptVisitor()
        {

        }

        //public override object? VisitAssignFile()
        //{
        //    FunctionEndAction();
        //    currentFile = context.GetText().Remove(0,1).Replace(":","");
        //    FunctionCompiler.Lines = new();
        //    FunctionCompiler.Lines.Lines = new();
        //    FunctionCompiler.Lines.FilePath = currentFile;
        //    Variables.Clear();
        //    tempVariables.Clear();
        //    functionVariables.Clear();
        //    scoreboards.Clear();
        //    return null;
        //}

        public override object? VisitAssignFile(MCFBuilderParser.AssignFileContext context)
        {
            FunctionEndAction();
            currentFile = context.GetText()[1..];
            FunctionCompiler.Lines = new();
            FunctionCompiler.Lines.Lines = new();
            FunctionCompiler.Lines.FilePath = currentFile;
            Variables.Clear();
            tempVariables.Clear();
            functionVariables.Clear();
            scoreboards.Clear();

            return null;
        }

        public void FunctionEndAction()
        {
            foreach (var item in scoreboards)
            {
                FunctionCompiler.Lines.Lines.Add($"scoreboard objectives remove {item.ScoreboardValues.Name}");
            }

            foreach (var item in tags)
            {
                var selectors = from i in item.Value where i.Value select i.Key;
                foreach (string selector in selectors)
                {
                    FunctionCompiler.Lines.Lines.Add($"tag {selector} remove {item.Name}");
                }
                
            }
            if (currentFile != null)
            {
                Logging.Debug($"{Execute.Namespace}/data/{Execute.Namespace}/functions/{currentFile}.mcfunction {string.Join('\n', FunctionCompiler.Lines.Lines)}");
                File.WriteAllText($"{Execute.Namespace}/data/{Execute.Namespace}/functions/" + currentFile + ".mcfunction:\n", string.Join('\n', FunctionCompiler.Lines.Lines));
            }
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

            bool isInsideFunc = false;
            var parent = context.parent;
            while (parent != null)
            {
                if (parent.parent is MCFBuilderParser.AssignFunctionContext)
                {
                    isInsideFunc = true;
                    parent = parent.parent;
                    break;
                }
                parent = parent.parent;
            }

            if (context.INTEGER().Length > 0)
            {
                var start = int.Parse(context.INTEGER(0).GetText());
                var end = int.Parse(context.INTEGER(1).GetText());

                //Variables[varName] = context.INTEGER(0);
                for (int i = start; i < end; i++)
                {
                    if (!isInsideFunc)
                        Variables[varName] = i;
                    else if (parent != null)
                    {
                        tempVariables[parent.GetChild(1).GetText()].Add(varName);
                        functionVariables[parent.GetChild(1).GetText()][varName] = i;
                    }
                    Visit(context.block());
                    
                }
            }
            if (context.dict() is { } d)
            {
                Dictionary<string, object?> dict = new();
                for (int e = 0; e < d.STRING().Length; e++)
                {
                    dict.Add(d.STRING()[e].GetText()[1..^1]
                        , this.Visit(d.expression()[e]));
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
                foreach (var item in l.expression())
                {
                    list.Add(Visit(item));
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

        public override object? VisitIfBlock(MCFBuilderParser.IfBlockContext context)
        {
            var exp = context.expression();
            var type = context.IFTYPES();

            int? currentNum = null;

            CommandAttribute.IsContainElse = false;
            if (Visit(exp) is string s)
            {
                currentNum = IfConditionHandler.Add(s, type.GetText());
                Visit(context.block());
            }
            else if (type == null)
            {
                if (IsTrue(Visit(exp)))
                {
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



        public override object? VisitSelector(MCFBuilderParser.SelectorContext context)
        {
            if (context.SELECTOR() != null)
                return context.SELECTOR().GetText();
            else if ((context.STRING() != null))
                return context.STRING().GetText()[1..^1];
            else if (context.IDENTIFIER() != null)
            {
                var varName = context.IDENTIFIER().GetText();

                if (Variables.ContainsKey(varName))
                {
                    if (Variables[varName] is string)
                        return Variables[varName];
                    else
                        throw new InvalidOperationException($"'{varName}' must be a string");
                }
                else if (ProgramVariables.GlobalVariables.ContainsKey(varName))
                {
                    if (ProgramVariables.GlobalVariables[varName] is string)
                        return ProgramVariables.GlobalVariables[varName];
                    else
                        throw new InvalidOperationException($"'{varName}' must be a string");
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
                throw new NotImplementedException();
        }

        public override object? VisitGetIdentifierDataExpression([NotNull] MCFBuilderParser.GetIdentifierDataExpressionContext context)
        {
            var target = Visit(context.expression(0));
            var value = Visit(context.expression(1));

            if (target is List<object?> list)
            {
                if (value is int i)
                {
                    return list[i];
                }
            }
            else if (target is Dictionary<object, object?> dict)
            {
                return dict[value];
            }

            throw new NotImplementedException();
        }

        public override object? VisitCommand(MCFBuilderParser.CommandContext context)
        {
            FunctionCompiler.Lines.Lines.Add(context.GetText()[1..]);


            return null;
        }

        public override object? VisitProgram([NotNull] MCFBuilderParser.ProgramContext context)
        {
            FunctionCompiler.Lines = new();
            FunctionCompiler.Lines.Lines = new();
            return base.VisitProgram(context);
        }

        public void GlobalAssignment([NotNull] MCFBuilderParser.ProgramContext context)
        {
            var lines = context.line();

            FunctionCompiler.Lines = new();
            FunctionCompiler.Lines.Lines = new() { "scoreboard objectives add .number dummy" };
            FunctionCompiler.Lines.FilePath = currentFile;

            foreach (var line in lines)
            {
                if (line.ToStringTree().Contains("global"))
                {
                    Visit(line);
                }
            }
            
            File.WriteAllText("load.mcfunction", string.Join('\n', FunctionCompiler.Lines.Lines));
        }
    }
}
