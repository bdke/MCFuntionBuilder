using MCFBuilder.Type;
using System;
using System.Linq;

namespace MCFBuilder.Utility.BuiltIn
{
    public class Selector : BuiltInClass
    {
        public string? _value = null;
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

                //Volume Dimensions
                var dx = (selectorArgs.VolumeDimensions.X != null) ? $"dx={selectorArgs.VolumeDimensions.X}" : "";
                var dy = (selectorArgs.VolumeDimensions.Y != null) ? $"dy={selectorArgs.VolumeDimensions.Y}" : "";
                var dz = (selectorArgs.VolumeDimensions.Z != null) ? $"dz={selectorArgs.VolumeDimensions.Z}" : "";

                var volumeDimensions = string.Join(',', new string[] { dx, dy, dz }.Where(v => v != string.Empty));

                //Output
                _value = $"{selector}[{string.Join(',', new string[] { coordinate, distance, volumeDimensions }.Where(v => v != string.Empty))}]";

                return _value;
            }
            set => _value = value; 
        }
        private SelectorArgs selectorArgs = new();

        public Selector(string? value) : base(typeof(Selector))
        {
            selectorArgs.Selector = value;

        }

        public Selector() : base(typeof(Selector))
        {

        }


        //Set things
        public object? SetSelector(object?[]? args)
        {
            if (args != null && args.Length == 1)
            {
                selectorArgs.Selector = (string)args[0];
            }

            return null;
        }

        
        public object? SetCoordinate(object?[]? args)
        {
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

        public object? SetDistance(object?[]? args)
        {
            if (args != null && args.Length == 2)
            {
                selectorArgs.Distance = new(
                    float.Parse(args[0].ToString()),
                    float.Parse(args[1].ToString())
                    );
            }
            return null;
        }

        public object? SetVolumeDistance(object?[]? args)
        {
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




        //default things
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

        public override string? ToString()
        {
            return Value;
        }
    }
}
