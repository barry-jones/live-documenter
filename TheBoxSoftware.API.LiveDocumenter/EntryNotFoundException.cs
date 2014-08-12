using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.API.LiveDocumentor {
    public class EntryNotFoundException : Exception {
        internal EntryNotFoundException() : base() { }
        internal EntryNotFoundException(string message) : base(message) { }
        internal EntryNotFoundException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) 
            : base(info, context) { }
    }
}
