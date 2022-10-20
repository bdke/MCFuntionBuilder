using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using MCFBuilder.Utility;

namespace MCFBuilder
{
    public class SyntaxErrorListener : BaseErrorListener
    {
        public override void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg,
            RecognitionException e)
        {
            //Console.WriteLine(msg);
            //Console.WriteLine($"at {recognizer}{offendingSymbol} ({line}:{charPositionInLine})");
            ErrorMessage.Send($"SyntaxError: {msg}\nat {Execute.CurrentFile} in {line}:{charPositionInLine}");
            Logging.Fatal(ErrorType.CompileException, $"at {Execute.CurrentFile} in {line}:{charPositionInLine}");
        }
    }
}
