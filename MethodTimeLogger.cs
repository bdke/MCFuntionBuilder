using System;
using System.Reflection;
using MCFBuilder;

namespace Fody.MethodTimer
{
    public static class MethodTimeLogger
    {
        public static void Log(MethodBase methodBase, long milliseconds, string message)
        {
            Logging.Debug($"{methodBase.Name}: {message} {milliseconds}ms");
        }
    }
}