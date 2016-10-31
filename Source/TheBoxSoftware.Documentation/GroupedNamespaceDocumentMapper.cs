
// This currently loads the assembly defs as required and then interates over them, in 
// reality though it should be possible to provide the class with already loaded AssemblyDef
// files - which should open a door to being able to pass specific values through and to 
// create explicit tests.

namespace TheBoxSoftware.Documentation
{
    using System.Collections.Generic;
    using Reflection;
    using Reflection.Comments;
    using TheBoxSoftware;

    /// <summary>
    /// <para>A DocumentMapper that generates a map starting from namespaces. Where those namespaces
    /// have been grouped together to simplify the starting point.</para>
    /// <para>See the MSDN library for an example of what this produces.</para>
    /// </summary>
    public class GroupedNamespaceDocumentMapper : DocumentMapper
    {
        private readonly IFileSystem _fileSystem;

        /// <summary>
        /// Initialises a new instance of the NamespaceFirstDocumentMapper.
        /// </summary>
        /// <param name="assemblies">The assemblies being documented.</param>
        /// <param name="useObservableCollection">Indicates if an observable collection should be used instead of a normal one.</param>
        /// <param name="creator">The factory class for creating new <see cref="Entry"/> instances.</param>
        public GroupedNamespaceDocumentMapper
            (
            List<DocumentedAssembly> assemblies, 
            bool useObservableCollection, 
            EntryCreator creator
            )
            : base(assemblies, useObservableCollection, creator)
        {
            _fileSystem = FileSystem.Singleton;
        }

        public GroupedNamespaceDocumentMapper
            (
            List<DocumentedAssembly> assemblies, 
            bool useObservableCollection, 
            EntryCreator creator, 
            IFileSystem fileSystem
            )
            : base(assemblies, useObservableCollection, creator)
        {
            _fileSystem = fileSystem;
        }

        /// <summary>
        /// Generates a document map grouping related namespaces.
        /// </summary>
		public override DocumentMap GenerateMap()
        {
            EntryCreator.Created = 0;
            DocumentMap map = UseObservableCollection ? new ObservableDocumentMap() : new DocumentMap();
            int fileCounter = 1;

            // For each of the documentedfiles generate the document map and add
            // it to the parent node of the document map
            for (int i = 0; i < CurrentFiles.Count; i++)
            {
                if (!CurrentFiles[i].IsCompiled)
                    continue;

                GenerateDocumentForAssembly(map, CurrentFiles[i], ref fileCounter);
            }

            map.Sort();

            bool dontGroupNamespaces = true;
            List<string> counter = new List<string>();
            List<Entry> namespaceContainers = new List<Entry>();

            if (map.Count > 10)
            {
                // calculate the best level to create groups from or if there is no best place
                dontGroupNamespaces = false;
                float parentPercentage = 0, currentPercentage = 0;
                int currentLevel = 0;

                do
                {
                    counter.Clear();

                    for (int dmI = 0; dmI < map.Count; dmI++)
                    {
                        string[] parts = map[dmI].Name.Split('.');
                        string currentNamespace = parts.Length > currentLevel ? string.Join(".", parts, 0, currentLevel + 1) : string.Join(".", parts);

                        if (!counter.Contains(currentNamespace))
                        {
                            counter.Add(currentNamespace);
                        }
                    }

                    currentPercentage = counter.Count / ((float)map.Count);
                    if (parentPercentage < 0.1 && currentPercentage > 0.65 && currentLevel > 0)
                    {
                        dontGroupNamespaces = true;
                        break;
                    }

                    currentLevel++;
                    parentPercentage = counter.Count / ((float)map.Count);
                }
                while (counter.Count / ((float)map.Count) < 0.1306);
            }

            if (!dontGroupNamespaces)
            {
                // create all the top level groupings
                int id = 0;
                for (int cI = 0; cI < counter.Count; cI++)
                {
                    Entry namespaceContainer = EntryCreator.Create(EntryTypes.NamespaceContainer, counter[cI], null);
                    namespaceContainer.Key = id++;
                    if(string.IsNullOrEmpty(counter[cI]))
                    {
                        // this is the no name namespace
                        namespaceContainer.SubKey = "No Namespace";
                    }
                    else
                    {
                        namespaceContainer.SubKey = counter[cI] + "Namespaces";
                    }
                    namespaceContainers.Add(namespaceContainer);
                }

                // add all the namespaces to the groupings
                for (int namespaceI = 0; namespaceI < map.Count; namespaceI++)
                {
                    for (int containersI = namespaceContainers.Count; containersI > 0; containersI--)
                    {
                        if (map[namespaceI].Name.Contains(namespaceContainers[containersI - 1].Name))
                        {
                            map[namespaceI].Parent = namespaceContainers[containersI - 1];
                            namespaceContainers[containersI - 1].Children.Add(map[namespaceI]);
                            break;
                        }
                    }
                }
            }

            if (namespaceContainers.Count > 1)
            {
                map.Clear();

                for (int i = 0; i < namespaceContainers.Count; i++)
                {
                    namespaceContainers[i].Name += " Namespaces";
                    map.Add(namespaceContainers[i]);
                }
            }

            map.NumberOfEntries = EntryCreator.Created;

            return map;
        }

