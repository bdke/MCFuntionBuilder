using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCFBuilder.Utility
{
    public static class Message
    {
        public static void Send(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void Send(ErrorMessageType errorType, string message, int line, int pos)
        {

        }
    }

    public enum ErrorMessageType
    {
        SyntaxError,
        NotImplementedError
    }
}
