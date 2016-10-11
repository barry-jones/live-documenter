using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Documentation
{
    public interface IDocumentMapper
    {
        void GenerateMap();

        Entry GenerateDocumentForAssembly(DocumentedAssembly documentedAssembly, ref int fileCounter);

        DocumentMap DocumentMap { get; }

        event EventHandler<PreEntryAddedEventArgs> PreEntryAdded;
    }
}
