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
	public sealed class LiveDocument : Document {
		/// <summary>
		/// Initialises a new instance of the LiveDocument class.
		/// </summary>
		/// <param name="assemblies">The assemblies to document.</param>
		public LiveDocument(List<DocumentedAssembly> assemblies)
			: base(assemblies, Mappers.AssemblyFirst, true, new LiveDocumenterEntryCreator()) {

			DocumentSettings settings = new DocumentSettings();
			settings.VisibilityFilters.AddRange(new Visibility[] { 
			        Visibility.Private,
			        Visibility.Protected,
			        Visibility.Public,
			        Visibility.InternalProtected,
			        Visibility.Internal
			        });
			//settings.VisibilityFilters.AddRange(new Visibility[] { 
			//        Visibility.Public
			//        });
			this.Settings = settings;
		}

		/// <summary>
		/// Updates the contents of the live document for the current list of
		/// file references.
		/// </summary>
		public void Update() {
			this.Mapper.GenerateMap();
			this.Map = this.Mapper.DocumentMap;
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
			int fileCounter = LiveDocumentorFile.Singleton.LiveDocument.Map.Count;
			for (int i = 0; i < LiveDocumentorFile.Singleton.LiveDocument.Map.Count; i++) {
				Entry currentEntry = LiveDocumentorFile.Singleton.LiveDocument.Map[i];
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
				this.Map.RemoveAt(entryAtIndex);

				fileCounter = ((TheBoxSoftware.Reflection.AssemblyDef)existingEntry.Item).UniqueId;
			}
			Entry assemblyEntry = this.Mapper.GenerateDocumentForAssembly(documentedAssembly, ref fileCounter);

			// insert the newly generated entry in the same location as the old one or make it the first entry
			this.Map.Insert(entryAtIndex == -1 ? 0 : entryAtIndex, assemblyEntry);
		}

		/// <summary>
		/// Returns the assembly currently selected by the user.
		/// </summary>
		public DocumentedAssembly SelectedAssembly {
			get {
				DocumentedAssembly selected = null;
				if (this.Map != null && this.Assemblies != null) {
					foreach (LiveDocumenterEntry entry in this.Map) {
						if (entry.IsSelected) {
							long assemblyId = ReflectedMember.GetAssemblyId(entry.Key);
							foreach (DocumentedAssembly assembly in this.Assemblies) {
								if (assembly.UniqueId == assemblyId) {
									selected = assembly;
									break;
								}
							}
						}
					}
				}
				return selected;
			}
		}
	}
}