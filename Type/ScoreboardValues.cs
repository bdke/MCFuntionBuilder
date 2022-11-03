using MCFBuilder.Type.Info;
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
        public Dictionary<string, int?> Value { get; set; }
        public string Name { get; set; }
        public string? File { get; set; }
        public bool IsModify { get; set; }
        public ScoreboardValues(ScoreboardTypes scoreboardType, Dictionary<string, int?> value, string name, string? file, bool isModify)
        {
            ScoreboardType = scoreboardType;
            Value = value;
            Name = name;
            File = file;
            IsModify = isModify;
        }

        public static ScoreboardTypes GetScoreboardTypes(string type)
        {
            switch (type)
            {
                case "dummy":
                    return ScoreboardTypes.Dummy;
                case "health":
                    return ScoreboardTypes.Health;
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
                case ScoreboardTypes.Health:
                    return "health";
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
