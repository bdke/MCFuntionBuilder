using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCFBuilder.Type.Compiler;

namespace MCFBuilder.Utility.BuiltIn
{
    public class Function : BuiltInClass
    {
        public Function() : base(nameof(Function))
        {

        }

        public static object? GetCurrentFile(object?[]? args)
        {
            return ScriptVisitor.CurrentFile;
        }

        public static object? GetCurrentNamespace(object?[]? args)
        {
            return Execute.Namespace;
        }

        public static object? Call(object?[]? args)
        {
            if (args != null && args.Length == 2)
            {
                FunctionCompiler.Lines.Lines.Add($"{CommandAttribute.Compile()}function {((string?)args[0]).ToLower()}:{(string?)args[1]}");
            }

            return null;
        }


        //TODO return a .mcfunction file object
        public static object? Create(object?[]? args)
        {
            if (args != null && args.Length == 1)
            {
                File.Create($"{Execute.Namespace}/data/{Execute.Namespace}/functions/" + (string?)args[0] + ".mcfunction");
            }
            return null;
        }

        public override object? GetValue(string name)
        {
            return name switch
            {
                _ => throw new NotImplementedException()
            };
        }

        public override void SetValue(string name, object? value)
        {
            throw new NotImplementedException();
        }
    }
}
