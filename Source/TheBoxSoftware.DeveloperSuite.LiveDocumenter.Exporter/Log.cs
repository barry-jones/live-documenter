
namespace TheBoxSoftware.Exporter
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Class for logging output to the System.Console.
    /// </summary>
    /// <include file='Documentation\logger.xml' path='members/member[@name="Logger"]/*'/>
    internal class Logger : ILog
    {
        private readonly Dictionary<LogType, ConsoleColor> _outputColours;
        private readonly Dictionary<LogType, string> _messagePrefix;
        private bool _verbose = false;
        private readonly IUserInterface _ui;

        public Logger(IUserInterface ui)
        {
            _outputColours = new Dictionary<LogType, ConsoleColor> {
                { LogType.Information, ConsoleColor.White },
                { LogType.Warning, ConsoleColor.Yellow },
                { LogType.Error, ConsoleColor.Red },
                { LogType.Progress, ConsoleColor.Green }
                };
            _messagePrefix = new Dictionary<LogType, string> {
                { LogType.Information,  "" },
                { LogType.Progress,  "" },
                { LogType.Warning,      "[wrn] " },
                { LogType.Error,        "[err] " }
                };
            _ui = ui;
        }

        /// <summary>
        /// Initialises the logger with details that control how this application instance will run.
        /// </summary>
        /// <param name="verbose">Indicates if verbose messages should be logged.</param>
        public void Init(bool verbose)
        {
            _verbose = verbose;
        }

        /// <summary>
        /// Log an informational message to the console.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Log(string message)
        {
            Log(message, LogType.Information);
        }

        /// <summary>
        /// Log a message to the console.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="type">The type of message to log.</param>
        public void Log(string message, LogType type)
        {
            if(type != LogType.Information)
            {
                _ui.ForegroundColor = _outputColours[type];
            }

            _ui.Write($"{_messagePrefix[type]}{message}");
            _ui.ResetColor();
        }

        /// <summary>
        /// Log an informational verbose message to the console.
        /// </summary>
        /// <param name="message"></param>
        public void Verbose(string message)
        {
            Verbose(message, LogType.Information);
        }

        /// <summary>
        /// Log a verbose message to the console.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="type">The type of message to log.</param>
        public void Verbose(string message, LogType type)
        {
            if(_verbose)
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