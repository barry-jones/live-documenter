using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Exporter
{
    /// <summary>
    /// Class for logging output to the System.Console.
    /// </summary>
    /// <include file='Documentation\logger.xml' path='members/member[@name="Logger"]/*'/>
    internal static class Logger
    {
        private static readonly Dictionary<LogType, ConsoleColor> outputColours;
        private static readonly Dictionary<LogType, string> messagePrefix;
        private static bool verbose = false;

        /// <summary>
        /// Static constructor for initialising readonly static fields.
        /// </summary>
        static Logger()
        {
            Logger.outputColours = new Dictionary<LogType, ConsoleColor> {
                { LogType.Information, ConsoleColor.White },
                { LogType.Warning, ConsoleColor.Yellow },
                { LogType.Error, ConsoleColor.Red },
                { LogType.Progress, ConsoleColor.Green }
                };
            Logger.messagePrefix = new Dictionary<LogType, string> {
                { LogType.Information,  "" },
                { LogType.Progress,  "" },
                { LogType.Warning,      "[wrn] " },
                { LogType.Error,        "[err] " }
                };
        }

        /// <summary>
        /// Initialises the logger with details that control how this application instance will run.
        /// </summary>
        /// <param name="verbose">Indicates if verbose messages should be logged.</param>
        public static void Init(bool verbose)
        {
            Logger.verbose = verbose;
        }

        /// <summary>
        /// Log an informational message to the console.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Log(string message)
        {
            Log(message, LogType.Information);
        }

        /// <summary>
        /// Log a message to the console.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="type">The type of message to log.</param>
        public static void Log(string message, LogType type)
        {
            if (type != LogType.Information)
            {
                Console.ForegroundColor = Logger.outputColours[type];
            }

            Console.Write(string.Format("{0}{1}", Logger.messagePrefix[type], message));
            Console.ResetColor();
        }

        /// <summary>
        /// Log an informational verbose message to the console.
        /// </summary>
        /// <param name="message"></param>
        public static void Verbose(string message)
        {
            Verbose(message, LogType.Information);
        }

        /// <summary>
        /// Log a verbose message to the console.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="type">The type of message to log.</param>
        public static void Verbose(string message, LogType type)
        {
            if (Logger.verbose)
            {
                Log(message, type);
            }
        }
    }

    /// <summary>
    /// Types of log messages that can be output via the <see cref="Logger"/> class.
    /// </summary>
    internal enum LogType
    {
        /// <summary>
        /// An information message which can be largely ignored.
        /// </summary>
        Information,

        /// <summary>
        /// A fatal error or issue which the user must know about and describes an interupption
        /// to the current process.
        /// </summary>
        Error,

        /// <summary>
        /// A message describing an issue that may need further investigation but will not
        /// interupt the current process.
        /// </summary>
        Warning,

        /// <summary>
        /// A message detailing a major step forward in the process.
        /// </summary>
        Progress
    }
}
