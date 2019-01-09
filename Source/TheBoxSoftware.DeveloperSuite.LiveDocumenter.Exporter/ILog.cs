
namespace TheBoxSoftware.Exporter
{
    public interface ILog
    {
        void LogInformation(string message);

        void LogWarning(string message);

        void LogError(string message);

        void LogProgress(string message);
    }
}
