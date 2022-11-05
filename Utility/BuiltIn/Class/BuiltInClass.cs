using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MCFBuilder.Utility.BuiltIn.Class
{
    public abstract class BuiltInClass
    {
        public string Name { get; }
        public List<ClassMethodInfos> Methods { get; } = new();

        public BuiltInClass(System.Type type)
        {
            Name = type.Name;
            var methods = type.GetMethods();
            if (methods.Any())
            {
                foreach (var method in methods)
                {
                    Methods.Add(new() { Name = method.Name, Func = method });
                }
            }
        }

        public object? InvokeMethod(string name, object?[] args)
        {
            return Methods.FirstOrDefault(m => m.Name == name).Func.Invoke(this, args);
        }

        public object? GetValue(string name)
        {
            var props = GetType().GetProperty(name);

            if (props != null)
            {
                return props.GetValue(this, null);
            }
            else
            {
                throw new ArgumentException();
            }
        }
        public void SetValue(string name, object? value)
        {
            var props = GetType().GetProperty(name);

            if (props != null)
            {
                props.SetValue(this, value);
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public struct ClassMethodInfos
        {
            public string Name { get; set; }
            public MethodInfo? Func { get; set; }
        }
    }
}
