using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCFBuilder.Utility.BuiltIn.Enum
{
    internal class ScoreboardTypes : Class.BuiltInClass
    {
        public const string DUMMY = "dummy";
        public const string TRIGGER = "trigger";
        public const string DEATH_COUNT = "deathCount";
        public const string PLAYER_KILL_COUNT = "playerKillCount";
        public const string TOTAL_KILL_COUNT = "totalKillCount";
        public const string HEALTH = "health";
        public const string XP = "xp";
        public const string LEVEL = "level";
        public const string FOOD = "food";
        public const string AIR = "air";
        public const string ARMOR = "armor";
        public ScoreboardTypes() : base(typeof(ScoreboardTypes))
        {
            
        }

        public static object? GetTypes(object?[]? args)
        {
            return new string[]
            {
                nameof(DUMMY),
                nameof(TRIGGER),
                nameof(DEATH_COUNT),
                nameof(PLAYER_KILL_COUNT),
                nameof(TOTAL_KILL_COUNT),
                nameof(HEALTH),
                nameof(XP),
                nameof(LEVEL),
                nameof(FOOD),
                nameof(AIR),
                nameof(ARMOR)
            };
        }

        public override object? GetValue(string name)
        {
            return GetType().GetField(name).GetValue(null);
        }
    }
}
