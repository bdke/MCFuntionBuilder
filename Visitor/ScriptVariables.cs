using Antlr4.Runtime.Misc;
using MCFBuilder.Type;
using MCFBuilder.Utility;
using Serilog;
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
                        , this.Visit(d.expression()[e]));
                }
                return dict;
            }
            if (context.list() is { } l)
            {
                List<object?> list = new();
                foreach (var item in l.expression())
                {
                    list.Add(Visit(item));
                }
                return list;
            }

            throw new NotImplementedException();
        }

        public override object? VisitParenthesizedExpression(MCFBuilderParser.ParenthesizedExpressionContext context)
        {
            return Visit(context.expression());
        }

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
                return from i in value select new { Name = i.Key, Value = i.Value };
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
