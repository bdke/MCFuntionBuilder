using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Dfa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCFBuilder.Type.Compiler
{
    public static class CommandAttribute
    {
        public static List<AttributeValue> Attributes { get; } = new();

        public static bool IsContainElse { get; set; } = true;

        public static string Compile()
        {
            var s = "";

            //check if contain any attribute
            if (!Attributes.Any())
            {
                return s;
            }

            s += "execute ";

            var IfItem = (from i in Attributes.Where(v => v.AttributeType == AttributeType.IF) select i.Value).ToList();

            var IfCount = IfItem.Count();
            var ic = 0;

            foreach (var attr in Attributes)
            {
                if (attr.AttributeType == AttributeType.IF)
                {
                    ic++;
                    for (int i = 0; i < ic - 1; i++)
                    {
                        s += "unless " + IfItem[i] + " ";
                    }
                    if (ic == IfCount && IsContainElse)
                        s += "unless " + IfItem.Last() + " ";
                    else if (ic == IfCount && !IsContainElse)
                        s += "if " + IfItem.Last() + " ";
                }
                else if (attr.AttributeType == AttributeType.EXECUTE)
                {
                    s += attr.Value + " ";
                }
            }

            s += "run ";
            return s;
        }
    }

    public static class IfConditionHandler
    {
        public static int Add(string condition, string type)
        {
            switch (type)
            {
                case "entity":
                    CommandAttribute.Attributes.Add(new($"entity {condition}", AttributeType.IF));
                    return CommandAttribute.Attributes.Count - 1;
                default:
                    throw new ArgumentException();
            }
        }

        public static void Add(MCFBuilderParser.ExpressionContext condition, string type)
        {

        }

        public static void Remove(int? num)
        {
            CommandAttribute.Attributes.RemoveAt((int)num);
        }
    }

    public static class ExecutesHandler
    {
        public static int Add(string s)
        {
            CommandAttribute.Attributes.Add(new(s, AttributeType.EXECUTE));
            return CommandAttribute.Attributes.Count - 1;
        }

        public static void Remove(int num)
        {
            CommandAttribute.Attributes.RemoveAt(num);
        }
    }

    public struct AttributeValue
    {
        public string Value { get; set; }
        public AttributeType AttributeType { get; set; }
        public AttributeValue(string value, AttributeType attributeType)
        {
            Value = value;
            AttributeType = attributeType;
        }
    }

    public enum AttributeType
    {
        IF,
        EXECUTE
    }
}
