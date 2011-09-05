using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using External = TheBoxSoftware.Documentation;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter {
	public sealed class Document {
		private External.Document baseDocument;

		#region Constructors
		/// <summary>
		/// Initialises a new Document
		/// </summary>
		public Document() { }

		/// <summary>
		/// Initialises a new Document
		/// </summary>
		/// <param name="file">The</param>
		public Document(string file) {
			this.Filename = file;
		}
		#endregion

		#region Methods
		public void Refresh() {
			External.InputFileReader.Read(this.Filename, "Debug");
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the name of the file being documented.
		/// </summary>
		public string Filename { get; set; }
		#endregion
	}
}
