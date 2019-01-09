
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

        public void LogInformation(string message)
        {
            LogTemp(message, LogType.Information);
        }

        public void LogWarning(string message)
        {
            LogTemp(message, LogType.Warning);
        }

        public void LogError(string message)
        {
            LogTemp(message, LogType.Error);
        }

        public void LogProgress(string message)
        {
            LogTemp(message, LogType.Progress);
        }

        private void LogTemp(string message, LogType type)
        {
            _ui.ForegroundColor = _outputColours[type];
            _ui.Write($"{_messagePrefix[type]}{message}");
            _ui.ResetColor();
        }
    }

    /// <summary>
    /// Types of log messages that can be output via the <see cref="Logger"/> class.
    /// </summary>
    public enum LogType
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