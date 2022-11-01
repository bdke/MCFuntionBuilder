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
using Antlr4.Runtime.Misc;

namespace MCFBuilder
{
    public static class Execute
    {
        public static string? Namespace { get; set; }
        public static string? CurrentFile { get; set; }
        private static ArgsOptions Options = new() { };
        private static List<string> Commands { get; set; } = new();


        [Time($"Compiled code...")]
        public static async Task Run(string filePath)
        {
            var fileContents = await File.ReadAllTextAsync(filePath);
            AntlrInputStream inputStream = new AntlrInputStream(fileContents);
            var lexer = new MCFBuilderLexer(inputStream);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            MCFBuilderParser parser = new MCFBuilderParser(tokens);
            parser.AddErrorListener(new SyntaxErrorListener());
            var context = parser.program();
            FunctionCompiler.Lines = new();
            ScriptVisitor visitor = new ScriptVisitor();
            visitor.Visit(context);
            visitor.FunctionEndAction();
        }

        [Time("Loaded global variables...")]
        private static async Task LoadGlobal(string filePath)
        {
            var fileContents = await File.ReadAllTextAsync(filePath);
            AntlrInputStream inputStream = new AntlrInputStream(fileContents);
            var lexer = new MCFBuilderLexer(inputStream);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            MCFBuilderParser parser = new MCFBuilderParser(tokens);
            ScriptVisitor visitor = new ScriptVisitor();
            visitor.Init = true;
            await visitor.GlobalAssignment(parser.program());
            visitor.Init = false;
        }

        private static void ErrorListener(object? sender, FirstChanceExceptionEventArgs args)
        {
            Logging.Error(args.Exception);
        }

        [Time("Application finished...")]
        public static async Task<int> Main(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith("--") || args[i].StartsWith("-"))
                {
                    switch (args[i][2..^0])
                    {
                        case "loglevel":
                            try
                            {
                                Options.LogLevel = int.Parse(args[i + 1]);
                                Logging.LogLevel = int.Parse(args[i + 1]);
                            }
                            catch (Exception e)
                            {
                                ErrorMessage.Send($"Invalid value of loglevel: '{args[1]}'");
                                Logging.Fatal(e);
                            }
                            break;
                        case "version":
                            try
                            {
                                Options.Version = int.Parse(args[i + 1]);
                            }
                            catch (Exception e)
                            {
                                ErrorMessage.Send($"Invalid value of loglevel: '{args[1]}'");
                                Logging.Fatal(e);
                            }
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    i += 1;
                    continue;
                }
                else
                    Commands.Add(args[i]);
            }
            Logging.Init();

            AppDomain.CurrentDomain.FirstChanceException += ErrorListener;
            if (args.Length < 1)
            {
                ErrorMessage.Send("Input arguments must contain at least 1");
                return 1;
            }

            string command = args[0];

            if (command == "compile")
            { 
                return await Compile();
            }
            else if (command == "new")
            {
                return await New();
            }
            else
                ErrorMessage.Send("Missing Argument: command");

            return 0;
        }

        private async static Task<int> Compile()
        {

            string[] namespaces = Directory.GetFiles(".", "*.*", SearchOption.AllDirectories)
                .Where(v => v.EndsWith(".mcfconfig"))
                .ToArray();
            if (namespaces.Length == 0)
            {
                ErrorMessage.Send("Unable to find any working namespaces, please create a new namepsace with 'new' command");
                Logging.Error(ErrorType.RuntimeException, "No namepsace avaliable");
                return 1;
            }

            foreach (string ns in namespaces)
            {
                DatapackData datapackData = JsonConvert.DeserializeObject<DatapackData>(File.ReadAllText(ns));

                Namespace = datapackData.Name;

                Directory.Delete($"./{datapackData.Name}/data/{datapackData.Name}/functions", true);
                Directory.CreateDirectory($"./{datapackData.Name}/data/{datapackData.Name}/functions");


                string[] scripts = Directory.GetFiles($"./{datapackData.Name}/scripts", "*.mcf", SearchOption.AllDirectories)
                    .ToArray();

                foreach (string file in scripts)
                {
                    CurrentFile = file;
                    await LoadGlobal(file);
                }

                foreach (string file in scripts)
                {
                    CurrentFile = file;
                    await Run(file);
                }
            }
            return 0;
        }

        private async static Task<int> New()
        {
            string name = Commands[0].ToLower();
            int? version;
            if (Options.Version == null)
            {
                version = Options.Version;
            }
            else
            {
                version = 10;
            }

            if (Directory.Exists(name))
            {
                ErrorMessage.Send($"namespace '{name}' is already existed");
                return 1;
            }

            string[] allfiles = Directory.GetFiles(".", "*.*", SearchOption.AllDirectories);


            DatapackData datapackData = new DatapackData()
            {
                Name = name,
                Version = version
            };

            //Create folders
            Directory.CreateDirectory(name);

            string jsonString = JsonConvert.SerializeObject(datapackData);
            File.WriteAllText($"{name}/.mcfconfig", jsonString);


            Directory.CreateDirectory($"{name}/data");
            await File.WriteAllTextAsync($"{name}/pack.mcmeta", @$"{{
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
            return 0;
        }
    }

    struct DatapackData
    {
        public string Name { get; set; }
        public int? Version { get; set; }
        //public List<string> FilesPath { get; set; }
    }

    struct ArgsOptions
    {
        public int? LogLevel { get; set; }
        public int? Version { get; set; }
    }
}
