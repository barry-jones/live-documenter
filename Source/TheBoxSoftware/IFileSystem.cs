
namespace TheBoxSoftware
{
    using System.IO;

    public interface IFileSystem
    {
        bool FileExists(string filename);

        byte[] ReadAllBytes(string _fileName);
    }

    public class FileSystem : IFileSystem
    {
        public static FileSystem Singleton = new FileSystem();

        public FileSystem() { }

        public bool FileExists(string filename)
        {
            return File.Exists(filename);
        }

        public byte[] ReadAllBytes(string filename)
        {
            return File.ReadAllBytes(filename);
        }
    }
}
