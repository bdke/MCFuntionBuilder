using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCFBuilder.Utility
{
    public static class ProgramVariables
    {
        public static Dictionary<string, object?> globalVariables { get; } = new();
    }
}
