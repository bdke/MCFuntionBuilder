using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MCFBuilder.MCData
{
    public struct JsonEffectsData
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Type { get; set; }
    }

    public struct EffectsData
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public EffectType EffectType { get; set; }
    }

    public static class EffectsDatas
    {
        public static List<EffectsData> Effects = new();

        public static void Init()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            List<JsonEffectsData> effectsDatas = JsonConvert.DeserializeObject<List<JsonEffectsData>>(File.ReadAllText(@$"{baseDir}/MCData/Datas/Effects.json"));
            foreach (var effectsData in effectsDatas)
            {
                EffectsData data = new();
                data.EffectType = effectsData.Type switch
                {
                    "good" => EffectType.GOOD,
                    "bad" => EffectType.BAD,
                    _ => throw new NotImplementedException()
                };
                data.DisplayName = effectsData.DisplayName.ToUpper().Replace(' ', '_');
                var r = new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);
                data.Name = r.Replace(effectsData.Name, "_").ToLower();
                Effects.Add(data);
            }
        }
    }

    public enum EffectType
    {
        GOOD,BAD
    }
}
