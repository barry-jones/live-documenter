using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter {
    /// <summary>
    /// An entry in the document map that indicates that there are no entries. Used
    /// to allow the software to make sure no action occurs when this element is
    /// activated. Also allows a seperate style to be applied.
    /// </summary>
    internal sealed class EmptyEntry : Entry {
		/// <summary>
		/// Initialises a new instance of the EmptyEntry.
		/// </summary>
		/// <param name="displayText">The text the enty should display.</param>
        public EmptyEntry(string displayText) : base(null, displayText, null) { }
    }
}
