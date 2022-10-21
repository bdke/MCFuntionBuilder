using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using MCFBuilder.Type;
using MCFBuilder.Utility;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using MethodTimer;
using System.Diagnostics;
using System.Runtime.ExceptionServices;

namespace MCFBuilder
{
    public static class Execute
    {
        public static string? Namespace { get; set; }
        public static string? CurrentFile { get; set; }

        [Time($"Compiled code...")]
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

        [Time("Loaded global variables...")]
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

        private static void ErrorListener(object? sender, FirstChanceExceptionEventArgs args)
        {
            Logging.Error(args.Exception);
        }

        [Time("Application finished...")]
        public static async Task<int> Main(string[] args)
        {
            AppDomain.CurrentDomain.FirstChanceException += ErrorListener;
            if (args.Length < 1)
            {
                ErrorMessage.Send("Input arguments must contain at least 1");
                return 1;
            }

            string command = args[0];

            //Run("Test/test.mcf");
            if (command == "compile")
            {
                //string[] allfiles = Directory.GetFiles("./", "*.*", SearchOption.AllDirectories);
                //var targets = allfiles.Where(v => v.EndsWith(".mcf"));
                
                if (args.Length > 1)
                {
                    try
                    {
                        int logLevel = int.Parse(args[1]);
                        Logging.LogLevel = logLevel;
                    }
                    catch (Exception e)
                    {
                        ErrorMessage.Send($"Invalid value of loglevel: '{args[1]}'");
                        Logging.Fatal(e);
                        
                    }
                }
                Logging.Init();

                string[] namespaces = Directory.GetFiles(".", "*.*", SearchOption.AllDirectories)
                    .Where(v => v.EndsWith(".mcfconfig"))
                    .ToArray();
                if (namespaces.Length == 0)
                {
                    ErrorMessage.Send("Unable to find any working namespaces, please create a new namepsace with 'new' command");
                    Logging.Error(ErrorType.RuntimeException,"No namepsace avaliable");
                    return 1;
                }

                foreach (string ns in namespaces)
                {
                    DatapackData datapackData = JsonConvert.DeserializeObject<DatapackData>(File.ReadAllText(ns));

                    Namespace = datapackData.Name;

                    string[] scripts = Directory.GetFiles($"./{datapackData.Name}/scripts", "*.mcf", SearchOption.AllDirectories)
                        .ToArray();

                    foreach (string file in scripts)
                    {
                        CurrentFile = file;
                        LoadGlobal(file);
                    }

                    foreach (string file in scripts)
                    {
                        CurrentFile = file;
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
                Logging.Init();
                if (args.Length < 3)
                {
                    if (args.Length == 2)
                        ErrorMessage.Send("Missing Argument: version");

                    return 1;
                }

                string name = args[1].ToLower();
                int version = int.Parse(args[2]);

                if (Directory.Exists(name))
                {
                    ErrorMessage.Send($"namespace '{name}' is already existed");
                    return 1;
                }

                string[] allfiles = Directory.GetFiles(".", "*.*", SearchOption.AllDirectories);


                DatapackData datapackData = new DatapackData()
                {
                    Name = name,
                };

                //Create folders
                Directory.CreateDirectory(name);

                string jsonString = JsonConvert.SerializeObject(datapackData);
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
                Directory.CreateDirectory($"{name}/scripts");

                Console.WriteLine($"created new namespace: {name}");
            }
            else
                ErrorMessage.Send("Missing Argument: command");

            return 0;
        }
    }

    struct DatapackData
    {
        public string Name { get; set; }
        //public List<string> FilesPath { get; set; }
    }
}
