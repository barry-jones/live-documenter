
namespace TheBoxSoftware.Documentation.Exporting
{
    using System;
    using System.Linq;
    using System.IO;
    using System.IO.Compression;

    public class ZipCompressedConfigFile : ICompressedConfigFile
    {
        private bool _disposed = false;
        private readonly string _filename;
        private readonly IFileSystem _filesystem;
        private ZipArchive _file;

        public ZipCompressedConfigFile(string filename) 
            : this(filename, new FileSystem())
        {
        }

        public ZipCompressedConfigFile(string filename, IFileSystem fileSystem)
        {
            _filename = filename;
            _filesystem = fileSystem;
            _file = ZipFile.OpenRead(filename);
        }

        public void ExtractEntry(string entry, string location)
        {
            ZipArchiveEntry contents = _file.GetEntry(entry);
            Extract(location, contents);
        }

        public Stream GetEntry(string entryName)
        {
            return CreateDuplicate(_file.GetEntry(entryName).Open());
        }

        public CompressedFileEntry GetEntryDetails(string entry)
        {
            ZipArchiveEntry contents = _file.GetEntry(entry);
            CompressedFileEntry details = new CompressedFileEntry();

            details.FileName = contents.FullName;
            details.IsDirectory = contents.FullName.EndsWith("/");

            return details;
        }

        public bool HasEntry(string entryName)
        {
            return _file.Entries.First(p => p.FullName == entryName) != null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            _disposed = true;
            if(disposing)
            {
                _file.Dispose();
            }
        }

        private Stream CreateDuplicate(Stream streamToCopy)
        {
            MemoryStream copy = new MemoryStream();
            streamToCopy.CopyTo(copy);
            copy.Seek(0, SeekOrigin.Begin);
            return copy;
        }

        private void Extract(string location, ZipArchiveEntry entry)
        {
            string destinationPath = Path.GetFullPath(Path.Combine(location, entry.FullName));
            _filesystem.CreateDirectory(Path.GetDirectoryName(destinationPath));

            if (entry.FullName.EndsWith("/"))
            {
                var directoryEntries = from e in _file.Entries
                                       where e.FullName.StartsWith(entry.FullName) && e.FullName != entry.FullName
                                       select e;
                foreach (ZipArchiveEntry current in directoryEntries)
                {
                    Extract(location, current);
                }
            }
            else
            {
                entry.ExtractToFile(destinationPath);
            }
        }
    }
}
