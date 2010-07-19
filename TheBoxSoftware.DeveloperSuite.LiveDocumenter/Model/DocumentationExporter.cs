using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Model {
	/// <summary>
	/// Entry and manager class for exporting the documentation from the Live
	/// Documentor to xml and then uses a selected XSLT to produce other
	/// documentation sets.
	/// </summary>
	/// <remarks>
	/// The exporter utilises the <see cref="LiveDocumentorFile.Singleton"/> instance
	/// to get all the required XAML <see cref="Page"/>s. This will cause the application
	/// to generate all of the pages in the documentation. Becuase of that the class
	/// exposes three events, begin, processed and end which inform the rest of the 
	/// application of its state. This will provide enough information for progress bars
	/// etc.
	/// </remarks>
	internal sealed class DocumentationExporter {
		private static string TEMP_DOCUMENATION_DIRECTORY = @"temp";
		private static string TEMP_LIVE_DIRECTORY = @"temp\live";
		private static string LIVEXML_XSLT = @"ApplicationData\xamltolivexml.xslt";
		private LiveDocumentorFile currentFile;
		private System.Text.RegularExpressions.Regex illegalFileCharacters;

		/// <summary>
		/// Initialises a new instance of the DocumentationExporter
		/// </summary>
		public DocumentationExporter() {
			this.currentFile = LiveDocumentorFile.Singleton;
			string regex = string.Format("{0}{1}",
					 new string(Path.GetInvalidFileNameChars()),
					 new string(Path.GetInvalidPathChars()));
			illegalFileCharacters = new System.Text.RegularExpressions.Regex(
				string.Format("[{0}]", System.Text.RegularExpressions.Regex.Escape(regex))
				);
		}

		/// <summary>
		/// Exports the full contained documentation.
		/// </summary>
		public void Export() {
			this.PrepareDirectory(TEMP_DOCUMENATION_DIRECTORY);
			foreach (Entry current in this.currentFile.LiveDocument.DocumentMap) {
				this.RecursiveEntryExport(current);
			}

			// Now we need to apply the xslt to each of the files to generate the 
			// livexml documents
			this.PrepareDirectory(TEMP_LIVE_DIRECTORY);
			XslCompiledTransform transform = new XslCompiledTransform(true);
			transform.Load(LIVEXML_XSLT);
			foreach (string current in Directory.GetFiles(TEMP_DOCUMENATION_DIRECTORY)) {
				transform.Transform(current, TEMP_LIVE_DIRECTORY + @"\" + Path.GetFileName(current));
			}
			
			// Finally perform the user selected output xslt
			this.PrepareDirectory(@"temp\output");
			transform.Load(@"ApplicationData\livexmltohtml.xslt");
			foreach (string current in Directory.GetFiles(TEMP_LIVE_DIRECTORY)) {
				transform.Transform(current, @"temp\output\" + Path.GetFileNameWithoutExtension(current) + ".htm");
			}
		}

		/// <summary>
		/// A method that recursively, through the documentation tree, exports all of the
		/// found pages for each of the entries in the documentation.
		/// </summary>
		/// <param name="currentEntry">The current entry to export</param>
		private void RecursiveEntryExport(Entry currentEntry) {
			this.SaveXaml(currentEntry);
			for (int i = 0; i < currentEntry.Children.Count; i++) {
				this.RecursiveEntryExport(currentEntry.Children[i]);
			}
		}

		private void SaveXaml(Entry current) {
			System.Xml.XmlDocument xdoc = new System.Xml.XmlDocument();
			current.Page.Generate();	// Make sure each of the pages has content
			xdoc.LoadXml(System.Windows.Markup.XamlWriter.Save(current.Page));
			xdoc.Save(string.Format("{0}/{1}_{2}.xml", TEMP_DOCUMENATION_DIRECTORY, current.Key, illegalFileCharacters.Replace(current.SubKey ?? string.Empty, "_")));
		}

		/// <summary>
		/// Ensures there is an empty temp directory to proccess the documentation in.
		/// </summary>
		private void PrepareDirectory(string directory) {
			if (!Directory.Exists(directory)) {
				Directory.CreateDirectory(directory);
			}
			else {
				Directory.Delete(directory, true);
				Directory.CreateDirectory(directory);
			}
		}
	}
}
