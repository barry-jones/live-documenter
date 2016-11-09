
namespace TheBoxSoftware.Reflection.Comments
{
    using System;
    using System.IO;
    using System.Xml;
    using System.Xml.XPath;

    /// <summary>
    /// Implementation of ICommentSource that uses an underlying XML Comment File.
    /// </summary>
    public class XmlCommentFile : ICommentSource
    {
        private readonly string _filename;
        private readonly IFileSystem _fileSystem;

        private XPathDocument _document;
        private XPathNavigator _navigator;
        private bool _isLoaded = false;

        public XmlCommentFile(string filename, IFileSystem fileSystem)
        {
            _filename = filename;
            _fileSystem = fileSystem;
        }

        public void Load()
        {
            if(!Exists())
                return;

            using(MemoryStream stream = new MemoryStream(_fileSystem.ReadAllBytes(_filename)))
            {
                _document = new XPathDocument(stream);
                _navigator = _document.CreateNavigator();
            }
            _isLoaded = true;
        }

        public bool Exists()
        {
            if(string.IsNullOrEmpty(_filename))
            {
                return false;
            }
            return _fileSystem.FileExists(_filename);
        }

        public XmlCodeComment GetComment(CRefPath forMember)
        {
            if(forMember == null || !_isLoaded || forMember.PathType == CRefTypes.Error)
                return XmlCodeComment.Empty;
            string xpath = $"/doc/members/member[@name='{forMember.ToString()}']";

            XmlCodeComment comment = FindCommentWithXPath(xpath);

            return comment;
        }

        public XmlCodeComment GetSummary(CRefPath forMember)
        { 
            if(forMember == null || !_isLoaded || forMember.PathType == CRefTypes.Error)
                return XmlCodeComment.Empty;
            string xpath = $"/doc/members/member[@name='{forMember.ToString()}']/summary";

            XmlCodeComment comment = FindCommentWithXPath(xpath);

            return comment;
        }

        private XmlCodeComment FindCommentWithXPath(string xpath)
        {
            XmlCodeComment comment = XmlCodeComment.Empty;
            XmlNode commentXml = FindXmlElement(xpath);

            if(commentXml != null)
            {
                try
                {
                    comment = new XmlCodeComment(commentXml);
                }
                catch(Exception ex)
                {
                    // we cant fix this problem but we do need to add more details
                    // to the exception
                    throw new XmlCommentException(commentXml.InnerXml, "An error occurred while parsing XML comments", ex);
                }
            }

            return comment;
        }

        private XmlNode FindXmlElement(string xpath)
        {
            XPathNodeIterator iterator = _navigator.Select(xpath);
            XmlNode commentXml = null;

            if(iterator.MoveNext())
            {
                using(StringReader reader = new StringReader(iterator.Current.OuterXml))
                {
                    XmlReader xmlReader = XmlTextReader.Create(reader);
                    XmlDocument tempD = new XmlDocument();
                    commentXml = tempD.ReadNode(xmlReader);
                }
            }

            return commentXml;
        }

        public bool IsLoaded
        {
            get { return _isLoaded; }
        }
    }
}
