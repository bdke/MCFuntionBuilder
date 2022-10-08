using Antlr4.Runtime.Misc;
using MCFBuilder.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCFBuilder.Utility.BuiltIn
{
    public class Selector : BuiltInClass
    {
        private string? _value = null;
        public string? Value
        {
            get
            {
                //Coordinate
                var coorX = (selectorArgs.Coordinates.X != null) ? $"x={selectorArgs.Coordinates.X}" : "";
                var coorY = (selectorArgs.Coordinates.Y != null) ? $"y={selectorArgs.Coordinates.Y}" : "";
                var coorZ = (selectorArgs.Coordinates.Z != null) ? $"z={selectorArgs.Coordinates.Z}" : "";

                var coordinate = string.Join(',',new string[] { coorX, coorY, coorZ }.Where(v => v != string.Empty));

                //Distance
                var minDis = (selectorArgs.Distance.Min != null) ? $"{selectorArgs.Distance.Min}" : "";
                var maxDis = (selectorArgs.Distance.Max != null) ? $"{selectorArgs.Distance.Max}" : "";

                var distance = string.Join("..", new string[] { minDis, maxDis }.Where(v => v != string.Empty));

                //Output
                _value = $"[{string.Join(',', new string[] { coordinate, distance }.Where(v => v != string.Empty))}]";

                return _value;
            }
            set => _value = value; 
        }
        private SelectorArgs selectorArgs = new();

        public Selector(string? value) : base(nameof(Selector))
        {
            Value = value;
            Methods.Add(new MethodInfo() { Name = nameof(SetCoordinate), Func = SetCoordinate});
            Methods.Add(new MethodInfo() { Name = nameof(SetDistance), Func = SetDistance });
        }

        public Selector() : base(nameof(Selector))
        {
            Methods.Add(new MethodInfo() { Name = nameof(SetCoordinate), Func = SetCoordinate });
            Methods.Add(new MethodInfo() { Name = nameof(SetDistance), Func = SetDistance });
        }

        
        private object? SetCoordinate(object?[]? args)
        {
            //args.Coordinates = new(x, y, z);
            if (args != null && args.Length == 3)
            {
                selectorArgs.Coordinates = new((double?)args[0], (double?)args[1], (double?)args[2]);
            }
            return null;
        }

        private object? SetDistance(object?[]? args)
        {
            //args.Distance = new(min, max);
            if (args != null && args.Length == 2)
            {
                selectorArgs.Distance = new((float?)args[0], (float?)args[1]);
            }
            return null;
        }

        public override object? GetValue(string name)
        {
            return name switch
            {
                nameof(Value) => Value,
                _ => throw new ArgumentException()
            };
        }

        public override void SetValue(string name, object? value)
        {
            switch (name)
            {
                case nameof(Value):
                    if (value is string s)
                    {
                        Value = s;
                    }
                    break;
                default:
                    throw new ArgumentException();
            }
        }
    }
}
