
namespace TheBoxSoftware
{
    using System.IO;

    public interface IFileSystem
    {
        bool FileExists(string filename);

        byte[] ReadAllBytes(string fileName);

        bool DirectoryExists(string directory);

        void DeleteDirectory(string directory, bool recursive);

        void CreateDirectory(string directory);

        void DeleteFile(string filename);
    }

    public class FileSystem : IFileSystem
    {
        public FileSystem() { }

        public bool FileExists(string filename)
        {
            return File.Exists(filename);
        }

        public void DeleteFile(string filename)
        {
            File.Delete(filename);
        }

        public byte[] ReadAllBytes(string filename)
        {
            return File.ReadAllBytes(filename);
        }

        public bool DirectoryExists(string directory)
        {
            return Directory.Exists(directory);
        }

        public void DeleteDirectory(string directory, bool recursive)
        {
            Directory.Delete(directory, true);
        }

        public void CreateDirectory(string directory)
        {
            Directory.CreateDirectory(directory);
        }
    }
}
