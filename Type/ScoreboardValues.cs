using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCFBuilder.Type
{
    public class ScoreboardValues
    {
        public const string RootPath = "ROOT";
        public readonly static string[] ScoreboardTypesString = { 
            "dummy" 
        };
        public ScoreboardTypes ScoreboardType { get; set; }
        public int? Value { get; set; }
        public string Name { get; set; }
        public string? Modifier { get; set; }
        public ScoreboardValues(ScoreboardTypes scoreboardType, int? value, string name, string? modifier)
        {
            ScoreboardType = scoreboardType;
            Value = value;
            Name = name;
            Modifier = modifier;
        }

        public static ScoreboardTypes GetScoreboardTypes(string type)
        {
            switch (type)
            {
                case "dummy":
                    return ScoreboardTypes.Dummy;
                default:
                    throw new NotImplementedException();
            }
        }

        public static string GetScoreboardTypes(ScoreboardTypes type)
        {
            switch (type)
            {
                case ScoreboardTypes.Dummy:
                    return "dummy";
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
