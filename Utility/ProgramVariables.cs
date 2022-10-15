﻿using MCFBuilder.Type;
using MCFBuilder.Type.Compiler;
using MCFBuilder.Visitor.BuiltInFuntions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MCFBuilder.Utility.BuiltIn;
using Selector = MCFBuilder.Utility.BuiltIn.Selector;

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
        };

        public static Dictionary<string, object?> GlobalVariables { get; } = VariablesInit();
        public static List<Scoreboard> ScoreboardObjects { get; } = new();
        public static List<TagsType> Tags { get; } = new();

        private static Dictionary<string, object?> VariablesInit()
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
