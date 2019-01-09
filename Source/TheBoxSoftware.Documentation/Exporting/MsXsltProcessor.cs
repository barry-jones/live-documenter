
namespace TheBoxSoftware.Documentation.Exporting
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Xsl;

    /// <summary>
    /// MS .NET Core XSLT Processor which support version 1.0 of the XSLT spec.
    /// </summary>
    public class MsXsltProcessor : IXsltProcessor
    {
        private bool _disposedValue = false;
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

        public async Task TransformAsync(string inputFile, string outputFile)
        {
            Action action = () =>
            {
                XsltArgumentList arguments = new XsltArgumentList();
                arguments.AddParam("directory", "", _xmlDirectory);

                using (StreamWriter writer = new StreamWriter(outputFile))
                {
                    _transform.Transform(inputFile, arguments, writer);
                }
            };
            await Task.Run(action);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(!_disposedValue)
            {
                if(disposing)
                {
                    _transform = null;
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
