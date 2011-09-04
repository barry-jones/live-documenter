using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheBoxSoftware.Reflection;

namespace TheBoxSoftware.Documentation.Exporting {
	/// <summary>
	/// Settings which govern where, when, how and all other information regarding
	/// the export of documentation.
	/// </summary>
	public sealed class ExportSettings {
		/// <summary>
		/// Initialises a new instance of the ExportSettings class.
		/// </summary>
		public ExportSettings() {
		}

		/// <summary>
		/// The export overridden settings for exporting documentation
		/// </summary>
		public DocumentSettings Settings { get; set; }

		/// <summary>
		/// The user configured title for the documentation
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// The directory where all final output should be copied after export.
		/// </summary>
		public string PublishDirectory { get; set; }
	}
}
