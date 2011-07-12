using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace TheBoxSoftware.Reflection.Comments {
	/// <summary>
	/// A container and manager class for the xml code comments files associated
	/// with libraries.
	/// </summary>
	public class XmlCodeCommentFile {
		private string xmlCommentFileName;

		/// <summary>
		/// Initialises a new instance of the XmlCodeCommentFile
		/// </summary>
		/// <param name="xmlCommentFile">The file to parse.</param>
		public XmlCodeCommentFile(string xmlCommentFile) {
			this.xmlCommentFileName = xmlCommentFile;
			this.Exists = System.IO.File.Exists(xmlCommentFile);
		}

		/// <summary>
		/// Default constructor
		/// </summary>
		public XmlCodeCommentFile() { }

		/// <summary>
		/// Reads the XML code comments for the member specified <paramref name="forMember"/>.
		/// </summary>
		/// <param name="forMember">The CRefPath to read the xml code comments for.</param>
		/// <returns>The <see cref="XmlCodeComment"/>.</returns>
		public XmlCodeComment ReadComment(CRefPath forMember) {
			string xpath = string.Format(
					"/doc/members/member[@name=\"{0}\"]",
					forMember.ToString()
					);
			return this.GetComment(xpath);
		}

		/// <summary>
		/// Reads the XML code comments for the specified xpath. This method allows callers
		/// to be specific about which top-level code comment elements are loaded in to the
		/// comment.
		/// </summary>
		/// <param name="xpath">The xpath path to load the comments of.</param>
		/// <returns>The XmlCodeComment</returns>
		public XmlCodeComment ReadComment(string xpath) {
			return this.GetComment(xpath);
		}

		/// <summary>
		/// Reads the XML code comments for the specified xpath. This method allows callers
		/// to be specific about which top-level code comment elements are loaded in to the
		/// comment.
		/// </summary>
		/// <param name="xpath">The xpath path to load the comments of.</param>
		/// <returns>The XmlCodeComment</returns>
		protected virtual XmlCodeComment GetComment(string xpath) {
			XmlCodeComment parsedComment = XmlCodeComment.Empty;

			if (this.Exists) {
				XPathDocument commentsDocument = new XPathDocument(this.xmlCommentFileName);
				XPathNavigator n = commentsDocument.CreateNavigator();
				XPathNodeIterator ni = n.Select(xpath);
				XmlNode memberComment = null;
				if (ni.MoveNext()) {
					using (System.IO.StringReader reader = new System.IO.StringReader(ni.Current.OuterXml)) {
						XmlReader xmlReader = XmlTextReader.Create(reader);
						XmlDocument tempD = new XmlDocument();
						memberComment = tempD.ReadNode(xmlReader);
					}
				}

				if (memberComment != null) {
					parsedComment = new XmlCodeComment(memberComment);
				}
			}

			return parsedComment;
		}

		/// <summary>
		/// Obtains an instance of the <see cref="ReusableXmlCodeCommentFile"/>.
		/// </summary>
		/// <returns>The instance.</returns>
		public ReusableXmlCodeCommentFile GetReusableFile() {
			return new ReusableXmlCodeCommentFile(this.xmlCommentFileName, this.Exists);
		}

		#region Properties
		/// <summary>
		/// Indicates if the xml code comment file exists.
		/// </summary>
		public bool Exists { get; set; }
		#endregion

		#region Internals
		/// <summary>
		/// A re-usable copy of the XmlCodeCommentFile. This class is more
		/// resource intensive and should not be kept for long periods of time.
		/// However it does speed up the process of reading many elements from
		/// the xml file in iterations.
		/// </summary>
		public class ReusableXmlCodeCommentFile : XmlCodeCommentFile {
			XPathDocument commentsDocument;
			XPathNavigator navigator;
			internal ReusableXmlCodeCommentFile(string file, bool exists) {
				this.xmlCommentFileName = file;
				this.Exists = exists;
				if (exists) {
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
			protected override XmlCodeComment GetComment(string xpath) {
				XmlCodeComment parsedComment = XmlCodeComment.Empty;

				if (this.Exists) {
					XPathNodeIterator ni = navigator.Select(xpath);
					XmlNode memberComment = null;
					if (ni.MoveNext()) {
						using (System.IO.StringReader reader = new System.IO.StringReader(ni.Current.OuterXml)) {
							XmlReader xmlReader = XmlTextReader.Create(reader);
							XmlDocument tempD = new XmlDocument();
							memberComment = tempD.ReadNode(xmlReader);
						}
					}

					if (memberComment != null) {
						parsedComment = new XmlCodeComment(memberComment);
					}
				}

				return parsedComment;
			}
		}
		#endregion
	}
}
