using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCFBuilder.Utility.BuiltIn.Enum
{
    internal class MinecraftItems : Class.BuiltInClass
    {
        public static List<ItemsData>? ItemsDatas;

        public MinecraftItems() : base(typeof(MinecraftItems))
        {
        }

        public static void Init()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var datas = JsonConvert.DeserializeObject<List<ItemsData>>(File.ReadAllText(@$"{baseDir}/MCData/Datas/Items.json"));
            ItemsDatas = datas.Select(v =>
            {
                v.DisplayName = v.DisplayName.Replace(" ", "_").ToUpper();
                return v;
            }).ToList();
        }
        public override object? GetValue(string name)
        {
            return ItemsDatas.Find(v => v.DisplayName == name).Name;
        }

        public override void SetValue(string name, object? value)
        {
            throw new InvalidOperationException();
        }

        public struct ItemsData
        {
            public int ID { get; set; }
            public string DisplayName { get; set; }
            public string Name { get; set; }
            public int StackSize { get; set; }
            public int? MaxDurability { get; set; }
        }
    }
}
