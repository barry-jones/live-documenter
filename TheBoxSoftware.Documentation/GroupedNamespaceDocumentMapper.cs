using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Documentation {
	using TheBoxSoftware.Reflection;
	using TheBoxSoftware.Reflection.Comments;

	public class GroupedNamespaceDocumentMapper : DocumentMapper {
		/// <summary>
		/// Initialises a new instance of the NamespaceFirstDocumentMapper.
		/// </summary>
		/// <param name="assemblies">The assemblies being documented.</param>
		/// <param name="settings">Documentation settings.</param>
		/// <param name="useObservableCollection">Indicates if an observable collection should be used instead of a normal one.</param>
		/// <param name="creator">The factory class for creating new <see cref="Entry"/> instances.</param>
		public GroupedNamespaceDocumentMapper(List<DocumentedAssembly> assemblies, bool useObservableCollection, EntryCreator creator)
			: base(assemblies, useObservableCollection, creator) {
		}

		public override void GenerateMap() {
			this.EntryCreator.Created = 0;
			this.DocumentMap = this.UseObservableCollection ? new ObservableDocumentMap() : new DocumentMap();
			int fileCounter = 1;
			List<NamespacePart> namespaces = new List<NamespacePart>();

			// For each of the documentedfiles generate the document map and add
			// it to the parent node of the document map
			for (int i = 0; i < this.CurrentFiles.Count; i++) {
				if (!this.CurrentFiles[i].IsCompiled)
					continue;
				this.GenerateDocumentForAssembly(
					this.CurrentFiles[i], ref fileCounter
					);
			}
			this.DocumentMap.Sort();

			List<Entry> namespaceContainers = new List<Entry>();
			foreach(Entry current in this.DocumentMap) {
				bool isChild = false;
				foreach(Entry namespaceContainer in namespaceContainers) {
					if(current.Name.Contains(namespaceContainer.Name)) {
						current.Parent = namespaceContainer;
						namespaceContainer.Children.Add(current);
						isChild = true;
					}
				}
				if(!isChild) {
					Entry namespaceContainer = this.EntryCreator.Create(
						EntryTypes.NamespaceContainer, current.Name, null
						);
					current.Parent = namespaceContainer;
					namespaceContainer.Children.Add(current);
					namespaceContainers.Add(namespaceContainer);
				}
			}

			this.DocumentMap.Clear();
			for(int i = 0; i < namespaceContainers.Count; i++) {
				namespaceContainers[i].Name += " Namespaces";
				this.DocumentMap.Add(namespaceContainers[i]);
			}
			this.DocumentMap.NumberOfEntries = this.EntryCreator.Created;
		}

		public override Entry GenerateDocumentForAssembly(DocumentedAssembly current, ref int fileCounter) {
			AssemblyDef assembly = AssemblyDef.Create(current.FileName);
			current.LoadedAssembly = assembly;

			XmlCodeCommentFile xmlComments = null;
			bool fileExists = System.IO.File.Exists(current.XmlFileName);
			if (fileExists) {
				xmlComments = new XmlCodeCommentFile(current.XmlFileName).GetReusableFile();
			}
			else {
				xmlComments = new XmlCodeCommentFile();
			}

			Entry assemblyEntry = this.EntryCreator.Create(assembly, System.IO.Path.GetFileName(current.FileName), xmlComments);
			current.UniqueId = assembly.UniqueId = fileCounter++;
			assemblyEntry.Key = assembly.GetGloballyUniqueId();
			assemblyEntry.IsSearchable = false;
			assemblyEntry.HasXmlComments = fileExists;
			Entry namespaceEntry = null;

			// Add the namespaces to the document map
			foreach (KeyValuePair<string, List<TypeDef>> currentNamespace in assembly.GetTypesInNamespaces()) {
				if (string.IsNullOrEmpty(currentNamespace.Key) || currentNamespace.Value.Count == 0) {
					continue;
				}
				string namespaceSubKey = this.BuildSubkey(currentNamespace);

				namespaceEntry = this.Find(namespaceSubKey);
				if (namespaceEntry == null) {
					namespaceEntry = this.EntryCreator.Create(currentNamespace, currentNamespace.Key, xmlComments);
					namespaceEntry.Key = assemblyEntry.Key;
					namespaceEntry.SubKey = namespaceSubKey;
					namespaceEntry.IsSearchable = false;
				}

				// Add the types from that namespace to its map
				foreach (TypeDef currentType in currentNamespace.Value) {
					if (currentType.Name.StartsWith("<")) {
						continue;
					}
					PreEntryAddedEventArgs e = new PreEntryAddedEventArgs(currentType);
					this.OnPreEntryAdded(e);
					if (!e.Filter) {
						Entry typeEntry = this.EntryCreator.Create(currentType, currentType.GetDisplayName(false), xmlComments, namespaceEntry);
						typeEntry.Key = currentType.GetGloballyUniqueId();
						typeEntry.IsSearchable = true;

						// For some elements we will not want to load the child objects
						// this is currently for System.Enum derived values.
						if (
							currentType.InheritsFrom != null && currentType.InheritsFrom.GetFullyQualifiedName() == "System.Enum" ||
							currentType.IsDelegate) {
							// Ignore children
						}
						else {
							this.GenerateTypeMap(currentType, typeEntry, xmlComments);
							typeEntry.Children.Sort();
						}

						namespaceEntry.Children.Add(typeEntry);
					}
				}
				if (namespaceEntry.Children.Count > 0) {
					namespaceEntry.Children.Sort();
					// we still need to add here otherwise we get duplicate namespaces.
					assemblyEntry.Children.Add(namespaceEntry);
					if(!this.DocumentMap.Contains(namespaceEntry)) {
						this.DocumentMap.Add(namespaceEntry);
					}
					else {
						// update the type list is the contianing namespace
						KeyValuePair<string, List<TypeDef>> original = (KeyValuePair<string, List<TypeDef>>)namespaceEntry.Item;
						original.Value.AddRange(currentNamespace.Value);
					}
				}
			}

			// we are not interested in assemblies being used here so make them childless
			return this.EntryCreator.Create(null, string.Empty, null);
		}

		/// <summary>
		/// Searches the top level elements for the specified <paramref name="name"/>.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>The Entry if found else null.</returns>
		private Entry Find(string name) {
			Entry found = null;
			for (int i = 0; i < this.DocumentMap.Count; i++) {
				found = this.DocumentMap[i].Name == name ? this.DocumentMap[i] : null;
				if (found != null) break;
			}
			return found;
		}

		#region Internals
		private class NamespacePart {
			public NamespacePart(string name) {
				this.Part = name;
				this.Children = new List<NamespacePart>();
			}

			public int Count { get; set; }
			public string Part { get; set; }
			public List<NamespacePart> Children { get; set; }
		}
		#endregion
	}
}
