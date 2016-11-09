
namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Model
{
    using System.Collections.Generic;
    using TheBoxSoftware.Documentation;

    internal sealed class SearchResultCollection : List<SearchResult>
    {
        public void AddEntriesToResults(List<Entry> entries)
        {
            foreach(Entry current in entries)
            {
                this.Add(new SearchResult(current));
            }
        }
    }
}