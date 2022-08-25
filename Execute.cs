using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;

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
            var context = parser.program();
            ScriptVisitor visitor = new ScriptVisitor();
            visitor.Visit(context);
        }

        public static void Main(string[] args)
        {
            Run("Test/test.mcf");
        }
    }
}
