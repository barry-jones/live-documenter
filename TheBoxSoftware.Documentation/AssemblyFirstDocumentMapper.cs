using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Documentation {
	using TheBoxSoftware.Reflection;
	using TheBoxSoftware.Reflection.Comments;

	/// <summary>
	/// A <see cref="DocumentMapper"/> that produces document maps that start at
	/// the assembly level.
	/// </summary>
	/// <remarks>
	/// An AssemblYFirstDocumentMapper produces a <see cref="DocumentMap"/> which starts at the
	/// assembly/library level e.g:
	/// <pre>
	/// assembly.dll
	///   Namespace.First
	///     FirstType
	///       Fields
	///       ..
	///     SecondType
	///       Methods
	///       ..
	///   Namespace.Second
	/// anotherassembly.dll
	///   Namespace.Third
	///     FirstType
	///       Fields
	///       ..
	///     SecondType
	///       Methods
	///       ..
	///   Namespace.Fourth
	/// </pre>
	/// </remarks>
	public class AssemblyFirstDocumentMapper : DocumentMapper {
		/// <summary>
		/// Initialises a new instance of the AssemblyFirstDocumentMapper.
		/// </summary>
		/// <param name="assemblies">The assemblies to be mapped.</param>
		/// <param name="settings">The settings to use while producing the map.</param>
		/// <param name="useObservableCollection">Is an observable collection required.</param>
		public AssemblyFirstDocumentMapper(List<DocumentedAssembly> assemblies, DocumentSettings settings, bool useObservableCollection, EntryCreator creator)
			: base(assemblies, settings, useObservableCollection, creator) {
		}

		public override Entry GenerateDocumentForAssembly(DocumentedAssembly current, ref int fileCounter) {
			AssemblyDef assembly = AssemblyDef.Create(current.FileName);
			current.LoadedAssembly = assembly;

			XmlCodeCommentFile xmlComments = null;
			bool fileExists = System.IO.File.Exists(current.XmlFileName);
			if (fileExists) {
				xmlComments = new XmlCodeCommentFile(current.XmlFileName);
			}
			else {
				xmlComments = new XmlCodeCommentFile();
			}

			Entry assemblyEntry = this.EntryCreator.Create(assembly, System.IO.Path.GetFileName(current.FileName), xmlComments);
			assembly.UniqueId = fileCounter++;
			assemblyEntry.Key = this.GetUniqueKey(assembly);
			assemblyEntry.IsSearchable = false;
			assemblyEntry.HasXmlComments = fileExists;

			// Add the namespaces to the document map
			foreach (KeyValuePair<string, List<TypeDef>> currentNamespace in assembly.GetTypesInNamespaces()) {
				if (string.IsNullOrEmpty(currentNamespace.Key)) {
					continue;
				}
				Entry namespaceEntry = assemblyEntry.FindByKey(assemblyEntry.Key, currentNamespace.Key, false);
				//namespaceEntry.Item = currentNamespace;
				if (namespaceEntry == null) {
					namespaceEntry = this.EntryCreator.Create(currentNamespace, currentNamespace.Key, xmlComments, assemblyEntry);
					namespaceEntry.Key = assemblyEntry.Key;
					namespaceEntry.SubKey = currentNamespace.Key;
					namespaceEntry.IsSearchable = false;
					namespaceEntry.FullName = currentNamespace.Key;
				}

				// Add the types from that namespace to its map
				foreach (TypeDef currentType in currentNamespace.Value) {
					if (currentType.Name.StartsWith("<")) {
						continue;
					}
					Entry typeEntry = this.EntryCreator.Create(currentType, currentType.GetDisplayName(false), xmlComments, namespaceEntry);
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
					assemblyEntry.Children.Add(namespaceEntry);
					namespaceEntry.Children.Sort();
				}
			}

			assemblyEntry.Children.Sort();

			return assemblyEntry;
		}
	}
}
