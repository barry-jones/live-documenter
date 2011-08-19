using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter {
	using TheBoxSoftware.Reflection;
	using TheBoxSoftware.Reflection.Core;
	using TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages;
	using TheBoxSoftware.Reflection.Comments;
	using TheBoxSoftware.Documentation;

	/// <summary>
	/// Represents a live document, which is a collection of pages which display
	/// code information and diagrams etc. For all of the loaded files for the
	/// current LiveDocumentFile projects.
	/// </summary>
	public sealed class LiveDocument {
		private DocumentMapper documentMapper;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public LiveDocument() { }

		/// <summary>
		/// Initialises a new LiveDocument
		/// </summary>
		/// <param name="files">The files to be managed by this LiveDocument.</param>
		public LiveDocument(List<DocumentedAssembly> files) {
			this.DocumentedFiles = files;
			this.Update();
		}

		/// <summary>
		/// Updates the contents of the live document for the current list of
		/// file references.
		/// </summary>
		public void Update() {
			if (this.documentMapper == null) {
				DocumentSettings settings = new DocumentSettings();
				settings.VisibilityFilters.AddRange(new Visibility[] { 
					Visibility.Private,
					Visibility.Protected,
					Visibility.Public,
					Visibility.InternalProtected,
					Visibility.Internal
					});

				this.documentMapper = DocumentMapper.Create(this.DocumentedFiles, Mappers.AssemblyFirst, settings, true, new LiveDocumenterEntryCreator());
			}
			this.documentMapper.GenerateMap();
			this.DocumentMap = this.documentMapper.DocumentMap;
		}

		/// <summary>
		/// Refreshes the specified assemblies documentation in the UI.
		/// </summary>
		/// <param name="documentedAssembly">The assembly whose documentation needs updating.</param>
		/// <remarks>
		/// <para>
		/// When an assembly is refreshed the <see cref="DocumentMapper"/> is used to regenerate the
		/// document map for that assembly.
		/// </para>
		/// <para>
		/// If the assembly was not compiled before the refresh, is has its document map generated
		/// and is then inserted in to the current <see cref="DocumentMap"/>.
		/// </para>
		/// </remarks>
		public void RefreshAssembly(DocumentedAssembly documentedAssembly) {
			// The assembly has been modified, find the existing node
			// and generate the new one
			Entry existingEntry = null;
			int entryAtIndex = -1;
			int fileCounter = LiveDocumentorFile.Singleton.LiveDocument.DocumentMap.Count;
			for (int i = 0; i < LiveDocumentorFile.Singleton.LiveDocument.DocumentMap.Count; i++) {
				Entry currentEntry = LiveDocumentorFile.Singleton.LiveDocument.DocumentMap[i];
				if (currentEntry.Name == System.IO.Path.GetFileName(documentedAssembly.FileName)) {
					existingEntry = currentEntry;
					entryAtIndex = i;
					break;
				}
			}

			// only remove and reset if the assembly already exists in the document map
			if (entryAtIndex != -1) {
				// Remove the old entry, we need to do this now so any searches performed in the
				// generate method do not return values from here.
				this.DocumentMap.RemoveAt(entryAtIndex);

				fileCounter = ((TheBoxSoftware.Reflection.AssemblyDef)existingEntry.Item).UniqueId;
			}
			Entry assemblyEntry = this.documentMapper.GenerateDocumentForAssembly(documentedAssembly, ref fileCounter);

			// insert the newly generated entry in the same location as the old one or make it the first entry
			this.DocumentMap.Insert(entryAtIndex == -1 ? 0 : entryAtIndex, assemblyEntry);
		}

		/// <summary>
		/// Searches the contents of the document map and returns any elements which
		/// contain the text provided in their names.
		/// </summary>
		/// <param name="searchText">The text to search for</param>
		/// <returns>The list of found entries or a single entry describing no results if not found</returns>
		internal List<Entry> Search(string searchText) {
			List<Entry> results = new List<Entry>();
			if (this.DocumentMap != null) {
				for (int i = 0; i < this.DocumentMap.Count; i++) {
					results.AddRange(this.DocumentMap[i].Search(searchText));
				}
			}
			return results;
		}

		/// <summary>
		/// The files that are used to build the live document, this is the
		/// project, solution and library references.
		/// </summary>
		public List<DocumentedAssembly> DocumentedFiles { get; set; }

		/// <summary>
		/// The base document map entry for this live document.
		/// </summary>
		internal DocumentMap DocumentMap { get; set; }
	}
}