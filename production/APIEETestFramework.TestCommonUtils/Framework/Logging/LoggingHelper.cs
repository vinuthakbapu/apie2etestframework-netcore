namespace TestCommonUtils
{
    using System;
    using System.Diagnostics;
    using NLog;

    public class LoggingHelper
    {
        private const int MaxFeatureLength = 30;

        private const int MaxMethodLength = 130;

        private const int MaxLengthLine = 200;


        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public static void MarkFeatureStart(string message)
        {
            Banner($"Start Feature - {message}", "*");
        }

        public static void MarkFeatureComplete(string message)
        {
            Banner($"End Feature - {message}", "*");
        }

        public static void MarkScenarioStart(string message)
        {
            Banner($"Begin Scenario - {message}", "<>");
        }

        public static void MarkScenarioComplete(string message)
        {
            Banner($"End Scenario - {message}", "<>");
        }

        public static void MarkStepStart(string message)
        {
            Banner(message, "-");
        }

        public static void Log(string message)
        {
            LogWithMethodFromStackIndex(LogLevel.Info, message);
        }

        public static void Error(string message)
        {
            LogWithMethodFromStackIndex(LogLevel.Error, message);
        }

        public static void LogExceptionAndThrow(string message)
        {
            LogWithMethodFromStackIndex(LogLevel.Error, message);
            throw new Exception(message);
        }

        private static void Banner(string message, string bannerPattern)
        {
            LogWithMethodFromStackIndex(LogLevel.Info, InsertLine(MaxLengthLine, bannerPattern), 3);
            LogWithMethodFromStackIndex(LogLevel.Info, message, 3);
            LogWithMethodFromStackIndex(LogLevel.Info, InsertLine(MaxLengthLine, bannerPattern), 3);
        }

        private static void LogWithMethodFromStackIndex(LogLevel level, string message, int index = 2)
        {
            var s = new StackTrace();
            var methodName = s.GetFrame(index).GetMethod().Name;
        }

        private static string InsertLine(int maxLength, string symbol)
        {
            var line = "";
            while (line.Length < maxLength)
            {
                line += symbol;
            }
      
                return line;
        }
    }
}