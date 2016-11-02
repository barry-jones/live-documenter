
namespace TheBoxSoftware.Documentation.Exporting
{
    using System.IO;
    using System.Xml;
    using System.Xml.Xsl;

    /// <summary>
    /// Implementation of the IXsltProcesser used by the export routines which uses an underlying
    /// System.Xml.XslCompiledTransform. 
    /// </summary>
    /// <remarks>
    /// This does not currently work as it uses XSLT 1.0 which does not support some of the functionality
    /// that is being used in the XSLT in the exporters. But if used would allow us to move to .NET standard
    /// for this and other libraries.
    /// </remarks>
    public class MsXsltProcessor : IXsltProcessor
    {
        private bool _disposedValue = false; // To detect redundant calls
        private XslCompiledTransform _transform;
        private readonly string _xmlDirectory;

        public MsXsltProcessor(string xmlDirectory)
        {
            _transform = new XslCompiledTransform();
            _xmlDirectory = xmlDirectory;
        }

        public void CompileXslt(Stream xsltStream)
        {
            using(XmlReader reader = XmlReader.Create(xsltStream))
            {
                XsltSettings settings = new XsltSettings();
                settings.EnableDocumentFunction = true;

                _transform.Load(reader, settings, new XmlUrlResolver());
            }
        }

        public void Transform(string inputFile, string outputFile)
        {
            XsltArgumentList arguments = new XsltArgumentList();
            arguments.AddParam("directory", "", _xmlDirectory);

            using(StreamWriter writer = new StreamWriter(outputFile))
            {
                _transform.Transform(inputFile, arguments, writer);
            }
        }


        protected virtual void Dispose(bool disposing)
        {
            if(!_disposedValue)
            {
                if(disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _transform = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                _disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }
    }
}
