using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCFBuilder.Utility
{
    public static class ErrorMessage
    {
        public static void Send(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void Send(ErrorMessageType errorType, string message, int line, int pos)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{errorType}: {message}\nin {line}:{pos}");
            Console.ResetColor();
        }
    }

    public enum ErrorMessageType
    {
        SyntaxError,
        NotImplementedError
    }
}
