using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Documentation {
	using TheBoxSoftware.Reflection;

	public class DocumentSettings {
		// TODO: use for both export and live, allow live to be used as export (checkbox on settings)
		// TODO: implement other things such as inherited members, inherited documentation settings ala sandcastle

		public DocumentSettings() {
			this.VisibilityFilters = new List<Visibility>();
		}

		/// <summary>
		/// A flag describing which library members are exported.
		/// </summary>
		/// <value>The visibility.</value>
		public List<Visibility> VisibilityFilters { get; set; }
	}
}
