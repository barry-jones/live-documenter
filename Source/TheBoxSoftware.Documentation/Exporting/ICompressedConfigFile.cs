
namespace TheBoxSoftware.Documentation.Exporting
{
    using System.IO;

    public interface ICompressedConfigFile
    {
        Stream GetEntry(string entryName);

        bool HasEntry(string entryName);
    }
}
