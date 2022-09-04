using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCFBuilder.Type;

namespace MCFBuilder.Utility
{
    public static class FunctionCompiler
    {
        public static List<string> Lines = new();

        public static Scoreboard Scoreboard(ScoreboardValues v)
        {
            return new Scoreboard(v);
        }
    }

    public class Scoreboard
    {
        public Dictionary<ScoreboardTypes, string> ScoreboardName = new();

        private ScoreboardValues scoreboardValues;

        //TODO: get the operator
        public Scoreboard(ScoreboardValues scoreboardValues)
        {
            this.scoreboardValues = scoreboardValues;
        }

        public void Create()
        {

        }
    }
}
