using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MCFBuilder.Utility.BuiltIn
{
    public abstract class BuiltInClass
    {
        public string Name { get; }
        public List<MethodInfo> Methods { get; } = new();

        public BuiltInClass(string name)
        {
            Name = name;
        }

        public object? InvokeMethod(string name ,object?[] args)
        {
            return Methods.FirstOrDefault(m => m.Name == name).Func.Invoke(args);
        }

        public abstract object? GetValue(string name);
        public abstract void SetValue(string name, object? value);

        public struct MethodInfo
        {
            public string Name { get; set; }
            public Func<object?[]?, object?> Func { get; set; }
        }
    }
}
