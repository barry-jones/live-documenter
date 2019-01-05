
namespace TheBoxSoftware.Exporter
{
    using System;

    public interface IUserInterface
    {
        void Write(string content);

        void WriteLine(string content);

        void ResetColor();

        ConsoleColor ForegroundColor { set; }
    }
}
