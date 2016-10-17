using System;
using System.Text;
using System.Runtime.Serialization;

namespace TheBoxSoftware.Reflection.Comments
{
    /// <summary>
    /// Describes errors that occur while using the members and methods defined
    /// to read XML code comments
    /// </summary>
    [Serializable]
    public class XmlCommentException : Exception, ISerializable, IExtendedException
    {
        /// <summary>
        /// Initialises a new instance of the XmlCommentException.
        /// </summary>
        public XmlCommentException() : base() { }

        /// <summary>
        /// Initialises a new instance of the XmlCommentException.
        /// </summary>
        /// <param name="message">Message describing the error.</param>
        public XmlCommentException(string message) : base(message) { }

        /// <summary>
        /// Initialises a new instance of the XmlCommentException.
        /// </summary>
        /// <param name="xml">The XML from the comment that caused the problem</param>
        /// <param name="message">Message describing the error.</param>
        public XmlCommentException(string xml, string message)
            : base(message)
        {
            this.Xml = xml;
        }

        /// <summary>
        /// Initialises a new instance of the XmlCommentException.
        /// </summary>
        /// <param name="message">Message describing the error.</param>
        /// <param name="innerException">The thrown exception.</param>
        public XmlCommentException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initialises a new instance of the XmlCommentException.
        /// </summary>
        /// <param name="xml">The XML from the comment that caused the problem</param>
        /// <param name="message">Message describing the error.</param>
        /// <param name="innerException">The thrown exception.</param>
        public XmlCommentException(string xml, string message, Exception innerException)
            : base(message, innerException)
        {
            this.Xml = xml;
        }

        /// <summary>
        /// Serialization constructor.
        /// </summary>
        /// <param name="info">The info describing the exception.</param>
        /// <param name="context">The serialization context.</param>
        protected XmlCommentException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.Xml = info.GetString("Xml");
        }

        /// <summary>
        /// The XML of the comment that caused the exception.
        /// </summary>
        public string Xml { get; set; }

        /// <summary>
        /// Serializes the custom details of this exception to the SerializationInfo.
        /// </summary>
        /// <param name="info">The info to populate with custom details</param>
        /// <param name="context">The context</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if(info == null) throw new ArgumentNullException("info");

            info.AddValue("Xml", this.Xml);

            base.GetObjectData(info, context);
        }

        /// <summary>
        /// Extracts as much information from the exception as possible and returns it as
        /// a formatted string.
        /// </summary>
        /// <returns>The formatted exception details.</returns>
        public string GetExtendedInformation()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(string.Format("XML: {0}", this.Xml));

            return builder.ToString();
        }
    }
}