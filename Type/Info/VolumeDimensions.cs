using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCFBuilder.Type.Info
{
    internal struct VolumeDimensions
    {
        public double? X { get; set; }
        public double? Y { get; set; }
        public double? Z { get; set; }
        public VolumeDimensions(double? x = null, double? y = null, double? z = null)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
