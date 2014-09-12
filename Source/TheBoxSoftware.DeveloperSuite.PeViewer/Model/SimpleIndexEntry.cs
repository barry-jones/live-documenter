using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Model {
	internal class SimpleIndexEntry {
		public SimpleIndexEntry(string index, string value) {
			this.Index = index;
			this.Value = value;
		}

		public static List<SimpleIndexEntry> Create<T, U>(Dictionary<T, U> dictionary) {
			List<SimpleIndexEntry> entries = new List<SimpleIndexEntry>();
			foreach(KeyValuePair<T,U> current in dictionary){
				entries.Add(new SimpleIndexEntry(current.Key.ToString(), current.Value.ToString()));
			}
			return entries;
		}

		public string Index { get; set; }
		public string Value { get; set; }
	}
}
