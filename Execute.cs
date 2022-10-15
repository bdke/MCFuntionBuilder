using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using MCFBuilder.Type;
using MCFBuilder.Utility;
using System.Text.Json;
using System.Runtime.CompilerServices;

namespace MCFBuilder
{
    public static class Execute
    {
        public static string? Namespace { get; set; }
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

        private static void LoadGlobal(string filePath)
        {
            var fileContents = File.ReadAllText(filePath);
            AntlrInputStream inputStream = new AntlrInputStream(fileContents);
            var lexer = new MCFBuilderLexer(inputStream);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            MCFBuilderParser parser = new MCFBuilderParser(tokens);
            ScriptVisitor visitor = new ScriptVisitor();
            visitor.Init = true;
            visitor.GlobalAssignment(parser.program());
            visitor.Init = false;
        }

        public static async Task<int> Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Input arguments must contain at least 1");
                Console.ResetColor();


                return 1;
            }

            string command = args[0];

            //Run("Test/test.mcf");
            if (command == "compile")
            {
                ////TODO: Actions during running
                //string[] allfiles = Directory.GetFiles("./", "*.*", SearchOption.AllDirectories);
                //var targets = allfiles.Where(v => v.EndsWith(".mcf"));

                string[] namespaces = Directory.GetFiles(".", "*.*", SearchOption.AllDirectories)
                    .Where(v => v.EndsWith(".mcfconfig"))
                    .ToArray();

                foreach (string ns in namespaces)
                {
                    DatapackData datapackData = JsonSerializer.Deserialize<DatapackData>(File.ReadAllText(ns));

                    Namespace = datapackData.Name;

                    foreach (string file in datapackData.FilesPath)
                    {
                        LoadGlobal(file);
                    }

                    foreach (string file in datapackData.FilesPath)
                    {
                        Run(file);
                    }
                }


                //foreach (var target in targets)
                //{
                //    Run(target);
                //}
            }
            else if (command == "new")
            {
                if (args.Length < 3)
                {
                    if (args.Length == 2)
                        Message.Send("Missing Argument: version");

                    return 1;
                }

                string name = args[1];
                int version = int.Parse(args[2]);

                if (Directory.Exists(name))
                {
                    Message.Send($"namespace '{name}' is already existed");
                    return 1;
                }

                string[] allfiles = Directory.GetFiles(".", "*.*", SearchOption.AllDirectories);
                var targets = allfiles.Where(v => v.EndsWith(".mcf")).ToList();


                DatapackData datapackData = new DatapackData()
                {
                    Name = name,
                    FilesPath = targets
                };

                //Create folders
                Directory.CreateDirectory(name);

                string jsonString = JsonSerializer.Serialize(datapackData);
                File.WriteAllText($"{name}/.mcfconfig", jsonString);
                
                
                Directory.CreateDirectory($"{name}/data");
                File.WriteAllText($"{name}/pack.mcmeta", @$"{{
    ""pack"": {{
        ""pack_format"": {version},
        ""description"": ""The default data for Minecraft""
    }}
}}");
                Directory.CreateDirectory($"{name}/data/{name}/advancements");
                Directory.CreateDirectory($"{name}/data/{name}/functions");
                Directory.CreateDirectory($"{name}/data/{name}/predicates");
                Directory.CreateDirectory($"{name}/data/{name}/loot_tables");
            }
            else
                Message.Send("Missing Argument: command");


            return 0;
        }
    }

    struct DatapackData
    {
        public string Name { get; set; }
        public List<string> FilesPath { get; set; }
    }
}
