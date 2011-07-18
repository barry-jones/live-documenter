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
			this.DocumentSettings = new DocumentSettings();
		}

		/// <summary>
		/// The settings for the produced/exported document.
		/// </summary>
		public DocumentSettings DocumentSettings { get; set; }

		/// <summary>
		/// The full filepath to the XSLT which will be executed on all of
		/// the XML files exported as part of the export process.
		/// </summary>
		/// <value>The XSLT.</value>
		public string Xslt { get; set; }

		public string TempDirectory { get; set; }

		/// <summary>
		/// The full file path to the directory where all of the final files will
		/// be output to. 
		/// </summary>
		/// <value>The output directory.</value>
		public string OutputDirectory { get; set; }
	}
}
