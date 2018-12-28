
namespace TheBoxSoftware.Documentation.Exporting
{
    using System.IO;
    using Ionic.Zip;
    using System;

    public class IonicsCompressedConfigFile : ICompressedConfigFile
    {
        private readonly string _filename;
        private bool _disposed = false;
        private ZipFile _file;

        public IonicsCompressedConfigFile(string filename)
        {
            _filename = filename;
        }

        public Stream GetEntry(string entryName)
        {
            ZipFile file = GetFile();
            
            MemoryStream imageStream = new MemoryStream();
            file[entryName].Extract(imageStream);
            imageStream.Seek(0, SeekOrigin.Begin);
            return imageStream;            
        }

        public bool HasEntry(string entryName)
        {
            ZipFile file = GetFile();
            return file.ContainsEntry(entryName);
        }

        public void ExtractEntry(string entry, string toLocation)
        {
            ZipFile file = GetFile();

            if (file[entry].IsDirectory)
            {
                file.ExtractSelectedEntries("name = *.*", file[entry].FileName, toLocation, ExtractExistingFileAction.OverwriteSilently);
            }
            else
            {
                file[entry].Extract(toLocation);
            }
        }

        public CompressedFileEntry GetEntryDetails(string entry)
        {
            ZipFile file = GetFile();

            CompressedFileEntry details = new CompressedFileEntry();
            details.FileName = file[entry].FileName;
            details.IsDirectory = file[entry].IsDirectory;
            return details;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private ZipFile GetFile()
        {
            if (_disposed) throw new ObjectDisposedException("IonicsCompressedConfigFile");
            if (_file == null) _file = new ZipFile(_filename);

            return _file;
        }

        private void Dispose(bool disposing)
        {
            _disposed = true;
            if(disposing)
            {
                _file.Dispose();
            }
        }
    }
}
