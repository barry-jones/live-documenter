
namespace TheBoxSoftware.Documentation
{
    using System.Collections.Generic;
    using TheBoxSoftware.Reflection;
    using TheBoxSoftware.Reflection.Comments;

    /// <file name='code-documentation\entry.xml' path='docs/namespacefirstdocumentmapper/member[@name="class"]/*' />
	internal class NamespaceFirstDocumentMapper : DocumentMapper
    {
        /// <file name='code-documentation\entry.xml' path='docs/namespacefirstdocumentmapper/member[@name="ctor1"]/*' />
        public NamespaceFirstDocumentMapper(List<DocumentedAssembly> assemblies, bool useObservableCollection, EntryCreator creator)
            : base(assemblies, useObservableCollection, creator)
        {
        }

        protected override Entry GenerateDocumentForAssembly(DocumentMap map, DocumentedAssembly current, ref int fileCounter)
        {
            AssemblyDef assembly = AssemblyDef.Create(current.FileName);
            current.LoadedAssembly = assembly;

            XmlCodeCommentFile xmlComments = null;
            bool fileExists = System.IO.File.Exists(current.XmlFileName);
            if (fileExists)
            {
                xmlComments = new XmlCodeCommentFile(current.XmlFileName).GetReusableFile();
            }
            else
            {
                xmlComments = new XmlCodeCommentFile();
            }

            Entry assemblyEntry = this.EntryCreator.Create(assembly, System.IO.Path.GetFileName(current.FileName), xmlComments);
            current.UniqueId = assembly.UniqueId = fileCounter++;
            assemblyEntry.Key = assembly.GetGloballyUniqueId();
            assemblyEntry.IsSearchable = false;
            Entry namespaceEntry = null;

            // Add the namespaces to the document map
            Dictionary<string, List<TypeDef>> typesInNamespaces = assembly.GetTypesInNamespaces();
            foreach (KeyValuePair<string, List<TypeDef>> currentNamespace in typesInNamespaces)
            {
                if (string.IsNullOrEmpty(currentNamespace.Key) || currentNamespace.Value.Count == 0)
                {
                    continue;
                }
                string namespaceSubKey = this.BuildSubkey(currentNamespace);

                namespaceEntry = Find(map, namespaceSubKey);
                if (namespaceEntry == null)
                {
                    namespaceEntry = this.EntryCreator.Create(currentNamespace, currentNamespace.Key, xmlComments);
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
                    namespaceEntry.Children.Sort();
                    // we still need to add here otherwise we get duplicate namespaces.
                    assemblyEntry.Children.Add(namespaceEntry);
                    if (!map.Contains(namespaceEntry))
                    {
                        map.Add(namespaceEntry);
                    }
                    else
                    {
                        // update the type list is the contianing namespace
                        KeyValuePair<string, List<TypeDef>> original = (KeyValuePair<string, List<TypeDef>>)namespaceEntry.Item;
                        original.Value.AddRange(currentNamespace.Value);
                    }
                }
            }

            map.Sort();

            // we are not interested in assemblies being used here so make them childless
            return this.EntryCreator.Create(null, string.Empty, null);
        }
    }
}