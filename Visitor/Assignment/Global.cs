using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCFBuilder.Utility;
using MCFBuilder.Type;

namespace MCFBuilder
{
    public partial class ScriptVisitor
    {
        public override object? VisitGlobalAssignment(MCFBuilderParser.GlobalAssignmentContext context)
        {
            if (Init)
            {
                var varName = context.IDENTIFIER().GetText();
                if (CheckExists(varName))
                    throw new InvalidOperationException($"'{varName}' is already existed");
                if (context.GetText().Contains('='))
                    ProgramVariables.GlobalVariables[varName] = Visit(context.expression());
                else
                    ProgramVariables.GlobalVariables[varName] = null;
            }
            return null;
        }

        public override object? VisitGlobalScoresAssignment(MCFBuilderParser.GlobalScoresAssignmentContext context)
        {
            if (Init)
            {
                var varName = context.IDENTIFIER().GetText();
                var selector = (string?)Visit(context.selector());
                object? value = null;
                if (context.expression() != null)
                {
                    value = Visit(context.expression());
                }
                CheckExists(varName);
                if (value != null)
                {
                    if (value is int)
                    {
                        var scoreboard = new Scoreboard(
                            new(
                                ScoreboardValues.GetScoreboardTypes("dummy"),
                                new() { [selector] = (int?)Visit(context.expression()) },
                                varName,
                                currentFile,
                                true)
                            );
                        ProgramVariables.ScoreboardObjects.Add(scoreboard);
                        scoreboard.Set(value, selector);
                    }
                    else if (value is string)
                    {
                        ProgramVariables.ScoreboardObjects.Add(new Scoreboard(
                            new(
                                ScoreboardValues.GetScoreboardTypes((string)value),
                                new(),
                                varName,
                                currentFile,
                                false)
                            )
                         );
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                else
                {
                    ProgramVariables.ScoreboardObjects.Add(new Scoreboard(
                        new(
                            ScoreboardValues.GetScoreboardTypes("dummy"),
                            new(),
                            varName,
                            currentFile,
                            true
                        )
                    ));
                }
            }

            return null;
        }

        public override object? VisitGlobalTagsAssignment(MCFBuilderParser.GlobalTagsAssignmentContext context)
        {
            if (Init)
            {
                var selector = (string?)Visit(context.selector());
                var varName = context.IDENTIFIER().GetText();
                var boolean = bool.Parse(context.BOOL().GetText());

                ProgramVariables.Tags.Add(new() { Name = varName, Selector = new(selector), Value = new() { [selector] = boolean } });

                if (boolean)
                {
                    Tags.Add(varName, selector);
                }
            }
            return null;
        }
    }
}
