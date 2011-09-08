using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using TheBoxSoftware.Reflection.Comments;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Model
{
	class XmlCommentParserException : Exception, ISerializable {
		/// <summary>
		/// Initialises a new instance of the XmlCommentParserException.
		/// </summary>
		public XmlCommentParserException() : base() {}

		/// <summary>
		/// Initialises a new instance of the XmlCommentParserException.
		/// </summary>
		/// <param name="message">Message describing the error.</param>
		public XmlCommentParserException(string message) : base(message) {}
		
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
			: base (message, innerException) { 
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
			if(info == null) throw new ArgumentNullException("info");

			base.GetObjectData(info, context);
		}
	}
}
