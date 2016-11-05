
namespace TheBoxSoftware.API.LiveDocumenter
{
    using System;

    /// <summary>
    /// Details an exception with the documentation.
    /// </summary>
    public class DocumentationException : Exception 
    {

        internal DocumentationException() : base() { }

        internal DocumentationException(string message) : base(message) { }

        internal DocumentationException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) 
            : base(info, context) { }
    }
}
