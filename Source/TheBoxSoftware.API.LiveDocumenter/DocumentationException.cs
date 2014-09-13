using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.API.LiveDocumenter {
    /// <summary>
    /// Details an exception with the documentation.
    /// </summary>
    public class DocumentationException : Exception {

        internal DocumentationException() : base() { }

        internal DocumentationException(string message) : base(message) { }

        internal DocumentationException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) 
            : base(info, context) { }
    }
}
