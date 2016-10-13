using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Documentation
{
    public interface IDocumentMapper
    {
        DocumentMap GenerateMap();

        event EventHandler<PreEntryAddedEventArgs> PreEntryAdded;
    }
}
