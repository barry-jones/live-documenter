
namespace TheBoxSoftware.Documentation.Exporting
{
    using System;
    using System.IO;

    public interface ICompressedConfigFile : IDisposable
    {
        Stream GetEntry(string entryName);

        bool HasEntry(string entryName);

        void ExtractEntry(string entry, string location);

        CompressedFileEntry GetEntryDetails(string entry);
    }
}
