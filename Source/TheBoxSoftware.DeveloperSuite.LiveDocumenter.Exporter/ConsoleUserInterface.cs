
namespace TheBoxSoftware.Exporter
{
    using System;

    internal class ConsoleUserInterface : IUserInterface
    {
        public ConsoleColor ForegroundColor { set => Console.ForegroundColor = value; }

        public void ResetColor()
        {
            Console.ResetColor();
        }

        public void Write(string content)
        {
            Console.Write(content);
        }

        public void WriteLine(string content)
        {
            Console.WriteLine(content);
        }
    }
}
