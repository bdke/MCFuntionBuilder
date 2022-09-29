using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCFBuilder.Type
{
    public class TagsType
    {
        public string? Name { get; set; }
        public Dictionary<string, bool>? Value { get; set; }
        public Selector? Selector { get; set; }
        
    }
}
