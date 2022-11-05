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
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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

        Dictionary<string, object?> Variables { get; set; } = new();
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
            FunctionCompiler.FileType = FileType.DEFAULT;
            if (currentFile.Contains(':'))
            {
                var type = currentFile.Split(':')[1];
                switch (type)
                {
                    case "temp":
                        FunctionCompiler.FileType = FileType.TEMP;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            FunctionCompiler.Lines = new();
            FunctionCompiler.Lines.Lines = new();
            FunctionCompiler.Lines.FilePath = currentFile;
            Variables.Clear();
            Variables = ProgramVariables.VariablesInit();
            tempVariables.Clear();
            functionVariables.Clear();
            scoreboards.Clear();
            tags.Clear();

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
            if (currentFile != null && FunctionCompiler.FileType == FileType.DEFAULT)
            {
                Logging.Debug($"{Execute.Namespace}/data/{Execute.Namespace}/functions/{currentFile}.mcfunction:\n{string.Join('\n', FunctionCompiler.Lines.Lines)}");
                File.WriteAllText($"{Execute.Namespace}/data/{Execute.Namespace}/functions/" + currentFile + ".mcfunction", string.Join('\n', FunctionCompiler.Lines.Lines));
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

        //public override object? VisitIfBlock(MCFBuilderParser.IfBlockContext context)
        //{
        //    var exp = context.expression();
        //    var type = context.IFTYPES();

        //    int? currentNum = null;

        //    CommandAttribute.IsContainElse = false;
        //    if (type != null)
        //    {
        //        currentNum = IfConditionHandler.Add(Visit(exp), type.GetText());
        //        Visit(context.block());
        //    }
        //    else if (type == null)
        //    {
        //        if (IsTrue(Visit(exp)))
        //        {
        //            Visit(context.block());
        //        }
        //    }


        //    if (context.elseIfBlock() != null)
        //    {
        //        CommandAttribute.IsContainElse = true;
        //        Visit(context.elseIfBlock());
        //    }


        //    IfConditionHandler.Remove(currentNum);

        //    return null;
        //}



        public override object? VisitSelector(MCFBuilderParser.SelectorContext context)
        {
            if (context.SELECTOR() != null)
                return context.SELECTOR().GetText();
            //else if (context.STRING() != null)
            //    return context.STRING().GetText()[1..^1];
            //else if (context.IDENTIFIER() != null)
            //{
            //    var varName = context.IDENTIFIER().GetText();

            //    if (Variables.ContainsKey(varName))
            //    {
            //        if (Variables[varName] is string)
            //            return Variables[varName];
            //        else
            //            throw new InvalidOperationException($"'{varName}' must be a string");
            //    }
            //    else if (ProgramVariables.GlobalVariables.ContainsKey(varName))
            //    {
            //        if (ProgramVariables.GlobalVariables[varName] is string)
            //            return ProgramVariables.GlobalVariables[varName];
            //        else
            //            throw new InvalidOperationException($"'{varName}' must be a string");
            //    }
            //    else
            //    {
            //        throw new NotImplementedException();
            //    }
            //}
            else if (context.expression() != null)
            {
                var value = Visit(context.expression());
                if (value is string s)
                {
                    return s[1..^1];
                }
                else
                {
                    return value.ToString();
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

        public async Task GlobalAssignment([NotNull] MCFBuilderParser.ProgramContext context)
        {
            var lines = context.line();

            FunctionCompiler.Lines = new();
            FunctionCompiler.Lines.Lines = new() { "scoreboard objectives add .number dummy" };
            FunctionCompiler.Lines.FilePath = currentFile;

            //TODO: make it faster
            foreach (var line in lines)
            {
                bool variableMatch = false;
                foreach (var variable in ProgramVariables.GlobalVariables)
                {
                    Regex regex = new Regex($@"\b{variable.Key}\.\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    var results = regex.Matches(line.GetText());
                    if (results.Count > 0)
                    {
                        variableMatch = true;
                    }
                }

                if (line.ToStringTree().Contains("global") || variableMatch)
                {
                    Visit(line);
                }
            }

            Logging.Debug($"{Execute.Namespace}/data/{Execute.Namespace}/functions/load.mcfunction:\n{string.Join('\n', FunctionCompiler.Lines.Lines)}");
            await File.WriteAllTextAsync($"{Execute.Namespace}/data/{Execute.Namespace}/functions/load.mcfunction", string.Join('\n', FunctionCompiler.Lines.Lines));
        }

        public override object? VisitExactVectorExpression([NotNull] MCFBuilderParser.ExactVectorExpressionContext context)
        {
            var num1 = context.expression(0) != null ? Visit(context.expression(0)) : null;
            var num2 = context.expression(1) != null ? Visit(context.expression(1)) : null;
            var num3 = context.expression(2) != null ? Visit(context.expression(2)) : null;



            return $"~{num1} ~{num2} ~{num3}";
        }

        public override object? VisitRelativeVectorExpression([NotNull] MCFBuilderParser.RelativeVectorExpressionContext context)
        {
            var num1 = context.expression(0) != null ? Visit(context.expression(0)) : null;
            var num2 = context.expression(1) != null ? Visit(context.expression(1)) : null;
            var num3 = context.expression(2) != null ? Visit(context.expression(2)) : null;



            return $"^{num1} ^{num2} ^{num3}";
        }

        public override object? VisitSelectorExpressionInExpression([NotNull] MCFBuilderParser.SelectorExpressionInExpressionContext context)
        {
            return context.SELECTOR().GetText();
        }
    }
}
