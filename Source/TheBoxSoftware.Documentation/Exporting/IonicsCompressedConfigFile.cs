
namespace TheBoxSoftware.Documentation.Exporting
{
    using System.IO;
    using Ionic.Zip;

    public class IonicsCompressedConfigFile : ICompressedConfigFile
    {
        private readonly string _filename;

        public IonicsCompressedConfigFile(string filename)
        {
            _filename = filename;
        }

        public Stream GetEntry(string entryName)
        {
            using (ZipFile file = new ZipFile(_filename))
            {
                MemoryStream imageStream = new MemoryStream();
                file[entryName].Extract(imageStream);
                imageStream.Seek(0, SeekOrigin.Begin);
                return imageStream;
            }
        }

        public bool HasEntry(string entryName)
        {
            using (ZipFile file = new ZipFile(_filename))
            {
                return file.ContainsEntry(entryName);
            }
        }
    }
}
