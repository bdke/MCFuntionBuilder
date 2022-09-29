using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCFBuilder.Type.Info;
using MCFBuilder.Type.Compiler;


namespace MCFBuilder.Type
{
    public class Selector
    {
        public string? Value { get; set; }
        private SelectorArgs args = new();

        public Selector(string? value)
        {
            Value = value;
        }

        public void SetCoordinate(double? x = null, double? y = null, double? z = null)
        {
            args.Coordinates = new(x, y, z);
        }

        public void SetDistance(float? min, float? max = null)
        {
            args.Distance = new(min, max);
        }
    }

    internal struct SelectorArgs
    {
        public Coordinates Coordinates { get; set; }
        public Distance Distance { get; set; }

        public SelectorArgs(Coordinates coordinates, Distance distance)
        {
            Coordinates = coordinates;
            Distance = distance;
        }
    }
}
