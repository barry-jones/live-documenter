using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Documentation {
	using TheBoxSoftware.Reflection;

	/// <summary>
	/// Stores the settings which describe how the documentation should be produced and viewed.
	/// </summary>
	public sealed class DocumentSettings {
		// TODO: use for both export and live, allow live to be used as export (checkbox on settings)
		// TODO: implement other things such as inherited members, inherited documentation settings ala sandcastle

		/// <summary>
		/// Initialises a new instance of the DocumentSettings class.
		/// </summary>
		public DocumentSettings() {
			this.VisibilityFilters = new List<Visibility>();
		}

		/// <summary>
		/// A list of Visibility flags on types and members which should be visible.
		/// </summary>
		public List<Visibility> VisibilityFilters { get; set; }
	}
}
