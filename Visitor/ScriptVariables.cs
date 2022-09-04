using MCFBuilder.Type;
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
        public override object? VisitConstant(MCFBuilderParser.ConstantContext context)
        {
            if (context.FLOAT() is { } f)
                return float.Parse(f.GetText());
            if (context.INTEGER() is { } i)
                return int.Parse(i.GetText());
            if (context.STRING() is { } s)
                return s.GetText()[1..^1];
            if (context.BOOL() is { } b)
                return (context.GetChild(0).GetText() == "!") ? !(b.GetText() == "true") : b.GetText() == "true";
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

        public override object? VisitParenthesizedExpression(MCFBuilderParser.ParenthesizedExpressionContext context)
        {
            return Visit(context.expression());
        }

        public override object? VisitAssignment(MCFBuilderParser.AssignmentContext context)
        {
            string modifier = context.GetChild(0).GetText();
            var varName = context.IDENTIFIER(0).GetText();
            var value = Visit(context.expression());

            string? scoreboardType = null;
            if (context.GetChild(2).GetText() == ":")
                if (value is not int)
                    throw new Exception("Scoreboard must be a int");
            scoreboardType = (context.IDENTIFIER(1) != null) ? context.IDENTIFIER(1).GetText() : null;

            var scoreboardNames = from i in scoreboardValues
                                  where i.Modifier == "ROOT" || i.Modifier == currentFile
                                  select i.Name;



            if (BuiltInFunctions.Contains(varName))
                throw new Exception($"Unable to modify buily in function {varName}");

            if (modifier != "var" && modifier != "global")
            {
                if (scoreboardType == null)
                {
                    if (scoreboardNames.Contains(varName))
                        throw new Exception();

                    if (ProgramVariables.globalVariables.ContainsKey(varName))
                    {
                        ProgramVariables.globalVariables[varName] = value;
                    }
                    else if (Variables.ContainsKey(varName))
                    {
                        Variables[varName] = value;
                    }
                    else
                    {

                        throw new Exception($"Variable '{varName}' is not existed");
                    }
                }
                else
                {
                    if (value is not int)
                        throw new Exception();

                    if (scoreboardNames.Contains(varName))
                    {
                        scoreboardValues.Where(v => v.Name == varName).ElementAt(0).Value = (int?)value;
                    }
                }
            }
            else
            {
                if (scoreboardType == null)
                {
                    if (modifier == "var")
                    {
                        if (Variables.ContainsKey(varName))
                            throw new Exception();
                        Variables[varName] = value;
                    }
                    else if (modifier == "global")
                    {
                        if (ProgramVariables.globalVariables.ContainsKey(varName))
                            throw new Exception();
                        ProgramVariables.globalVariables[varName] = value;
                    }
                    else
                    {

                    }
                }
                else
                {
                    if (modifier == "var")
                    {
                        if (scoreboardValues.Where(v => v.Name == varName).Count() > 0)
                            throw new Exception();
                        scoreboardValues.Add(new(ScoreboardValues.GetScoreboardTypes(scoreboardType), (int?)value, varName, currentFile, "="));
                    }
                    else if (modifier == "global")
                    {
                        if (scoreboardValues.Where(v => v.Name == varName).Count() > 0)
                            throw new Exception();
                        scoreboardValues.Add(new(ScoreboardValues.GetScoreboardTypes(scoreboardType), (int?)value, varName, ScoreboardValues.RootPath, "="));
                    }
                    else
                    {

                    }
                }
            }
            return null;
        }

        public override object? VisitIdentifierExpression(MCFBuilderParser.IdentifierExpressionContext context)
        {
            var varName = context.IDENTIFIER().GetText();

            if (ProgramVariables.globalVariables.ContainsKey(varName))
            {
                return ProgramVariables.globalVariables[varName];
            }

            if (scoreboardValues.Where(v => v.Name == varName).Count() > 0)
            {
                return scoreboardValues.Where(v => v.Name == varName).ToList()[0].Value;
            }

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
                throw new Exception($"Variables '{varName}' is not defined");
            }

            return Variables[varName];
        }
    }
}
