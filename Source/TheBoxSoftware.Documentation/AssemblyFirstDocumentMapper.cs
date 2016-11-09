
namespace TheBoxSoftware.Documentation
{
    using System.Collections.Generic;
    using Reflection;
    using Reflection.Comments;

    /// <file name='code-documentation\entry.xml' path='docs/assemblyfirstdocumentmapper/member[@name="class"]/*' />
	internal class AssemblyFirstDocumentMapper : DocumentMapper
    {
        /// <file name='code-documentation\entry.xml' path='docs/assemblyfirstdocumentmapper/member[@name="ctor1"]/*' />
		public AssemblyFirstDocumentMapper(List<DocumentedAssembly> assemblies, bool useObservableCollection, EntryCreator creator)
			: base(assemblies, useObservableCollection, creator)
        {
		}

		protected override Entry GenerateDocumentForAssembly(DocumentMap map, DocumentedAssembly current, ref int fileCounter)
        {
			AssemblyDef assembly = AssemblyDef.Create(current.FileName);
			current.LoadedAssembly = assembly;

			XmlCommentFile commentFile = new XmlCommentFile(current.XmlFileName, new FileSystem());
            commentFile.Load();
            ICommentSource xmlComments = commentFile; // not nice having to call load then cast we wil have to fix this


			Entry assemblyEntry = this.EntryCreator.Create(assembly, System.IO.Path.GetFileName(current.FileName), xmlComments);
			current.UniqueId = assembly.UniqueId = fileCounter++;
			assemblyEntry.Key = assembly.GetGloballyUniqueId();
			assemblyEntry.IsSearchable = false;

            // Add the namespaces to the document map
            Dictionary<string, List<TypeDef>> typesInNamespaces = assembly.GetTypesInNamespaces();
			foreach (KeyValuePair<string, List<TypeDef>> currentNamespace in typesInNamespaces)
            {
				if (string.IsNullOrEmpty(currentNamespace.Key) || currentNamespace.Value.Count == 0)
                {
					continue;
				}
				string namespaceSubKey = this.BuildSubkey(currentNamespace);

				Entry namespaceEntry = this.FindByKey(map, assemblyEntry.Key, namespaceSubKey, false);
				if (namespaceEntry == null)
                {
					namespaceEntry = this.EntryCreator.Create(currentNamespace, currentNamespace.Key, xmlComments, assemblyEntry);
					namespaceEntry.Key = assemblyEntry.Key;
					namespaceEntry.SubKey = namespaceSubKey;
					namespaceEntry.IsSearchable = false;
				}

				// Add the types from that namespace to its map
				foreach (TypeDef currentType in currentNamespace.Value)
                {
					if (currentType.Name.StartsWith("<"))
                    {
						continue;
					}
					PreEntryAddedEventArgs e = new PreEntryAddedEventArgs(currentType);
					this.OnPreEntryAdded(e);
					if (!e.Filter)
                    {
						Entry typeEntry = this.EntryCreator.Create(currentType, currentType.GetDisplayName(false), xmlComments, namespaceEntry);
						typeEntry.Key = currentType.GetGloballyUniqueId();
						typeEntry.IsSearchable = true;

						// For some elements we will not want to load the child objects
						// this is currently for System.Enum derived values.
						if (
								currentType.InheritsFrom != null && currentType.InheritsFrom.GetFullyQualifiedName() == "System.Enum" ||
								currentType.IsDelegate)
                        {
							// Ignore children
						}
						else
                        {
							this.GenerateTypeMap(currentType, typeEntry, xmlComments);
							typeEntry.Children.Sort();
						}

						namespaceEntry.Children.Add(typeEntry);
					}
				}
				if (namespaceEntry.Children.Count > 0)
                {
					assemblyEntry.Children.Add(namespaceEntry);
					namespaceEntry.Children.Sort();
				}
			}

			assemblyEntry.Children.Sort();

			return assemblyEntry;
		}
	}
}
