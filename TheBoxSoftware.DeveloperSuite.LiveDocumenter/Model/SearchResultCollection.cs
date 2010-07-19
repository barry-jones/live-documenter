using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Model {
	internal sealed class SearchResultCollection : List<SearchResult> {
		public void AddEntriesToResults(List<Entry> entries) {
			foreach (Entry current in entries) {
				this.Add(new SearchResult(current));
			}
		}
	}
}
