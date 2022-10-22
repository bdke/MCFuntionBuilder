using MCFBuilder.Type;
using System;
using System.Linq;

namespace MCFBuilder.Utility.BuiltIn
{
    public class Selector : BuiltInClass
    {
        private string? _value = null;
        public string? Value
        {
            get
            {
                //Selector
                var selector = (selectorArgs.Selector != null) ? selectorArgs.Selector : throw new NotImplementedException();

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
                _value = $"{selector}[{string.Join(',', new string[] { coordinate, distance }.Where(v => v != string.Empty))}]";

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
            Methods.Add(new MethodInfo() { Name = nameof(SetSelector), Func = SetSelector });
        }

        public Selector() : base(nameof(Selector))
        {
            Methods.Add(new MethodInfo() { Name = nameof(SetCoordinate), Func = SetCoordinate });
            Methods.Add(new MethodInfo() { Name = nameof(SetDistance), Func = SetDistance });
            Methods.Add(new MethodInfo() { Name = nameof(SetSelector), Func = SetSelector });
        }

        private object? SetSelector(object?[]? args)
        {
            if (args != null && args.Length == 1)
            {
                selectorArgs.Selector = (string)args[0];
            }

            return null;
        }

        
        private object? SetCoordinate(object?[]? args)
        {
            //args.Coordinates = new(x, y, z);
            if (args != null && args.Length == 3)
            {
                selectorArgs.Coordinates = new(
                    (args[0] != null) ? double.Parse(args[0].ToString()) : null,
                    (args[1] != null) ? double.Parse(args[1].ToString()) : null,
                    (args[2] != null) ? double.Parse(args[2].ToString()) : null
                    );
            }
            return null;
        }

        private object? SetDistance(object?[]? args)
        {
            //args.Distance = new(min, max);
            if (args != null && args.Length == 2)
            {
                selectorArgs.Distance = new(
                    float.Parse(args[0].ToString()),
                    float.Parse(args[1].ToString())
                    );
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
