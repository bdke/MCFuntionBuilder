using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCFBuilder.Utility.BuiltIn.Enum
{
    internal class MinecraftEntities : Class.BuiltInClass
    {
        public static List<EntitiesData>? EntitiesDatas;
        public MinecraftEntities() : base(typeof(MinecraftEntities))
        {

        }

        public static void Init()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var datas = JsonConvert.DeserializeObject<List<EntitiesData>>(File.ReadAllText(@$"{baseDir}/MCData/Datas/Entities.json"));
            EntitiesDatas = datas.Select(v =>
            {
                v.DisplayName = v.DisplayName.Replace(" ", "_").ToUpper();
                return v;
            }).ToList();
        }

        public override object? GetValue(string name)
        {
            return EntitiesDatas.Find(v => v.DisplayName == name).Name;
        }

        public struct EntitiesData
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string DisplayName { get; set; }
        }
    }
}
