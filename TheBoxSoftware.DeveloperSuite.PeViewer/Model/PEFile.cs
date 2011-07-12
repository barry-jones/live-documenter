using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Model {
	using TheBoxSoftware.Reflection.Core;
	using TheBoxSoftware.Reflection.Core.PE;
	using TheBoxSoftware.Reflection.Core.COFF;

	/// <summary>
	/// This is a wrapper class that parses the business item PeCoffFile
	/// to a Model. Which can be utilised in the PE Viewer
	/// </summary>
	internal class PEFile {
		/// <summary>
		/// Initialises a new instance of the PEFile wrapper class.
		/// </summary>
		/// <param name="peCoffFile">The file instance to parse and wrap.</param>
		public PEFile(PeCoffFile peCoffFile) {
			this.Entries = new List<Entry>();
			this.Initialise(peCoffFile);
		}

		/// <summary>
		/// Initialises the PEFile.
		/// </summary>
		private void Initialise(PeCoffFile peCoffFile) {
			this.Entries.Add(new Entry("File Header"));

			// Initialise the display of the PE Header entries
			Entry peHeader = new Entry("PE Header");
			this.Entries.Add(peHeader);
			string[] names = Enum.GetNames(typeof(TheBoxSoftware.Reflection.Core.PE.DataDirectories));
			foreach (string name in names) {
				peHeader.Children.Add(Entry.Create(name));
			}

			// Initialise the section entries
			Entry sections = new Entry("Sections");
			this.Entries.Add(sections);
			foreach (TheBoxSoftware.Reflection.Core.PE.SectionHeader header in peCoffFile.SectionHeaders) {
				sections.Children.Add(Entry.Create(header.Name));
			}

			// Initialise the view of the different directories
			Entry directories = new Entry("Directories");
			this.Entries.Add(directories);
			foreach (KeyValuePair<DataDirectories, Directory> directory in peCoffFile.Directories) {
				directories.Children.Add(Entry.Create(directory.Value));
			}
		}

		private void BuildNodesForDirectory(DataDirectories type, Directory directory, Entry parent) {
			switch (type) {
				case DataDirectories.CommonLanguageRuntimeHeader:
					CLRDirectory clrDirectory = directory as CLRDirectory;
					MetadataDirectory metadataDirectory = clrDirectory.Metadata;

					// Metadata
					Entry metadataEntry = new Entry("Metadata");
					parent.Children.Add(metadataEntry);

					// Streams
					Entry streamsEntry = new Entry("Streams");
					foreach (KeyValuePair<Streams, Stream> currentStream in metadataDirectory.Streams) {
						Entry streamEntry = new Entry(currentStream.Value.Name);
						streamsEntry.Children.Add(streamEntry);
					}
					parent.Children.Add(streamsEntry);
					break;
			}
		}

		#region Properties
		/// <summary>
		/// A collection of entries for this file
		/// </summary>
		public List<Entry> Entries { get; set; }
		#endregion
	}
}
