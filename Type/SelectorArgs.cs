using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCFBuilder.Type.Info;
using MCFBuilder.Type.Compiler;


namespace MCFBuilder.Type
{
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
