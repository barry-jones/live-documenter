
namespace TheBoxSoftware.Documentation
{
    using System;

    public interface IDocumentMapper
    {
        DocumentMap GenerateMap();

        event EventHandler<PreEntryAddedEventArgs> PreEntryAdded;
    }
}
