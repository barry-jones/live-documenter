
namespace TheBoxSoftware.Exporter
{
    public interface ILog
    {
        void Log(string message);

        void Log(string message, LogType type);

        void Verbose(string message);

        void Verbose(string message, LogType type);

        void Init(bool verbose);
    }
}
