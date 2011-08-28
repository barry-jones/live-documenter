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
	internal class AssemblyFirstDocumentMapper : DocumentMapper {
		/// <summary>
		/// Initialises a new instance of the AssemblyFirstDocumentMapper.
		/// </summary>
		/// <param name="assemblies">The assemblies to be mapped.</param>
		/// <param name="settings">The settings to use while producing the map.</param>
		/// <param name="useObservableCollection">Is an observable collection required.</param>
		public AssemblyFirstDocumentMapper(List<DocumentedAssembly> assemblies, bool useObservableCollection, EntryCreator creator)
			: base(assemblies, useObservableCollection, creator) {
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
			assemblyEntry.Key = assembly.GetGloballyUniqueId();
			assemblyEntry.IsSearchable = false;
			assemblyEntry.HasXmlComments = fileExists;

			// Add the namespaces to the document map
			foreach (KeyValuePair<string, List<TypeDef>> currentNamespace in assembly.GetTypesInNamespaces()) {
				if (string.IsNullOrEmpty(currentNamespace.Key) || currentNamespace.Value.Count == 0) {
					continue;
				}
				string namespaceSubKey = this.BuildSubkey(currentNamespace);

				Entry namespaceEntry = this.FindByKey(assemblyEntry.Key, namespaceSubKey, false);
				if (namespaceEntry == null) {
					namespaceEntry = this.EntryCreator.Create(currentNamespace, currentNamespace.Key, xmlComments, assemblyEntry);
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
					assemblyEntry.Children.Add(namespaceEntry);
					namespaceEntry.Children.Sort();
				}
			}

			assemblyEntry.Children.Sort();

			return assemblyEntry;
		}
	}
}
