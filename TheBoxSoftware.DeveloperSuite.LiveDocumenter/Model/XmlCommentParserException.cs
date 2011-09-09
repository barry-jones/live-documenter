using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using TheBoxSoftware.Reflection.Comments;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Model {
	/// <summary>
	/// Describes an error that has occurred while parsing an XmlCodeComment to 
	/// WPF Document elements.
	/// </summary>
	public class XmlCommentParserException : Exception, ISerializable, IExtendedException {
		/// <summary>
		/// Initialises a new instance of the XmlCommentParserException.
		/// </summary>
		public XmlCommentParserException() : base() { }

		/// <summary>
		/// Initialises a new instance of the XmlCommentParserException.
		/// </summary>
		/// <param name="message">Message describing the error.</param>
		public XmlCommentParserException(string message) : base(message) { }

		/// <summary>
		/// Initialises a new instance of the XmlCommentParserException.
		/// </summary>
		/// <param name="comment">The XmlCodeCommnet being parsed when the error occurred.</param>
		/// <param name="message">Message describing the error.</param>
		public XmlCommentParserException(XmlCodeComment comment, string message)
			: base(message) {
			this.Comment = comment;
		}

		/// <summary>
		/// Initialises a new instance of the XmlCommentParserException.
		/// </summary>
		/// <param name="message">Message describing the error.</param>
		/// <param name="innerException">The thrown exception.</param>
		public XmlCommentParserException(string message, Exception innerException)
			: base(message, innerException) {
		}

		/// <summary>
		/// Initialises a new instance of the XmlCommentParserException.
		/// </summary>
		/// <param name="comment">The XmlCodeCommnet being parsed when the error occurred.</param>
		/// <param name="innerException">The thrown exception.</param>
		public XmlCommentParserException(XmlCodeComment comment, Exception innerException)
			: base(string.Empty, innerException) {
			this.Comment = comment;
		}

		/// <summary>
		/// Initialises a new instance of the XmlCommentParserException.
		/// </summary>
		/// <param name="comment">The XmlCodeCommnet being parsed when the error occurred.</param>
		/// <param name="message">Message describing the error.</param>
		/// <param name="innerException">The thrown exception.</param>
		public XmlCommentParserException(XmlCodeComment comment, string message, Exception innerException)
			: base(message, innerException) {
			this.Comment = comment;
		}

		/// <summary>
		/// Serialization constructor.
		/// </summary>
		/// <param name="info">The info describing the exception.</param>
		/// <param name="context">The serialization context.</param>
		protected XmlCommentParserException(SerializationInfo info, StreamingContext context)
			: base(info, context) {
		}

		/// <summary>
		/// The Comment being read when the exception was throwns.
		/// </summary>
		public XmlCodeComment Comment { get; set; }

		/// <summary>
		/// Serializes the custom details of this exception to the SerializationInfo.
		/// </summary>
		/// <param name="info">The info to populate with custom details</param>
		/// <param name="context">The context</param>
		public override void GetObjectData(SerializationInfo info, StreamingContext context) {
			if (info == null) throw new ArgumentNullException("info");

			base.GetObjectData(info, context);
		}

		#region IExtendedException Members
		/// <summary>
		/// Extracts as much information from the state of the exception as possible and returns
		/// it as a formatted string.
		/// </summary>
		/// <returns>The formatted exception details.</returns>
		public string GetExtendedInformation() {
			StringBuilder builder = new StringBuilder();

			if (this.Comment != null) {
				if (this.Comment == XmlCodeComment.Empty) {
					builder.AppendLine("The associated XmlCodeComment is equal to Empty");
				}
				else {
					if (this.Comment.Member != null) {
						// get the cref path of the member
						try {
							builder.AppendLine(string.Format("CRef: {0}", this.Comment.Member.ToString()));
						}
						catch(Exception) {}

						// if its not null lets just see if we can the base xml for this
						try {
							LiveDocumentorFile sf = LiveDocumentorFile.Singleton;
							if (sf != null) {
								if (sf.LiveDocument != null) {
									Documentation.Entry entry = sf.LiveDocument.Find(this.Comment.Member);
									if (entry != null) {
										builder.AppendLine(string.Format("Has XML Comments: {0}", entry.HasXmlComments));

										if (entry.XmlCommentFile != null) {
											builder.AppendLine(string.Format("XML: {0}", entry.XmlCommentFile.GetXmlFor(this.Comment.Member)));
										}
									}
								}
							}
						}
						catch(Exception) {}
					}

					builder.AppendLine("Formatted Comment");
					this.WriteAsText(builder, this.Comment);
					builder.AppendLine();
				}
			}
			else {
				builder.AppendLine("No XmlCodeComment was stored in the exception");
			}

			return builder.ToString();
		}

		private void WriteAsText(StringBuilder builder, XmlCodeComment comment) {
			builder.Append("{");
			builder.Append(comment.Element.ToString());
			builder.Append(":");
			builder.Append(comment.Text);
			foreach (XmlCodeElement element in comment.Elements) {
				this.WriteAsText(builder, element);
			}
			builder.Append("}");
		}

		private void WriteAsText(StringBuilder builder, XmlCodeElement element) {
			builder.Append("{");
			builder.Append(element.Element.ToString());
			builder.Append(":");
			builder.Append(element.Text);
			if(element is XmlContainerCodeElement) {
				foreach (XmlCodeElement child in ((XmlContainerCodeElement)element).Elements) {
					this.WriteAsText(builder, child);
				}
			}
			builder.Append("}");
		}
		#endregion
	}
}