        protected override Entry GenerateDocumentForAssembly(DocumentMap map, DocumentedAssembly current, ref int fileCounter)
        {
            AssemblyDef assembly = GetAssemblyDef(current);
            XmlCodeCommentFile xmlComments = GetXmlCommentFile(current);

            Entry assemblyEntry = EntryCreator.Create(assembly, System.IO.Path.GetFileName(current.FileName), xmlComments);
            current.UniqueId = assembly.UniqueId = fileCounter++;
            assemblyEntry.Key = assembly.GetGloballyUniqueId();
            assemblyEntry.IsSearchable = false;
            Entry namespaceEntry = null;

            // Add the namespaces to the document map
            Dictionary<string, List<TypeDef>> typesInNamespaces = assembly.GetTypesInNamespaces();
            foreach(KeyValuePair<string, List<TypeDef>> currentNamespace in typesInNamespaces)
            {
                if(currentNamespace.Value.Count == 0)
                {
                    continue;
                }
                string namespaceSubKey = BuildSubkey(currentNamespace);

                namespaceEntry = Find(map, namespaceSubKey);
                if(namespaceEntry == null)
                {
                    string displayName = currentNamespace.Key;

                    if(string.IsNullOrEmpty(currentNamespace.Key))
                    {
                        displayName = "None";
                    }

                    namespaceEntry = EntryCreator.Create(currentNamespace, displayName, xmlComments);
                    namespaceEntry.Key = assemblyEntry.Key;
                    namespaceEntry.SubKey = namespaceSubKey;
                    namespaceEntry.IsSearchable = false;
                }

                // Add the types from that namespace to its map
                foreach(TypeDef currentType in currentNamespace.Value)
                {
                    if(currentType.IsCompilerGenerated || currentType.Name[0] == '<') continue;

                    PreEntryAddedEventArgs e = new PreEntryAddedEventArgs(currentType);
                    this.OnPreEntryAdded(e);
                    if(!e.Filter)
                    {
                        Entry typeEntry = EntryCreator.Create(currentType, currentType.GetDisplayName(false), xmlComments, namespaceEntry);
                        typeEntry.Key = currentType.GetGloballyUniqueId();
                        typeEntry.IsSearchable = true;

                        // For some elements we will not want to load the child objects
                        // this is currently for System.Enum derived values.
                        if
                            (
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
                if(namespaceEntry.Children.Count > 0)
                {
                    namespaceEntry.Children.Sort();
                    // we still need to add here otherwise we get duplicate namespaces.
                    assemblyEntry.Children.Add(namespaceEntry);
                    if(!map.Contains(namespaceEntry))
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

            // we are not interested in assemblies being used here so make them childless
            return this.EntryCreator.Create(null, string.Empty, null);
        }

        private XmlCodeCommentFile GetXmlCommentFile(DocumentedAssembly current)
        {
            XmlCodeCommentFile xmlComments;
            if(_fileSystem.FileExists(current.XmlFileName))
            {
                xmlComments = new XmlCodeCommentFile(current.XmlFileName).GetReusableFile();
            }
            else
            {
                xmlComments = new XmlCodeCommentFile();
            }
            return xmlComments;
        }

        private AssemblyDef GetAssemblyDef(DocumentedAssembly current)
        {
            AssemblyDef assembly;
            if(current.LoadedAssembly == null)
            {
                assembly = AssemblyDef.Create(current.FileName);
                current.LoadedAssembly = assembly;
            }
            else
            {
                assembly = current.LoadedAssembly;
            }
            return assembly;
        }
    }
}