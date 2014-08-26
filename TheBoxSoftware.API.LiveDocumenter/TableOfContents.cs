using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.API.LiveDocumentor {
    using TheBoxSoftware.Documentation;

    /// <include file='Documentation\tableofcontents.xml' path='members/member[@name="tableofcontents"]/*'/>
    // basically acts as wrapper for the document map instance
    public sealed class TableOfContents : List<ContentsEntry> {
        private DocumentMap map;
        private bool isValid;                       // flag indicating if this map is still valid

        // initialises the toc class with the map reference.. this whole class will have
        // to be invalidated when the documentation is reloaded. <HOW?>
        internal TableOfContents(DocumentMap map) {
            this.map = map;
            this.isValid = true;
        }

        // invalidates the document map, so we can force the user of this instance to get a new instance
        // from the documentation - alternatively we can replace the map behind the scenes so it always
        // points to a clean reference.
        internal void Invalidate() {
            this.isValid = false;
        }
    }
}
