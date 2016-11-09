
namespace TheBoxSoftware.Reflection.Comments
{
    using System;
    using System.Xml;
    using System.Xml.XPath;

    /// <summary>
    /// A container and manager class for the xml code comments files associated
    /// with libraries.
    /// </summary>
    public class XmlCodeCommentFile : ICommentSource
    {
        private string _xmlCommentFileName;
        protected bool _exists;

        /// <summary>
        /// Initialises a new instance of the XmlCodeCommentFile
        /// </summary>
        /// <param name="xmlCommentFile">The file to parse.</param>
        public XmlCodeCommentFile(string xmlCommentFile)
        {
            _xmlCommentFileName = xmlCommentFile;
            _exists = System.IO.File.Exists(xmlCommentFile);
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public XmlCodeCommentFile() { }

        public bool Exists()
        {
            return _exists;
        }

        /// <summary>
        /// Reads the XML code comments for the member specified <paramref name="forMember"/>.
        /// </summary>
        /// <param name="forMember">The CRefPath to read the xml code comments for.</param>
        /// <returns>The <see cref="XmlCodeComment"/>.</returns>
        public XmlCodeComment GetComment(CRefPath forMember)
        {
            string xpath = $"/doc/members/member[@name=\"{forMember.ToString()}\"]";
            return GetComment(xpath);
        }

        public XmlCodeComment GetSummary(CRefPath forMember)
        {
            string xpath = $"/doc/members/member[@name=\"{forMember.ToString()}\"]/summary";
            return GetComment(xpath);
        }

        /// <summary>
        /// Reads the XML code comments for the specified xpath. This method allows callers
        /// to be specific about which top-level code comment elements are loaded in to the
        /// comment.
        /// </summary>
        /// <param name="xpath">The xpath path to load the comments of.</param>
        /// <returns>The XmlCodeComment</returns>
        public XmlCodeComment ReadComment(string xpath)
        {
            return GetComment(xpath);
        }

        /// <summary>
        /// Reads the XML code comments for the specified xpath. This method allows callers
        /// to be specific about which top-level code comment elements are loaded in to the
        /// comment.
        /// </summary>
        /// <param name="xpath">The xpath path to load the comments of.</param>
        /// <returns>The XmlCodeComment</returns>
        protected virtual XmlCodeComment GetComment(string xpath)
        {
            XmlCodeComment parsedComment = XmlCodeComment.Empty;

            if(Exists())
            {
                XPathDocument commentsDocument = new XPathDocument(_xmlCommentFileName);
                XPathNavigator n = commentsDocument.CreateNavigator();
                XPathNodeIterator ni = n.Select(xpath);
                XmlNode memberComment = null;
                if(ni.MoveNext())
                {
                    using(System.IO.StringReader reader = new System.IO.StringReader(ni.Current.OuterXml))
                    {
                        XmlReader xmlReader = XmlTextReader.Create(reader);
                        XmlDocument tempD = new XmlDocument();
                        memberComment = tempD.ReadNode(xmlReader);
                    }
                }

                if(memberComment != null)
                {
                    try
                    {
                        parsedComment = new XmlCodeComment(memberComment);
                    }
                    catch(Exception ex)
                    {
                        // we cant fix this problem but we do need to add more details
                        // to the exception
                        throw new XmlCommentException(memberComment.InnerXml, "An error occurred while parsing XML comments", ex);
                    }
                }
            }

            return parsedComment;
        }

        /// <summary>
        /// Obtains the original XML for the specified <paramref name="cref"/>.
        /// </summary>
        /// <param name="cref">The member to get the original XML for</param>
        /// <returns>The original XML for the specified member</returns>
        public virtual string GetXmlFor(CRefPath cref)
        {
            string xpath = $"/doc/members/member[@name=\"{cref.ToString()}\"]";
            string xml = string.Empty;

            if(Exists())
            {
                XPathDocument commentsDocument = new XPathDocument(_xmlCommentFileName);
                XPathNavigator n = commentsDocument.CreateNavigator();
                XPathNodeIterator ni = n.Select(xpath);
                XmlNode memberComment = null;
                if(ni.MoveNext())
                {
                    using(System.IO.StringReader reader = new System.IO.StringReader(ni.Current.OuterXml))
                    {
                        XmlReader xmlReader = XmlTextReader.Create(reader);
                        XmlDocument tempD = new XmlDocument();
                        memberComment = tempD.ReadNode(xmlReader);
                    }
                }

                if(memberComment != null)
                {
                    xml = memberComment.InnerXml;
                }
            }

            return xml;
        }

        /// <summary>
        /// Obtains an instance of the <see cref="ReusableXmlCodeCommentFile"/>.
        /// </summary>
        /// <returns>The instance.</returns>
        public ReusableXmlCodeCommentFile GetReusableFile()
        {
            return new ReusableXmlCodeCommentFile(_xmlCommentFileName, Exists());
        }
        
        /// <summary>
        /// A re-usable copy of the XmlCodeCommentFile. This class is more
        /// resource intensive and should not be kept for long periods of time.
        /// However it does speed up the process of reading many elements from
        /// the xml file in iterations.
        /// </summary>
        public sealed class ReusableXmlCodeCommentFile : XmlCodeCommentFile
        {
            private XPathDocument commentsDocument;
            private XPathNavigator navigator;

            /// <summary>
            /// Initialises a new instance of the ReusableXmlCodeCommentFile class.
            /// </summary>
            /// <param name="file">The filename of the xml comments file.</param>
            /// <param name="exists">Indicates if the file exists.</param>
            internal ReusableXmlCodeCommentFile(string file, bool exists)
            {
                _xmlCommentFileName = file;
                _exists = exists;
                if(exists)
                {
                    commentsDocument = new XPathDocument(file);
                    navigator = commentsDocument.CreateNavigator();
                }
            }

            /// <summary>
            /// <para>
            /// Overrides the basic implementation for retrieving a comment, this method
            /// utilises the expensive but much quicker in memory representation of the 
            /// xml file.
            /// </para>
            /// <para>
            /// If you only need to read a single comment you should use a XmlCodeCommentFile
            /// instance.
            /// </para>
            /// </summary>
            /// <param name="xpath">The XPath expression to search for.</param>
            /// <returns>The XmlCodeComment found or XmlCodeComment.Empty if not.</returns>
            protected override XmlCodeComment GetComment(string xpath)
            {
                XmlCodeComment parsedComment = XmlCodeComment.Empty;

                if(this.Exists())
                {
                    XPathNodeIterator ni = navigator.Select(xpath);
                    XmlNode memberComment = null;
                    if(ni.MoveNext())
                    {
                        using(System.IO.StringReader reader = new System.IO.StringReader(ni.Current.OuterXml))
                        {
                            XmlReader xmlReader = XmlTextReader.Create(reader);
                            XmlDocument tempD = new XmlDocument();
                            memberComment = tempD.ReadNode(xmlReader);
                        }
                    }

                    if(memberComment != null)
                    {
                        try
                        {
                            parsedComment = new XmlCodeComment(memberComment);
                        }
                        catch(Exception ex)
                        {
                            // we cant fix this problem but we do need to add more details
                            // to the exception
                            throw new XmlCommentException(memberComment.InnerXml, "An error occurred while parsing XML comments", ex);
                        }
                    }
                }

                return parsedComment;
            }

            /// <summary>
            /// Obtains the original XML for the specified <paramref name="cref"/>.
            /// </summary>
            /// <param name="cref">The member to get the original XML for</param>
            /// <returns>The original XML for the specified member</returns>
            public override string GetXmlFor(CRefPath cref)
            {
                string xpath = $"/doc/members/member[@name=\"{cref.ToString()}\"]";

                string xml = string.Empty;

                if(this.Exists())
                {
                    XPathNodeIterator ni = navigator.Select(xpath);
                    XmlNode memberComment = null;
                    if(ni.MoveNext())
                    {
                        using(System.IO.StringReader reader = new System.IO.StringReader(ni.Current.OuterXml))
                        {
                            XmlReader xmlReader = XmlTextReader.Create(reader);
                            XmlDocument tempD = new XmlDocument();
                            memberComment = tempD.ReadNode(xmlReader);
                        }
                    }

                    if(memberComment != null)
                    {
                        xml = memberComment.InnerXml;
                    }
                }

                return xml;
            }
        }
    }
}