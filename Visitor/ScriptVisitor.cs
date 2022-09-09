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

namespace MCFBuilder
{
    public partial class ScriptVisitor : MCFBuilderBaseVisitor<object?>
    {
        string? currentFile = null;
        Dictionary<string, object?> Variables { get; } = new();
        Dictionary<string, Dictionary<string, object?>> functionVariables { get; set; } = new();
        Dictionary<string,List<string>> tempVariables = new();
        List<Scoreboard> scoreboards = new();
        List<string>? tempOperators = null;
        string[] BuiltInFunctions { get; } =
        {
            "Write","LoadFile"
        };

        public ScriptVisitor()
        {
            InitFunctions();
        }

        private object? Write(object?[] args)
        {
            foreach (var arg in args)
            {
                Console.WriteLine(arg);
            }

            return null;
        }

        private async Task<object?> LoadFile(object?[] arg)
        {
            string? result = null;
            if (arg != null)
            {
#pragma warning disable CS8602 // 可能有 Null 參考引數。
#pragma warning disable CS8604 // 可能有 Null 參考引數。
                result = File.ReadAllText(arg[0].ToString());
#pragma warning restore CS8604 // 可能有 Null 參考引數。
#pragma warning restore CS8602 // 可能有 Null 參考引數。
                Console.WriteLine(await CSharpScript.EvaluateAsync(result));
            }
            return result;
        }

        private void InitFunctions()
        {
            Variables["Write"] = new Func<object?[], object?>(Write);
            Variables["LoadFile"] = new Func<object?[], object?>(LoadFile);
        }

        public override object? VisitAssignFile(MCFBuilderParser.AssignFileContext context)
        {
            RemoveScoreboards();
            currentFile = context.GetText().Remove(0,1).Replace(":","");
            FunctionCompiler.Lines = new();
            FunctionCompiler.Lines.Lines = new();
            FunctionCompiler.Lines.FilePath = currentFile;
            Variables.Clear();
            tempVariables.Clear();
            functionVariables.Clear();
            scoreboards.Clear();
            InitFunctions();
            return null;
        }

        public void RemoveScoreboards()
        {
            foreach (var item in scoreboards)
            {
                FunctionCompiler.Lines.Lines.Add($"scoreboard objectives remove {item.ScoreboardValues.Name}");
            }
            if (currentFile != null)
                File.WriteAllText(currentFile + ".mcfunction", string.Join('\n', FunctionCompiler.Lines.Lines));
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

        public override object? VisitIfBlock(MCFBuilderParser.IfBlockContext context)
        {
            var exp = context.expression();

            if (IsTrue(Visit(exp)))
            {
                Visit(context.block());
            }
            if (context.elseIfBlock() != null)
            {
                Visit(context.elseIfBlock());
            }
            return null;
        }
    }
}
