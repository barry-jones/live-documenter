
namespace TheBoxSoftware.API.LiveDocumenter
{
    using System;

    /// <summary>
    /// Details an error where an entry was not found in the documentation.
    /// </summary>
    public class EntryNotFoundException : Exception 
    {

        internal EntryNotFoundException() : base() { }

        internal EntryNotFoundException(string message) : base(message) { }

        internal EntryNotFoundException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) 
            : base(info, context) { }
    }
}
