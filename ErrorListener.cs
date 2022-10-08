using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;

namespace MCFBuilder
{
    public class SyntaxErrorListener : BaseErrorListener
    {
        public override void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg,
            RecognitionException e)
        {
            Console.WriteLine(msg);
            Console.WriteLine($"at {recognizer}{offendingSymbol} ({line}:{charPositionInLine})");
            //throw new Exception();
        }
    }
}
