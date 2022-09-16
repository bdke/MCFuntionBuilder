using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using MCFBuilder.Type;
using MCFBuilder.Utility;

namespace MCFBuilder
{
    public static class Execute
    {
        public static void Run(string filePath)
        {
            var fileContents = File.ReadAllText(filePath);
            AntlrInputStream inputStream = new AntlrInputStream(fileContents);
            var lexer = new MCFBuilderLexer(inputStream);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            MCFBuilderParser parser = new MCFBuilderParser(tokens);
            parser.AddErrorListener(new SyntaxErrorListener());
            var context = parser.program();
            FunctionCompiler.Lines = new();
            ScriptVisitor visitor = new ScriptVisitor();
            visitor.Visit(context);
            visitor.RemoveScoreboards();
            //var scoreboardString = from i in ProgramVariables.ScoreboardObjects select $"scoreboard objectives add " +
            //                       $"{i.ScoreboardValues.Name} " +
            //                       $"{ScoreboardValues.GetScoreboardTypes(i.ScoreboardValues.ScoreboardType)}";
            //var scoreboardInitValues = from i in ProgramVariables.ScoreboardObjects select $"scoreboard players set " +
            //                           $"{ProgramVariables.ScoreboardInitValues[i]} " +
            //                           $"{i.ScoreboardValues.Name} " +
            //                           $"{i.ScoreboardValues.Value}";
            //File.WriteAllText("load.mcfunction", String.Join('\n',scoreboardString.Concat(scoreboardInitValues)));
        }

        public static void Main(string[] args)
        {
            Run("Test/test.mcf");
        }
    }
}
