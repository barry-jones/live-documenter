
namespace TheBoxSoftware.Exporter
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    internal interface IUserInterface
    {
        void Write(string content);

        void WriteLine(string content);

        void ResetColor();

        ConsoleColor ForegroundColor { set; }
    }
}
