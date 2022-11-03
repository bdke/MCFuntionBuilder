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
        public List<ClassMethodInfos> Methods { get; } = new();

        public BuiltInClass(System.Type type)
        {
            Name = type.Name;
            var methods = type.GetMethods(BindingFlags.Public);
            if (methods.Any())
            {
                foreach (var method in methods)
                {

                }
            }
        }

        public object? InvokeMethod(string name ,object?[] args)
        {
            return Methods.FirstOrDefault(m => m.Name == name).Func.Invoke(this,args);
        }

        public abstract object? GetValue(string name);
        public abstract void SetValue(string name, object? value);

        public struct ClassMethodInfos
        {
            public string Name { get; set; }
            public MethodInfo Func { get; set; }
        }
    }
}
