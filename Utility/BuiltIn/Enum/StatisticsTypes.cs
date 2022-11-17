using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCFBuilder.Utility.BuiltIn.Enum
{
    internal class StatisticsTypes : Class.BuiltInClass
    {
        public const string CUSTOM = "custom";
        public const string BLOCK_MINED = "mined";
        public const string ITEM_BROKEN = "broken";
        public const string ITEM_CRAFTED = "crafted";
        public const string USED = "used";
        public const string ITEM_PICKED_UP = "picked_up";
        public const string ITEM_DROPPED = "dropped";
        public const string ENTITY_KILLED = "killed";
        public const string ENTITY_KILLED_BY = "killed_by";
        public StatisticsTypes() : base(typeof(StatisticsTypes))
        {

        }

        public override object? GetValue(string name)
        {
            switch (name)
            {
                case nameof(CUSTOM): return CUSTOM;
                case nameof(BLOCK_MINED): return BLOCK_MINED;
                case nameof(ITEM_BROKEN): return ITEM_BROKEN;
                case nameof(ITEM_CRAFTED): return ITEM_CRAFTED;
                case nameof(USED): return USED;
                case nameof(ITEM_PICKED_UP): return ITEM_PICKED_UP;
                case nameof(ENTITY_KILLED): return ENTITY_KILLED;
                case nameof(ENTITY_KILLED_BY): return ENTITY_KILLED_BY;
                default: return null;
            }
        }
    }

    internal class StatisticsCustomTypes : Class.BuiltInClass
    {
        public static List<StatsData>? StatsDatas;
        public StatisticsCustomTypes() : base(typeof(StatisticsCustomTypes))
        {

        }

        public static void Init()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            StatsDatas = JsonConvert.DeserializeObject<List<StatsData>>(File.ReadAllText(@$"{baseDir}/MCData/Datas/Statistics.json"));
        }

        public override object? GetValue(string name)
        {
            return StatsDatas.Find(v => v.Name == name).Location;
        }

        public struct StatsData
        {
            public string Name { get; set; }
            public string Location { get; set; }
        }
    }
}
