
namespace TheBoxSoftware
{
    using System.IO;

    public interface IFileSystem
    {
        bool FileExists(string filename);
    }

    public class FileSystem : IFileSystem
    {
        public static FileSystem Singleton = new FileSystem();

        public FileSystem() { }

        public bool FileExists(string filename)
        {
            return File.Exists(filename);
        }
    }
}
