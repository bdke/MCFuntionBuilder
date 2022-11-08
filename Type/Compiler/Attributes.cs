using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Dfa;
using MCFBuilder.Utility.BuiltIn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
            //TODO: debug this :(
            foreach (var attr in Attributes)
            {
                if (attr.AttributeType == AttributeType.IF)
                {
                    //ic++;
                    //for (int i = 0; i < ic - 1; i++)
                    //{
                    //    s += "unless " + IfItem[i] + " ";
                    //}
                    //if (ic == IfCount && IsContainElse)
                    //    s += "unless " + IfItem.Last() + " ";
                    //else if (ic == IfCount && !IsContainElse)
                        s += "if " + attr.Value + " ";
                }
                else if (attr.AttributeType == AttributeType.UNLESS)
                {
                    //ic++;
                    //for (int i = 0; i < ic - 1; i++)
                    //{
                    //    s += "unless " + IfItem[i] + " ";
                    //}
                    //if (ic == IfCount && IsContainElse)
                    //    s += "unless " + IfItem.Last() + " ";
                    //else if (ic == IfCount && !IsContainElse)
                        s += "unless " + attr.Value + " ";
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="selector"></param>
        /// <param name="value"></param>
        /// <param name="compareOp"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static int Add(string name, string selector, string compareOp, int num)
        {
            switch (compareOp)
            {
                case "==":
                    CommandAttribute.Attributes.Add(new($"score {selector} {name} matches {num}", AttributeType.IF));
                    return CommandAttribute.Attributes.Count - 1;
                case "!=":
                    CommandAttribute.Attributes.Add(new($"score {selector} {name} matches {num}", AttributeType.UNLESS));
                    return CommandAttribute.Attributes.Count - 1;
                case ">=":
                    CommandAttribute.Attributes.Add(new($"score {selector} {name} matches {num}..", AttributeType.IF));
                    return CommandAttribute.Attributes.Count - 1;
                case ">":
                    CommandAttribute.Attributes.Add(new($"score {selector} {name} matches {num+1}..", AttributeType.IF));
                    return CommandAttribute.Attributes.Count - 1;
                case "<=":
                    CommandAttribute.Attributes.Add(new($"score {selector} {name} matches ..{num}", AttributeType.IF));
                    return CommandAttribute.Attributes.Count - 1;
                case "<":
                    CommandAttribute.Attributes.Add(new($"score {selector} {name} matches ..{num-1}", AttributeType.IF));
                    return CommandAttribute.Attributes.Count - 1;
                default:
                    throw new ArgumentException();
            }
        }

        public static int Add(string left, string selectorLeft, string right, string selectorRight, string compareOp)
        {
            switch (compareOp)
            {
                case "==":
                    CommandAttribute.Attributes.Add(new($"score {selectorLeft} {left} = {selectorRight} {right}", AttributeType.IF));
                    return CommandAttribute.Attributes.Count - 1;
                case "!=":
                    CommandAttribute.Attributes.Add(new($"score {selectorLeft} {left} = {selectorRight} {right}", AttributeType.UNLESS));
                    return CommandAttribute.Attributes.Count - 1;
                case ">=":
                    CommandAttribute.Attributes.Add(new($"score {selectorLeft} {left} >= {selectorRight} {right}", AttributeType.IF));
                    return CommandAttribute.Attributes.Count - 1;
                case ">":
                    CommandAttribute.Attributes.Add(new($"score {selectorLeft} {left} > {selectorRight} {right}", AttributeType.IF));
                    return CommandAttribute.Attributes.Count - 1;
                case "<=":
                    CommandAttribute.Attributes.Add(new($"score {selectorLeft} {left} <= {selectorRight} {right}", AttributeType.IF));
                    return CommandAttribute.Attributes.Count - 1;
                case "<":
                    CommandAttribute.Attributes.Add(new($"score {selectorLeft} {left} < {selectorRight} {right}", AttributeType.IF));
                    return CommandAttribute.Attributes.Count - 1;
                default:
                    throw new ArgumentException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static int Add(object? condition, string type)
        {
            switch (type)
            {
                case "entity":
                    CommandAttribute.Attributes.Add(new($"entity {(string?)condition}", AttributeType.IF));
                    return CommandAttribute.Attributes.Count - 1;
                case "predicate":
                    CommandAttribute.Attributes.Add(new($"predicate {(string?)condition}", AttributeType.IF));
                    return CommandAttribute.Attributes.Count - 1;
                case "block":
                    CommandAttribute.Attributes.Add(new($"block {(string?)condition}", AttributeType.IF));
                    return CommandAttribute.Attributes.Count - 1;
                case "blocks":
                    CommandAttribute.Attributes.Add(new($"blocks {(string?)condition}", AttributeType.IF));
                    return CommandAttribute.Attributes.Count - 1;
                case "data block":
                    CommandAttribute.Attributes.Add(new($"data block {(string?)condition}", AttributeType.IF));
                    return CommandAttribute.Attributes.Count - 1;
                case "data entity":
                    CommandAttribute.Attributes.Add(new($"data entity {(string?)condition}", AttributeType.IF));
                    return CommandAttribute.Attributes.Count - 1;
                case "data storage":
                    CommandAttribute.Attributes.Add(new($"data storage {(string?)condition}", AttributeType.IF));
                    return CommandAttribute.Attributes.Count - 1;
                default:
                    throw new ArgumentException();
            }
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

    public class AttributeValue
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
        UNLESS,
        EXECUTE
    }
}
