using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Documentation.Exporting {
	using TheBoxSoftware.Reflection;

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
		/// 
		/// </summary>
		public DocumentSettings Settings { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string TempDirectory { get; set; }

		/// <summary>
		/// The full file path to the directory where all of the final files will
		/// be output to. 
		/// </summary>
		/// <value>The output directory.</value>
		public string OutputDirectory { get; set; }
	}
}
