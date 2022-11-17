using MCFBuilder.Type.Compiler;
using MCFBuilder.Utility.BuiltIn.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCFBuilder.Utility.BuiltIn.Class
{
    internal class Scoreboard : BuiltInClass
    {
        public Scoreboard() : base(typeof(Scoreboard)) {

        }

        public static object? Create(object?[]? args)
        {
            var str = CreatePrivate(args);
            FunctionCompiler.Lines.Lines.Add($"{CommandAttribute.Compile()}scoreboard objectives add {str}");
            return str;
        }


        private static string? CreatePrivate(object?[]? args)
        {
            if (args != null && args.Length == 2)
            {
                switch (args[0])
                {
                    case StatisticsTypes.CUSTOM:
                        return StatisticsCustomTypes.StatsDatas.Any(v => v.Location == (string?)args[1])
                            ? $"{args[0]}:{args[1]}"
                            : throw new ArgumentException();
                    case StatisticsTypes.BLOCK_MINED:
                        return MinecraftBlocks.BlocksDatas.Any(v => v.Name == (string?)args[1]) ? $"{args[0]}:{args[1]}" : throw new ArgumentException();
                    case StatisticsTypes.ITEM_BROKEN:
                        return MinecraftItems.ItemsDatas.Where(v => v.MaxDurability != null).Any(v => v.Name == (string?)args[1])
                            ? $"{args[0]}:{args[1]}"
                            : throw new ArgumentException();
                    case StatisticsTypes.ITEM_CRAFTED:
                        return MinecraftItems.ItemsDatas.Any(v => v.Name == (string?)args[1]) ? $"{args[0]}:{args[1]}" : throw new ArgumentException();
                    case StatisticsTypes.USED:
                        return MinecraftItems.ItemsDatas.Any(v => v.Name == (string?)args[1]) ? $"{args[0]}:{args[1]}" : throw new ArgumentException();
                    case StatisticsTypes.ITEM_PICKED_UP:
                        return MinecraftItems.ItemsDatas.Any(v => v.Name == (string?)args[1]) ? $"{args[0]}:{args[1]}" : throw new ArgumentException();
                    case StatisticsTypes.ITEM_DROPPED:
                        return MinecraftItems.ItemsDatas.Any(v => v.Name == (string?)args[1]) ? $"{args[0]}:{args[1]}" : throw new ArgumentException();
                    case StatisticsTypes.ENTITY_KILLED:
                        return MinecraftEntities.EntitiesDatas.Any(v => v.Name == (string?)args[1]) ? $"{args[0]}:{args[1]}" : throw new ArgumentException();
                    case StatisticsTypes.ENTITY_KILLED_BY:
                        return MinecraftEntities.EntitiesDatas.Any(v => v.Name == (string?)args[1]) ? $"{args[0]}:{args[1]}" : throw new ArgumentException();
                    default:
                        throw new NotImplementedException();
                }
            }
            else if (args != null && args.Length == 1)
            {
                return (string?)args[0];
            }
            else
                throw new ArgumentException();
        }
    }
}
