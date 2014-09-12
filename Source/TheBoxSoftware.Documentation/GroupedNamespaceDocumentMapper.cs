using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Documentation {
	using TheBoxSoftware.Reflection;
	using TheBoxSoftware.Reflection.Comments;

	/// <summary>
	/// <para>A DocumentMapper that generates a map starting from namespaces. Where those namespaces
	/// have been grouped together to simplify the starting point.</para
	/// <para>See the MSDN library for an example of what this produces.</para>
	/// </summary>
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

			bool dontGroupNamespaces = true;
			List<string> counter = new List<string>();
			List<Entry> namespaceContainers = new List<Entry>();

			if (this.DocumentMap.Count > 10) {
				// calculate the best level to create groups from or if there is no best place
				dontGroupNamespaces = false;
				float parentPercentage = 0, currentPercentage = 0;
				int currentLevel = 0;
				do {
					counter.Clear();

					for (int dmI = 0; dmI < this.DocumentMap.Count; dmI++) {
						string[] parts = this.DocumentMap[dmI].Name.Split('.');
						string currentNamespace = parts.Length > currentLevel ? string.Join(".", parts, 0, currentLevel + 1) : string.Join(".", parts);

						if (!counter.Contains(currentNamespace)) {
							counter.Add(currentNamespace);
						}
					}

					currentPercentage = ((float)counter.Count) / ((float)this.DocumentMap.Count);
					if (parentPercentage < 0.1 && currentPercentage > 0.65 && currentLevel > 0) {
						dontGroupNamespaces = true;
						break;
					}

					currentLevel++;
					parentPercentage = ((float)counter.Count) / ((float)this.DocumentMap.Count);
				} while (((float)counter.Count) / ((float)this.DocumentMap.Count) < 0.1306);
			}

			if (!dontGroupNamespaces) {
				// create all the top level groupings
				int id = 0;
				for (int cI = 0; cI < counter.Count; cI++) {
					Entry namespaceContainer = this.EntryCreator.Create(
						EntryTypes.NamespaceContainer, counter[cI], null
						);
					namespaceContainer.Key = id++;
					namespaceContainer.SubKey = counter[cI] + "Namespaces";
					namespaceContainers.Add(namespaceContainer);
				}

				// add all the namespaces to the groupings
				for (int namespaceI = 0; namespaceI < this.DocumentMap.Count; namespaceI++) {
					for (int containersI = namespaceContainers.Count; containersI > 0; containersI--) {
						if (this.DocumentMap[namespaceI].Name.Contains(namespaceContainers[containersI - 1].Name)) {
							this.DocumentMap[namespaceI].Parent = namespaceContainers[containersI - 1];
							namespaceContainers[containersI - 1].Children.Add(this.DocumentMap[namespaceI]);
							break;
						}
					}
				}
			}

			if (namespaceContainers.Count > 1) {
				this.DocumentMap.Clear();
				for (int i = 0; i < namespaceContainers.Count; i++) {
					namespaceContainers[i].Name += " Namespaces";
					this.DocumentMap.Add(namespaceContainers[i]);
				}
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
	}
}
