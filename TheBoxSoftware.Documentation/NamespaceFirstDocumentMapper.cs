using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Documentation {
	using TheBoxSoftware.Reflection;
	using TheBoxSoftware.Reflection.Comments;

	public class NamespaceFirstDocumentMapper : DocumentMapper {
		public NamespaceFirstDocumentMapper(List<DocumentedAssembly> assemblies, DocumentSettings settings, bool useObservableCollection)
			: base(assemblies, settings, useObservableCollection) {
		}

		protected override Entry GenerateDocumentForAssembly(DocumentedAssembly current, ref int fileCounter) {
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

			Entry assemblyEntry = new Entry(assembly, System.IO.Path.GetFileName(current.FileName), xmlComments);
			assembly.UniqueId = fileCounter++;
			assemblyEntry.Key = this.GetUniqueKey(assembly);
			assemblyEntry.IsSearchable = false;
			assemblyEntry.HasXmlComments = fileExists;
			Entry namespaceEntry = null;

			// Add the namespaces to the document map
			foreach (KeyValuePair<string, List<TypeDef>> currentNamespace in assembly.GetTypesInNamespaces()) {
				if (string.IsNullOrEmpty(currentNamespace.Key)) {
					continue;
				}
				namespaceEntry = this.FindByKey(assemblyEntry.Key, currentNamespace.Key, false);
				//namespaceEntry.Item = currentNamespace;
				if (namespaceEntry == null) {
					namespaceEntry = new Entry(currentNamespace, currentNamespace.Key, xmlComments);
					namespaceEntry.Key = assemblyEntry.Key;
					namespaceEntry.SubKey = this.illegalFileCharacters.Replace(currentNamespace.Key, "_");
					namespaceEntry.IsSearchable = false;
					namespaceEntry.FullName = currentNamespace.Key;
				}

				// Add the types from that namespace to its map
				foreach (TypeDef currentType in currentNamespace.Value) {
					if (currentType.Name.StartsWith("<")) {
						continue;
					}
					Entry typeEntry = new Entry(currentType, currentType.GetDisplayName(false), xmlComments, namespaceEntry);
					typeEntry.Key = this.GetUniqueKey(assembly, currentType);
					typeEntry.IsSearchable = true;
					typeEntry.FullName = currentType.GetFullyQualifiedName();

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

					if (this.PreEntryAdded(typeEntry)) {
						namespaceEntry.Children.Add(typeEntry);
					}
					else {
						continue;
					}
				}
				if (namespaceEntry.Children.Count > 0) {
					namespaceEntry.Children.Sort();
					this.DocumentMap[0].Children.Add(namespaceEntry);
				}
			}

			// we are not interested in assemblies being used here so make them childless
			return new Entry(null, string.Empty, null);
		}
	}
}