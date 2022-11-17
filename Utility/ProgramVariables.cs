using MCFBuilder.Type;
using MCFBuilder.Type.Compiler;
using MCFBuilder.Visitor.BuiltInFuntions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Selector = MCFBuilder.Utility.BuiltIn.Class.Selector;
using MCFBuilder.Utility.BuiltIn.Class;
using MCFBuilder.Utility.BuiltIn.Enum;

namespace MCFBuilder.Utility
{
    public static class ProgramVariables
    {
        public static string[] BuiltInFunctions { get; } =
            (from i in typeof(ProgramFunction).GetMethods(BindingFlags.Public | BindingFlags.Static) select i.Name)
            .ToArray();

        public static Dictionary<string, System.Type> BuiltInClasses { get; } = new()
        {
            [nameof(Selector)] = typeof(Selector),
            [nameof(Function)] = typeof(Function),
            [nameof(EffectTypes)] = typeof(EffectTypes),
            [nameof(StatisticsTypes)] = typeof(StatisticsTypes),
            [nameof(StatisticsCustomTypes)] = typeof(StatisticsCustomTypes),
            [nameof(Utility.BuiltIn.Class.Scoreboard)] = typeof(Utility.BuiltIn.Class.Scoreboard),
            [nameof(ScoreboardTypes)] = typeof(ScoreboardTypes),
            ["Items"] = typeof(MinecraftItems),
            ["Blocks"] = typeof(MinecraftBlocks),
            ["Entities"] = typeof(MinecraftEntities)
        };

        public static Dictionary<string, object?> GlobalVariables { get; } = VariablesInit();
        public static List<Scoreboard> ScoreboardObjects { get; } = new();
        public static List<TagsType> Tags { get; } = new();

        public static Dictionary<string, object?> VariablesInit()
        {
            var variables = new Dictionary<string, object?>();

            foreach (string function in BuiltInFunctions)
            {
                variables[function] = (object?[] s) => typeof(ProgramFunction).GetMethod(function).Invoke(typeof(ProgramFunction), new object[] { s });
            }



            return variables;
        }
    }
}
