using Microsoft.CodeAnalysis.CSharp.Scripting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCFBuilder.Visitor.BuiltInFuntions
{
    public static class ProgramFunction
    {
        public static object? Write(object?[] args)
        {
            foreach (var arg in args)
            {
                Console.WriteLine(arg);
            }

            return null;
        }

        public static async Task<object?> LoadFile(object?[] arg)
        {
            string? result = null;
            if (arg != null)
            {
                result = File.ReadAllText(arg[0].ToString());
                Console.WriteLine(await CSharpScript.EvaluateAsync(result));
            }
            return result;
        }
    }
}
