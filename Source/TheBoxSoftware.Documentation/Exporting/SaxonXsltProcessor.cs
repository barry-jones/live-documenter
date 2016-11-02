
namespace TheBoxSoftware.Documentation.Exporting
{
    using System;
    using System.IO;
    using System.Xml;
    using Saxon.Api;

    /// <summary>
    /// An implementation of the IXSltProcesser using an underlying Saxon-HE XSLT compiled
    /// transform. Support XSLT 2.0 but can not be ported to .net standard.
    /// </summary>
    public class SaxonXsltProcessor : IXsltProcessor
    {
        private Processor _processor;
        private XsltTransformer _transform;
        private bool _disposedValue = false;
        private readonly string _xmlDirectory;

        public SaxonXsltProcessor(string xmlDirectory)
        {
            _processor = new Processor();
            _xmlDirectory = xmlDirectory;
        }

        public void CompileXslt(Stream xsltStream)
        {
            _transform = _processor
                .NewXsltCompiler()
                .Compile(xsltStream)
                .Load();

            _transform.SetParameter(
                new QName(new XmlQualifiedName("directory")), 
                new XdmAtomicValue(Path.GetFullPath(_xmlDirectory))
                );
        }

        public void Transform(string inputFile, string outputFile)
        {
            using(FileStream fs = File.OpenRead(inputFile))
            {
                Serializer s = new Serializer();
                s.SetOutputFile(outputFile);

                _transform.SetInputStream(
                    fs, 
                    new Uri(new Uri(System.Reflection.Assembly.GetExecutingAssembly().Location), 
                    _xmlDirectory)
                    );
                _transform.Run(s);

                s.Close();
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
                    _processor = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
