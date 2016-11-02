
namespace TheBoxSoftware.Documentation.Exporting
{
    using System;
    using System.IO;

    public interface IXsltProcessor : IDisposable
    {
        void CompileXslt(Stream xsltStream);

        void Transform(string inputFile, string outputFile);
    }
}
