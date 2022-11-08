using MCFBuilder.MCData;
using MCFBuilder.Type.Compiler;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCFBuilder.Utility.BuiltIn.Class
{
    public class Effect : BuiltInClass
    {
        public Effect() : base(typeof(Effect))
        {

        }

        public static object? Give(object?[]? args)
        {
            var effects = from i in EffectsDatas.Effects select i.Name;
            if (args != null && effects.ToList().Contains(args[1]))
            {
                if (args.Length == 2)
                {
                    FunctionCompiler.Lines.Lines.Add($"{CommandAttribute.Compile()}effect give {args[0]} {args[1]}");
                }
                else if (args.Length == 3)
                {
                    FunctionCompiler.Lines.Lines.Add($"{CommandAttribute.Compile()}effect give {args[0]} {args[1]} {args[2]}");
                }
                else if (args.Length == 4)
                {
                    FunctionCompiler.Lines.Lines.Add($"{CommandAttribute.Compile()}effect give {args[0]} {args[1]} {args[2]} {args[2]}");
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            else
            {
                throw new ArgumentException();
            }

            return null;
        }

        public static object? Clear(object?[]? args)
        {
            var effects = from i in EffectsDatas.Effects select i.Name;
            if (args != null)
            {
                if (args.Length == 1)
                {
                    FunctionCompiler.Lines.Lines.Add($"{CommandAttribute.Compile()}effect clear {args[0]}");
                }
                else if (args.Length == 2 && effects.Contains(args[1]))
                {
                    FunctionCompiler.Lines.Lines.Add($"{CommandAttribute.Compile()}effect clear {args[0]} {args[1]}");
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            else
            {
                FunctionCompiler.Lines.Lines.Add($"{CommandAttribute.Compile()}effect clear");
            }

            return null;
        }
    }
}
