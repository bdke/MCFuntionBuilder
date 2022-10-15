using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCFBuilder.Utility;
using MCFBuilder.Type;
using MCFBuilder.Type.Compiler;
#pragma warning disable CS8604 // 可能有 Null 參考引數。
namespace MCFBuilder
{
    public partial class ScriptVisitor
    {
        public override object? VisitLocalAssignment(MCFBuilderParser.LocalAssignmentContext context)
        {
            var varName = context.IDENTIFIER().GetText();
            if (CheckExists(varName))
                throw new InvalidOperationException($"'{varName}' is already existed");
            if (context.GetText().Contains('='))
                Variables[varName] = Visit(context.expression());
            else
                Variables[varName] = null;
            return null;
        }

        public override object? VisitLocalScoresAssignment(MCFBuilderParser.LocalScoresAssignmentContext context)
        {
            var varName = context.IDENTIFIER().GetText();
            var selector = (context.selector() != null) ? (string?)Visit(context.selector()) : "";
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
                        scoreboards.Add(new Scoreboard(
                            new(
                                ScoreboardValues.GetScoreboardTypes("dummy"),
                                new() { [selector] = (int?)Visit(context.expression()) },
                                varName,
                                currentFile,
                                true)
                            )
                         );
                    if ((int?)Visit(context.expression()) != null)
                        FunctionCompiler.Lines
                            .Lines
                            .Add(
                                new($"{CommandAttribute.Compile()}scoreboard players set " +
                                $"{selector} " +
                                $"{varName} " +
                                $"{(int?)Visit(context.expression())}")
                            );

                }
                else if (value is string)
                {
                        scoreboards.Add(new Scoreboard(
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
                scoreboards.Add(new Scoreboard(
                    new(
                        ScoreboardValues.GetScoreboardTypes("dummy"),
                        new(),
                        varName,
                        currentFile,
                        true
                    )
                ));
            }
            return null;
        }

        public override object? VisitOperation(MCFBuilderParser.OperationContext context)
        {
            var varName = context.IDENTIFIER().GetText();
            var assignOp = context.assignOp().GetText();
            var selector = context.selector();
            var exp = context.expression();
            if (!CheckExists(varName))
                throw new InvalidOperationException($"'{varName}' is not existed");



            //normal variables
            if (Variables.ContainsKey(varName))
            {
                Variables[varName] = AssignmentHelper.AssignOp(assignOp,(int?)Visit(context.IDENTIFIER()),(int?)Visit(context.expression()));
            }
            else if (ProgramVariables.GlobalVariables.ContainsKey(varName))
            {
                ProgramVariables.GlobalVariables[varName] = AssignmentHelper.AssignOp(assignOp, (int?)Visit(context.IDENTIFIER()), (int?)Visit(context.expression()));
            }

            //tags variables
            else if (tags.Where(v => v.Name == varName).Any() && selector != null)
            {
                var value = Visit(exp);
                if (value is bool b)
                {
                    tags.Where(v => v.Name == varName).ToList().First().Value[(string?)Visit(selector)] = b;

                    if (b)
                        Tags.Add(varName, (string?)Visit(selector));
                    else
                        Tags.Remove(varName, (string?)Visit(selector));
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            else if (ProgramVariables.Tags.Where(v => v.Name == varName).Any() && selector != null)
            {
                var value = Visit(exp);
                if (value is bool b)
                {
                    ProgramVariables.Tags.Where(v => v.Name == varName).ToList().First().Value[(string?)Visit(selector)] = b;

                    if (b)
                        Tags.Add(varName, (string?)Visit(selector));
                    else
                        Tags.Remove(varName, (string?)Visit(selector));
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }

            //scores variables
            else if (scoreboards.Where(v => v.ScoreboardValues.Name == varName).Any() && selector != null)
            {
                var scoreboard = scoreboards.Where(v => v.ScoreboardValues.Name == varName).First();
                var sel = (string?)Visit(selector);

                if (!scoreboard.ScoreboardValues.Value.ContainsKey(sel))
                    scoreboard.ScoreboardValues.Value[sel] = (int?)Visit(exp);

                if (!scoreboard.ScoreboardValues.IsModify)
                    throw new InvalidOperationException($"'{varName}' can not be modified");
                var value = AssignmentHelper.ScoreAssignOp(
                                                            assignOp,
                                                            scoreboard.ScoreboardValues.Value[sel],
                                                            (int?)Visit(exp),
                                                            scoreboard,
                                                            (string?)Visit(selector)
                                                          );
                scoreboards.Where(v => v.ScoreboardValues.Name == varName).ToList().First().ScoreboardValues.Value[(string?)Visit(selector)] = value;
            }

            else if (ProgramVariables.ScoreboardObjects.Where(v => v.ScoreboardValues.Name == varName).Any() && selector != null)
            {
                var scoreboard = ProgramVariables.ScoreboardObjects.Where(v => v.ScoreboardValues.Name == varName).First();
                var sel = (string?)Visit(selector);

                if (!scoreboard.ScoreboardValues.Value.ContainsKey(sel))
                    scoreboard.ScoreboardValues.Value[sel] = (int?)Visit(exp);

                if (!scoreboard.ScoreboardValues.IsModify)
                    throw new InvalidOperationException($"'{varName}' can not be modified");
                var value = AssignmentHelper.ScoreAssignOp(
                                                            assignOp,
                                                            scoreboard.ScoreboardValues.Value[(string?)Visit(selector)],
                                                            (int?)Visit(exp),
                                                            scoreboard,
                                                            (string?)Visit(selector)
                                                          );
                scoreboards.Where(v => v.ScoreboardValues.Name == varName).ToList().First().ScoreboardValues.Value[(string?)Visit(selector)] = value;
            }

            else
            {
                throw new NotImplementedException();
            }
            return null;
        }

        //public override object? VisitScoresOperation(MCFBuilderParser.ScoresOperationContext context)
        //{
        //    var varName = context.IDENTIFIER().GetText();
        //    var assignOp = context.assignOp().GetText();
        //    var selector = (string?)Visit(context.selector());
        //    var expValue = Visit(context.expression());

        //    if (!CheckExists(varName))
        //        throw new InvalidOperationException($"'{varName}' is not existed");

        //    var scoreboard = scoreboards.Where(v => v.ScoreboardValues.Name == varName).ToList().First();
        //    if (!scoreboard.ScoreboardValues.IsModify)
        //        throw new InvalidOperationException($"'{varName}' can not be modified");
        //    var value = AssignmentHelper.ScoreAssignOp(
        //                                                assignOp, 
        //                                                scoreboard.ScoreboardValues.Value[selector],
        //                                                ((int?)expValue),
        //                                                scoreboard, 
        //                                                selector
        //                                              );
        //    scoreboards.Where(v => v.ScoreboardValues.Name == varName).ToList().First().ScoreboardValues.Value[selector] = value;

        //    return null;
        //}

        public override object? VisitScoresEqual(MCFBuilderParser.ScoresEqualContext context)
        {
            var varName = context.IDENTIFIER(0).GetText();
            var targetName = context.IDENTIFIER(1).GetText();

            var assignOp = context.assignOp().GetText();

            var selector = (string?)Visit(context.selector(0));
            var selector2 = (string?)Visit(context.selector(1));

            if (!CheckExists(varName) || !CheckExists(targetName))
                throw new InvalidOperationException($"'{varName}' is not existed");

            var scoreboard = scoreboards.Where(v => v.ScoreboardValues.Name == varName).ToList().FirstOrDefault();
            var globalScoreboard = ProgramVariables.ScoreboardObjects.Where(v => v.ScoreboardValues.Name == varName).ToList().FirstOrDefault();

            int? value = null;

            if (scoreboard != null)
            {
                var scoreboard2 = scoreboards.Where(v => v.ScoreboardValues.Name == targetName).FirstOrDefault();
                if (scoreboard2 != null)
                {
                    value = AssignmentHelper.ScoreAssignOp(
                            assignOp,
                            (int?)Visit(context.IDENTIFIER(0)),
                            (int?)Visit(context.IDENTIFIER(1)),
                            scoreboard,
                            scoreboard2,
                            selector,
                            selector2
                        );
                }
            }
            else if (globalScoreboard != null)
            {
                var scoreboard2 = ProgramVariables.ScoreboardObjects.Where(v => v.ScoreboardValues.Name == targetName).FirstOrDefault();
                if (scoreboard2 != null)
                {
                    value = AssignmentHelper.ScoreAssignOp(
                            assignOp,
                            (int?)Visit(context.IDENTIFIER(0)),
                            (int?)Visit(context.IDENTIFIER(1)),
                            globalScoreboard,
                            scoreboard2,
                            selector,
                            selector2
                        );
                }
            }

            scoreboards.Where(v => v.ScoreboardValues.Name == varName).ToList().First().ScoreboardValues.Value[selector] = value;

            return null;
        }

        public override object? VisitLocalTagsAssignment(MCFBuilderParser.LocalTagsAssignmentContext context)
        {
            var selector = (string?)Visit(context.selector());
            var varName = context.IDENTIFIER().GetText();
            var boolean = bool.Parse(context.BOOL().GetText());

            tags.Add(new() { Name = varName, Selector = new(selector), Value = new() { [selector] = boolean } });

            if (boolean)
            {
                Tags.Add(varName, selector);
            }

            return null;
        }

        //public override object? VisitTagsOperation(MCFBuilderParser.TagsOperationContext context)
        //{
        //    var selector = (string?)Visit(context.selector());
        //    var varName = context.IDENTIFIER().GetText();
        //    var boolean = bool.Parse(context.BOOL().GetText());

        //    var localTags = tags.Where(v => v.Name == varName).ToList();
        //    var globalTags = ProgramVariables.Tags.Where(v => v.Name == varName).ToList();

        //    if (localTags.Any())
        //    {
        //        tags.Where(v => v.Name == varName).ToList().First().Value[selector] = boolean ;

        //        if (boolean)
        //            Tags.Add(varName, selector);
        //        else
        //            Tags.Remove(varName, selector);
        //    }
        //    else if (globalTags.Any())
        //    {
        //        ProgramVariables.Tags.Where(v => v.Name == varName).ToList().First().Value[selector] = boolean ;
        //        if (boolean)
        //            Tags.Add(varName, selector);
        //        else
        //            Tags.Remove(varName, selector);
        //    }
        //    else
        //    {
        //        throw new InvalidOperationException($"'{varName}' is not existed");
        //    }

        //    return null;
        //}

        private bool CheckExists(string name)
        {
            if (
                Variables.ContainsKey(name) 
                || scoreboards.Where(v => v.ScoreboardValues.Name == name).Any() 
                || ProgramVariables.ScoreboardObjects.Where(v => v.ScoreboardValues.Name == name).Any()
                || ProgramVariables.GlobalVariables.ContainsKey(name)
                || tags.Where(v => v.Name == name).Any()
                || ProgramVariables.Tags.Where(v => v.Name == name).Any()
                )
                return true;
            else return false;
        }
    }
}
#pragma warning restore CS8604