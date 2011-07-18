using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Documentation {
	public sealed class ObservableDocumentMap : DocumentMap {
		public ObservableDocumentMap()
			: base(new ObservableCollection<Entry>()) {
		}
	}
}
