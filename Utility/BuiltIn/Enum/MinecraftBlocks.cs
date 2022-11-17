using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCFBuilder.Utility.BuiltIn.Enum
{
    internal class MinecraftBlocks : Class.BuiltInClass
    {
        public static List<BlocksData>? BlocksDatas;
        public MinecraftBlocks() : base(typeof(MinecraftBlocks))
        {
        }

        public static void Init()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var datas = JsonConvert.DeserializeObject<List<BlocksData>>(File.ReadAllText(@$"{baseDir}/MCData/Datas/Blocks.json"));
            BlocksDatas = datas.Select(v =>
            {
                v.DisplayName = v.DisplayName.Replace(" ", "_").ToUpper();
                return v;
            }).ToList();
        }

        public override object? GetValue(string name)
        {
            return BlocksDatas.Find(v => v.DisplayName == name).Name;
        }

        public struct BlocksData
        {
            public int ID;
            public string Name;
            public string DisplayName;
        }
    }
}
