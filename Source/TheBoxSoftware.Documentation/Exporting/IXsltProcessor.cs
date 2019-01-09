
namespace TheBoxSoftware.Documentation.Exporting
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public interface IXsltProcessor : IDisposable
    {
        void CompileXslt(Stream xsltStream);

        void Transform(string inputFile, string outputFile);

        Task TransformAsync(string current, string outputFile);
    }
}
