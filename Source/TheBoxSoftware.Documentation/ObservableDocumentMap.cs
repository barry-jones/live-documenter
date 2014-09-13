using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Documentation {
	/// <summary>
	/// A DocumentMap that informs observers that entries have been modified, added, changed etc.
	/// </summary>
	public sealed class ObservableDocumentMap : DocumentMap {
		/// <summary>
		/// Initialises a new instance of the ObservableDocumentMap class.
		/// </summary>
		public ObservableDocumentMap()
			: base(new ObservableCollection<Entry>()) {
		}
	}
}
