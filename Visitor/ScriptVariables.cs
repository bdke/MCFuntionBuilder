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

        //public override object? VisitAssignment(MCFBuilderParser.AssignmentContext context)
        //{
        //    #region Datas
        //    string modifier = context.GetChild(0).GetText();
        //    var varName = context.IDENTIFIER(0).GetText();
        //    var value = Visit(context.expression());
        //    var assignOp = context.assignOp().GetText();

        //    string? variableOperation = "";
        //    if (context.selector(1) != null)
        //        variableOperation = context.selector(1).GetText();

        //    string? scoreboardType = null;
        //    if (context.GetChild(2).GetText() == ":")
        //        if (value is not int)
        //            throw new Exception("Scoreboard must be a int");
        //    scoreboardType = (context.IDENTIFIER(1) != null) ? context.IDENTIFIER(1).GetText() : null;

        //    var scoreboardNames = from i in scoreboards
        //                          where i.ScoreboardValues.File == "ROOT" || i.ScoreboardValues.File == currentFile
        //                          select i.ScoreboardValues.Name;

        //    var globalScoreboardNames = from i in ProgramVariables.ScoreboardObjects
        //                                where i.ScoreboardValues.File == "ROOT" || i.ScoreboardValues.File == currentFile
        //                                select i.ScoreboardValues.Name;
        //    #endregion

        //    if (BuiltInFunctions.Contains(varName))
        //        throw new Exception($"Unable to modify buily in function {varName}");

        //    if (modifier != "var" && modifier != "global")
        //    {
        //        //command /tag
        //        if (value is bool valueBool && context.selector(0) != null)
        //        {
        //            var localTags = tags.Where(x => x.Name == varName);
        //            var globalTags = ProgramVariables.Tags.Where(v => v.Name == varName);

        //            if (localTags.Any())
        //            {
        //                localTags.ToList().ForEach(v => v.Value = valueBool);
        //            }
        //            else if (globalTags.Any())
        //            {
        //                globalTags.ToList().ForEach(v => v.Value = valueBool);
        //            }
        //            else
        //            {
        //                throw new NotImplementedException();
        //            }

        //            if (valueBool)
        //                Tags.Add(varName, context.selector(0).GetText());
        //            else
        //                Tags.Remove(varName, context.selector(0).GetText());
                    
        //        }
        //        else if (scoreboardType == null)
        //        {
        //            if (scoreboardNames.Contains(varName))
        //                throw new Exception();

        //            if (ProgramVariables.GlobalVariables.ContainsKey(varName))
        //            {
        //                ProgramVariables.GlobalVariables[varName] = assignOp switch
        //                {
        //                    "=" => value,
        //                    "+=" => Add(ProgramVariables.GlobalVariables[varName], value),
        //                    "-=" => Subtract(ProgramVariables.GlobalVariables[varName], value),
        //                    "*=" => Multiply(ProgramVariables.GlobalVariables[varName], value),
        //                    "%=" => Remainder(ProgramVariables.GlobalVariables[varName], value),
        //                    "/=" => Divide(ProgramVariables.GlobalVariables[varName], value),
        //                    _ => throw new NotImplementedException(),
        //                };
        //            }
        //            else if (Variables.ContainsKey(varName))
        //            {
        //                Variables[varName] = assignOp switch
        //                {
        //                    "=" => value,
        //                    "+=" => Add(Variables[varName] ,value),
        //                    "-=" => Subtract(Variables[varName], value),
        //                    "*=" => Multiply(Variables[varName], value),
        //                    "%=" => Remainder(Variables[varName], value),
        //                    "/=" => Divide(Variables[varName], value),
        //                    _ => throw new NotImplementedException()
        //                };
        //            }
        //            else
        //            {
        //                throw new Exception($"Variable '{varName}' is not existed");
        //            }
        //        }
        //        else
        //        {
        //            if (value is not int)
        //                throw new Exception();

        //            if (scoreboardNames.Contains(varName))
        //            {
        //                var scoreboard = scoreboards.Where(v => v.ScoreboardValues.Name == varName).ElementAt(0);
        //                switch (assignOp)
        //                {
        //                    case "=":
        //                        if (variableOperation.Length > 0)
        //                            scoreboard.Set(context.expression().GetText(), context.selector(0).GetText(), context.selector(1).GetText());
        //                        else
        //                            scoreboard.Set((int?)value, context.selector(0).GetText());
        //                        break;
        //                    case "+=":
        //                        if (variableOperation.Length > 0)
        //                            scoreboard.Add(context.expression().GetText(), context.selector(0).GetText(), context.selector(1).GetText());
        //                        else
        //                            scoreboard.Add((int?)value, context.selector(0).GetText());
        //                        break;
        //                    case "-=":
        //                        if (variableOperation.Length > 0)
        //                            scoreboard.Subtract(context.expression().GetText(), context.selector(0).GetText(), context.selector(1).GetText());
        //                        else
        //                            scoreboard.Subtract((int?)value, context.selector(0).GetText());
        //                        break;
        //                    case "*=":
        //                        if (variableOperation.Length > 0)
        //                            scoreboard.Multiply(context.expression().GetText(), context.selector(0).GetText(), context.selector(1).GetText());
        //                        else
        //                            scoreboard.Multiply((int?)value, context.selector(0).GetText());
        //                        break;
        //                    case "/=":
        //                        if (variableOperation.Length > 0)
        //                            scoreboard.Divide(context.expression().GetText(), context.selector(0).GetText(), context.selector(1).GetText());
        //                        else
        //                            scoreboard.Divide((int?)value, context.selector(0).GetText());
        //                        break;
        //                    case "%=":
        //                        if (variableOperation.Length > 0)
        //                            scoreboard.Remainder(context.expression().GetText(), context.selector(0).GetText(), context.selector(1).GetText());
        //                        else
        //                            scoreboard.Remainder((int?)value, context.selector(0).GetText());
        //                        break;
        //                    default:
        //                        break;
        //                }

        //                scoreboard.ScoreboardValues.Value = assignOp switch
        //                {
        //                    "=" => (int?)value,
        //                    "+=" => scoreboard.ScoreboardValues.Value + (int)value,
        //                    "-=" => scoreboard.ScoreboardValues.Value - (int)value,
        //                    "*=" => scoreboard.ScoreboardValues.Value * (int)value,
        //                    "/=" => scoreboard.ScoreboardValues.Value / (int)value,
        //                    "%=" => scoreboard.ScoreboardValues.Value % (int)value,
        //                    _ => throw new NotImplementedException()
        //                };
        //            }
        //            else if (globalScoreboardNames.Contains(varName))
        //            {
        //                var scoreboard =
        //                ProgramVariables.ScoreboardObjects
        //                    .Where(v => v.ScoreboardValues.Name == varName)
        //                    .ElementAt(0);

        //                switch (assignOp)
        //                {
        //                    case "=":
        //                        if (variableOperation.Length > 0)
        //                            scoreboard.Set(context.expression().GetText(), context.selector(0).GetText(), context.selector(1).GetText());
        //                        else
        //                            scoreboard.Set((int?)value, context.selector(0).GetText());
        //                        break;
        //                    case "+=":
        //                        if (variableOperation.Length > 0)
        //                            scoreboard.Add(context.expression().GetText(), context.selector(0).GetText(), context.selector(1).GetText());
        //                        else
        //                            scoreboard.Add((int?)value, context.selector(0).GetText());
        //                        break;
        //                    case "-=":
        //                        if (variableOperation.Length > 0)
        //                            scoreboard.Subtract(context.expression().GetText(), context.selector(0).GetText(), context.selector(1).GetText());
        //                        else
        //                            scoreboard.Subtract((int?)value, context.selector(0).GetText());
        //                        break;
        //                    case "*=":
        //                        if (variableOperation.Length > 0)
        //                            scoreboard.Multiply(context.expression().GetText(), context.selector(0).GetText(), context.selector(1).GetText());
        //                        else
        //                            scoreboard.Multiply((int?)value, context.selector(0).GetText());
        //                        break;
        //                    case "/=":
        //                        if (variableOperation.Length > 0)
        //                            scoreboard.Divide(context.expression().GetText(), context.selector(0).GetText(), context.selector(1).GetText());
        //                        else
        //                            scoreboard.Divide((int?)value, context.selector(0).GetText());
        //                        break;
        //                    case "%=":
        //                        if (variableOperation.Length > 0)
        //                            scoreboard.Remainder(context.expression().GetText(), context.selector(0).GetText(), context.selector(1).GetText());
        //                        else
        //                            scoreboard.Remainder((int?)value, context.selector(0).GetText());
        //                        break;
        //                    default:
        //                        break;
        //                }

        //                scoreboard.ScoreboardValues.Value = assignOp switch
        //                {
        //                    "=" => (int?)value,
        //                    "+=" => scoreboard.ScoreboardValues.Value + (int)value,
        //                    "-=" => scoreboard.ScoreboardValues.Value - (int)value,
        //                    "*=" => scoreboard.ScoreboardValues.Value * (int)value,
        //                    "/=" => scoreboard.ScoreboardValues.Value / (int)value,
        //                    "%=" => scoreboard.ScoreboardValues.Value % (int)value,
        //                    _ => throw new NotImplementedException()
        //                };

        //            }
        //            else
        //            {
        //                throw new NotImplementedException();
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (value is bool valueBool && context.selector(0) != null)
        //        {
        //            var localTags = tags.Where(x => x.Name == varName);
        //            var globalTags = ProgramVariables.Tags.Where(x => x.Name == varName);

        //            if (localTags.Any() || globalTags.Any())
        //            {
        //                throw new Exception($"variable {varName} already exists");
        //            }

        //            if (modifier == "var")
        //            {
        //                tags.Add(new() { 
        //                    Name = varName, 
        //                    Value = valueBool, 
        //                    Selector = context.selector(0).GetText() 
        //                });
        //            }
        //            else
        //            {
        //                ProgramVariables.Tags.Add(new() { 
        //                    Name = varName,
        //                    Value = valueBool, 
        //                    Selector = context.selector(0).GetText() 
        //                });
        //            }
        //            if (valueBool)
        //                Tags.Add(varName, context.selector(0).GetText());
        //            else
        //                Tags.Remove(varName, context.selector(0).GetText());
        //        }
        //        if (scoreboardType == null)
        //        {
        //            if (modifier == "var")
        //            {
        //                if (Variables.ContainsKey(varName))
        //                    throw new Exception();
        //                Variables[varName] = value;
        //            }
        //            else if (modifier == "global")
        //            {
        //                if (ProgramVariables.GlobalVariables.ContainsKey(varName))
        //                    throw new Exception();
        //                ProgramVariables.GlobalVariables[varName] = value;
        //            }
        //            else
        //            {
        //                throw new NotImplementedException();
        //            }
        //        }
        //        else
        //        {
        //            if (modifier == "var")
        //            {
        //                //test if the scoreboard already exists
        //                if (scoreboards.Where(v => v.ScoreboardValues.Name == varName).Count() > 0)
        //                    throw new Exception();
        //                var scoreboardValues = new ScoreboardValues(ScoreboardValues.GetScoreboardTypes(scoreboardType), (int?)value, varName, currentFile);
        //                Scoreboard scoreboard = new(scoreboardValues);

                        
        //                switch (assignOp)
        //                {
        //                    case "=":
        //                        if (variableOperation.Length > 0)
        //                            scoreboard.Set(context.expression().GetText(), context.selector(0).GetText(), context.selector(1).GetText());
        //                        break;
        //                    case "+=":
        //                        if (variableOperation.Length > 0)
        //                            scoreboard.Add(context.expression().GetText(), context.selector(0).GetText(), context.selector(1).GetText());
        //                        break;
        //                    case "-=":
        //                        if (variableOperation.Length > 0)
        //                            scoreboard.Subtract(context.expression().GetText(), context.selector(0).GetText(), context.selector(1).GetText());
        //                        break;
        //                    case "*=":
        //                        if (variableOperation.Length > 0)
        //                            scoreboard.Multiply(context.expression().GetText(), context.selector(0).GetText(), context.selector(1).GetText());
        //                        break;
        //                    case "/=":
        //                        if (variableOperation.Length > 0)
        //                            scoreboard.Divide(context.expression().GetText(), context.selector(0).GetText(), context.selector(1).GetText());
        //                        break;
        //                    case "%=":
        //                        if (variableOperation.Length > 0)
        //                            scoreboard.Remainder(context.expression().GetText(), context.selector(0).GetText(), context.selector(1).GetText());
        //                        break;
        //                    default:
        //                        break;
        //                }
        //                scoreboards.Add(scoreboard);
        //                if (value != null)
        //                {
        //                    scoreboard.Set((int?)value, context.selector(0).GetText());
        //                }
        //            }
        //            else if (modifier == "global")
        //            {
        //                if (scoreboards.Where(v => v.ScoreboardValues.Name == varName).Count() > 0)
        //                    throw new Exception();
        //                ScoreboardValues scoreboardValues = new(ScoreboardValues.GetScoreboardTypes(scoreboardType), (int?)value, varName, ScoreboardValues.RootPath);
        //                Scoreboard scoreboard = new(scoreboardValues);
        //                switch (assignOp)
        //                {
        //                    case "=":
        //                        if (variableOperation.Length > 0)
        //                            scoreboard.Set(context.expression().GetText(), context.selector(0).GetText(), context.selector(1).GetText());
        //                        break;
        //                    case "+=":
        //                        if (variableOperation.Length > 0)
        //                            scoreboard.Add(context.expression().GetText(), context.selector(0).GetText(), context.selector(1).GetText());
        //                        break;
        //                    case "-=":
        //                        if (variableOperation.Length > 0)
        //                            scoreboard.Subtract(context.expression().GetText(), context.selector(0).GetText(), context.selector(1).GetText());
        //                        break;
        //                    case "*=":
        //                        if (variableOperation.Length > 0)
        //                            scoreboard.Multiply(context.expression().GetText(), context.selector(0).GetText(), context.selector(1).GetText());
        //                        break;
        //                    case "/=":
        //                        if (variableOperation.Length > 0)
        //                            scoreboard.Divide(context.expression().GetText(), context.selector(0).GetText(), context.selector(1).GetText());
        //                        break;
        //                    case "%=":
        //                        if (variableOperation.Length > 0)
        //                            scoreboard.Remainder(context.expression().GetText(), context.selector(0).GetText(), context.selector(1).GetText());
        //                        break;
        //                    default:
        //                        break;
        //                }
        //                ProgramVariables.ScoreboardObjects.Add(scoreboard);

        //                if (value != null)
        //                {
        //                    ProgramVariables.ScoreboardInitValues[scoreboard] = (string?)context.selector(0).GetText();
        //                }
        //            }
        //            else
        //            {
        //                throw new NotImplementedException();
        //            }
        //        }
        //    }
        //    return null;
        //}

        public override object? VisitIdentifierExpression(MCFBuilderParser.IdentifierExpressionContext context)
        {
            var varName = context.IDENTIFIER().GetText();

            if (ProgramVariables.GlobalVariables.ContainsKey(varName))
            {
                return ProgramVariables.GlobalVariables[varName];
            }

            if (scoreboards.Where(v => v.ScoreboardValues.Name == varName).Any())
            {
                var value = scoreboards.Where(v => v.ScoreboardValues.Name == varName).ToList().First().ScoreboardValues.Value;
                return string.Join('\n',(from i in value select $"Selector: {i.Key}, Value: {i.Value}"));
            }

            if (ProgramVariables.ScoreboardObjects.Where(v => v.ScoreboardValues.Name == varName).Count() > 0)
            {
                return ProgramVariables.ScoreboardObjects.Where(v => v.ScoreboardValues.Name == varName).ElementAt(0).ScoreboardValues.Value;
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
