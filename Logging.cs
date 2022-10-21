using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace MCFBuilder
{
    internal static class Logging
    {
        public static int? LogLevel { get; set; } = null;
        public static void Init()
        {
            var logLevel = new LoggingLevelSwitch();
            if (LogLevel == null)
            {
                logLevel.MinimumLevel = ((LogEventLevel)1 + (int)LogEventLevel.Fatal);
            }
            else
            {
                logLevel.MinimumLevel = (LogEventLevel)LogLevel;
            }

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(logLevel)
                .WriteTo.Console()
                .WriteTo.File($"Logs/debug_.log", rollingInterval: RollingInterval.Hour, restrictedToMinimumLevel: LogEventLevel.Debug)
                .WriteTo.File($"Logs/trace_.log", rollingInterval: RollingInterval.Hour, restrictedToMinimumLevel: LogEventLevel.Verbose)
                .CreateLogger();

            Log.Information("Logger setup successfully");
        }

        public static void Info(string message)
        {
            Log.Information(message);
        }

        public static void Debug(string message)
        {
            Log.Debug(message);
        }

        public static void Trace(string message)
        {
            Log.Verbose(message);
        }

        public static void Error(ErrorType errorType, object message)
        {
            Log.Error($"{errorType}: {message}");
        }

        public static void Error(Exception e)
        {
            Log.Error(e, e.Message);
        }

        public static void Fatal(ErrorType errorType, object message)
        {
            Log.Fatal($"{errorType}: {message}");
            System.Environment.Exit(1);
        }

        public static void Fatal(Exception e)
        {
            Log.Fatal(e, e.Message);
            System.Environment.Exit(1);
        }


    }

    internal enum ErrorType
    {
        ArgumentException,
        CompileException,
        RuntimeException
    }
}
