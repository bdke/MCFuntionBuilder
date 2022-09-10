using MCFBuilder.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCFBuilder.Utility
{
    public static class ProgramVariables
    {
        public static Dictionary<string, object?> globalVariables { get; } = new();
        public static List<Scoreboard> ScoreboardObjects { get; } = new();
        public static Dictionary<Scoreboard, string?> ScoreboardInitValues { get; } = new();
        public static List<TagsType> Tags { get; } = new();
    }
}
