using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using TheBoxSoftware.Reflection;
using TheBoxSoftware.Reflection.Comments;

namespace TheBoxSoftware.Documentation
{
    /// <summary>
    /// Creates <see cref="DocumentMap"/>s based on the <see cref="Assemblies"/> and <see cref="Settings"/> provided.
    /// </summary>
    /// <remarks>
    /// <para>The document mapper creates a hierarchical representation of all of the entries in the documentation step. It
    /// is also the area where the decision about which details are provided easily for sorting and searching.</para>
    /// </remarks>
    public abstract class DocumentMapper : IDocumentMapper
    {
        private EventHandler<PreEntryAddedEventArgs> _preEntryAddedEvent;
        private List<DocumentedAssembly> _currentFiles;
        private DocumentMap _documentMap;
        private bool _useObservableCollection;
        private EntryCreator _entryCreator;

        /// <summary>
        /// Factory method for creating new DocumentMappers.
        /// </summary>
        /// <param name="assemblies">The assemblies to map.</param>
        /// <param name="typeOfMapper">The type of document mapper to instiate.</param>
        /// <param name="useObservableCollection">Wether or not to create an observable collection.</param>
        /// <param name="creator">The EntryCreator used to create new Entry instances.</param>
        /// <returns>The instantiated and initialised DocumentMapper.</returns>
        /// <exception cref="InvalidOperationException">
        /// The provided <paramref name="typeOfMapper"/> has no implementation, the document mapper failed to be
        /// created.
        /// </exception>
        public static IDocumentMapper Create(List<DocumentedAssembly> assemblies,
            Mappers typeOfMapper,
            bool useObservableCollection,
            EntryCreator creator)
        {

            DocumentMapper mapper = null;

            switch (typeOfMapper)
            {
                case Mappers.AssemblyFirst:
                    mapper = new AssemblyFirstDocumentMapper(assemblies, useObservableCollection, creator);
                    break;
                case Mappers.NamespaceFirst:
                    mapper = new NamespaceFirstDocumentMapper(assemblies, useObservableCollection, creator);
                    break;

                case Mappers.GroupedNamespaceFirst:
                default:
                    mapper = new GroupedNamespaceDocumentMapper(assemblies, useObservableCollection, creator);
                    break;
            }

            if (mapper == null)
                throw new InvalidOperationException(string.Format("There is no implementation of {0} document mapper. Document could not be mapped.", typeOfMapper));

            return mapper;
        }

        /// <summary>
        /// Initialises a new instance of the DocumentMapper.
        /// </summary>
        /// <param name="assemblies">The assembles to document.</param>
        /// <param name="useObservableCollection">Whether or not utilise an observable collection for the DocumentMap.</param>
        /// <param name="creator">The EntryCreator to use.</param>
        protected DocumentMapper(List<DocumentedAssembly> assemblies, bool useObservableCollection, EntryCreator creator)
        {
            _currentFiles = assemblies;
            _useObservableCollection = useObservableCollection;
            _entryCreator = creator;
        }

        /// <summary>
        /// Generates the document map based on the <see cref="CurrentFiles"/>.
        /// </summary>
        public virtual DocumentMap GenerateMap()
        {
            this.EntryCreator.Created = 0;
            DocumentMap map = this.UseObservableCollection ? new ObservableDocumentMap() : new DocumentMap();
            int fileCounter = 1;

            // For each of the documentedfiles generate the document map and add
            // it to the parent node of the document map
            for (int i = 0; i < this.CurrentFiles.Count; i++)
            {
                if (!this.CurrentFiles[i].IsCompiled)
                    continue;
                Entry assemblyEntry = this.GenerateDocumentForAssembly(map, this.CurrentFiles[i], ref fileCounter);
                if (assemblyEntry.Children.Count > 0)
                {
                    map.Add(assemblyEntry);
                }
            }

            map.Sort();
            map.NumberOfEntries = this.EntryCreator.Created;

            return map;
        }

        /// <summary>
        /// Finds the entry in the document map with the specified key.
        /// </summary>
        /// <param name="key">The key to search for.</param>
        /// <param name="checkChildren">Wether or not to check the child entries</param>
        /// <returns>The entry that relates to the key or null if not found</returns>
        protected Entry FindByKey(DocumentMap map, long key, string subKey, bool checkChildren)
        {
            Entry found = null;
            for (int i = 0; i < map.Count; i++)
            {
                found = map[i].FindByKey(key, subKey, checkChildren);
                if (found != null)
                {
                    break;
                }
            }
            return found;
        }

        protected virtual Entry GenerateDocumentForAssembly(DocumentMap map, DocumentedAssembly current, ref int fileCounter)
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
            foreach (KeyValuePair<string, List<TypeDef>> currentNamespace in assembly.GetTypesInNamespaces())
            {
                if (string.IsNullOrEmpty(currentNamespace.Key) || currentNamespace.Value.Count == 0)
                {
                    continue;
                }
                string namespaceSubKey = BuildSubkey(currentNamespace);

                namespaceEntry = FindByKey(map, assemblyEntry.Key, namespaceSubKey, false);
                if (namespaceEntry == null)
                {
                    namespaceEntry = EntryCreator.Create(currentNamespace, currentNamespace.Key, xmlComments);
                    namespaceEntry.Key = assemblyEntry.Key;
                    namespaceEntry.SubKey = namespaceSubKey;
                    namespaceEntry.IsSearchable = false;
                    map[0].Children.Add(namespaceEntry);
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
                        Entry typeEntry = EntryCreator.Create(currentType, currentType.GetDisplayName(false), xmlComments, namespaceEntry);
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
                }
            }

            // Make sure we dont display any empty namespaces
            for (int i = map[0].Children.Count - 1; i >= 0; i--)
            {
                Entry entry = map[0].Children[i];
                if (namespaceEntry.Children.Count == 0)
                {
                    map[0].Children.RemoveAt(i);
                }
            }

            return namespaceEntry;
        }

        protected string BuildSubkey(KeyValuePair<string, List<TypeDef>> namespaceEntry)
        {
            TypeDef def = namespaceEntry.Value[0];
            if (def.IsNested)
            {
                TypeDef container = def.ContainingClass;
                string baseNamespace = container.Namespace;
                List<string> containingClassNames = new List<string>();
                do
                {
                    containingClassNames.Add(container.Name);
                    container = container.ContainingClass;
                    if (container != null)
                    {
                        baseNamespace = container.Namespace;
                    }
                }
                while (container != null);
                containingClassNames.Add(baseNamespace);
                containingClassNames.Reverse();

                return string.Join(".", containingClassNames.ToArray());
            }
            else
            {
                return def.Namespace;
            }
        }

        /// <summary>
        /// Generates the document map for all of the types child elements, fields, properties
        /// and methods.
        /// </summary>
        /// <param name="typeDef">The type to generate the map for.</param>
        /// <param name="typeEntry">The entry to add the child elements to.</param>
        /// <param name="commentsXml">The assembly comment file.</param>
        protected virtual void GenerateTypeMap(TypeDef typeDef, Entry typeEntry, XmlCodeCommentFile commentsXml)
        {
            const string CONSTRUCTOR = "Constructors";
            const string METHOD = "Methods";
            const string OPERATOR = "Operators";
            const string FIELD = "Fields";
            const string PROPERTY = "Properties";
            const string EVENT = "Events";

            BuildMethodEntries(CONSTRUCTOR, typeDef.GetConstructors(), typeEntry, commentsXml);
            BuildMethodEntries(METHOD, typeDef.GetMethods(), typeEntry, commentsXml);
            BuildMethodEntries(OPERATOR, typeDef.GetOperators(), typeEntry, commentsXml);
            BuildEntries<FieldDef>(FIELD, typeDef.GetFields(), typeEntry, commentsXml);
            BuildEntries<PropertyDef>(PROPERTY, typeDef.GetProperties(), typeEntry, commentsXml);
            BuildEntries<EventDef>(EVENT, typeDef.GetEvents(), typeEntry, commentsXml);
        }

        /// <summary>
        /// Searches the top level elements for the specified <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The Entry if found else null.</returns>
        protected Entry Find(DocumentMap map, string name)
        {
            Entry found = null;
            for(int i = 0; i < map.Count; i++)
            {
                found = map[i].Name == name ? map[i] : null;
                if(found != null) break;
            }
            return found;
        }

        private void BuildEntries<T>(string collectionName, List<T> entries, Entry typeEntry, XmlCodeCommentFile commentsXml) where T: ReflectedMember
        {
            if(entries.Count == 0) return;

            ReflectedMember containingType = (ReflectedMember)typeEntry.Item;

            Entry eventsEntry = EntryCreator.Create(entries, collectionName, commentsXml, typeEntry);
            eventsEntry.IsSearchable = false;
            eventsEntry.Key = containingType.GetGloballyUniqueId();
            eventsEntry.SubKey = collectionName;

            foreach(T current in entries)
            {
                PreEntryAddedEventArgs e = new PreEntryAddedEventArgs(current);
                this.OnPreEntryAdded(e);
                if(!e.Filter)
                {
                    Entry propertyEntry = EntryCreator.Create(current, current.Name, commentsXml, eventsEntry);
                    propertyEntry.IsSearchable = true;
                    propertyEntry.Key = current.GetGloballyUniqueId();
                    eventsEntry.Children.Add(propertyEntry);
                }
            }

            if(eventsEntry.Children.Count > 0)
            {
                eventsEntry.Children.Sort();
                typeEntry.Children.Add(eventsEntry);
            }
        }

        private void BuildMethodEntries(string collectionName, List<MethodDef> entries, Entry typeEntry, XmlCodeCommentFile commentsXml)
        {
            if(entries.Count == 0) return;

            ReflectedMember containingType = (ReflectedMember)typeEntry.Item;

            Entry eventsEntry = EntryCreator.Create(entries, collectionName, commentsXml, typeEntry);
            eventsEntry.IsSearchable = false;
            eventsEntry.Key = containingType.GetGloballyUniqueId();
            eventsEntry.SubKey = collectionName;

            foreach(MethodDef current in entries)
            {
                PreEntryAddedEventArgs e = new PreEntryAddedEventArgs(current);
                this.OnPreEntryAdded(e);
                if(!e.Filter)
                {
                    Entry propertyEntry = EntryCreator.Create(current, current.GetDisplayName(false, false), commentsXml, eventsEntry);
                    propertyEntry.IsSearchable = true;
                    propertyEntry.Key = current.GetGloballyUniqueId();
                    eventsEntry.Children.Add(propertyEntry);
                }
            }

            if(eventsEntry.Children.Count > 0)
            {
                eventsEntry.Children.Sort();
                typeEntry.Children.Add(eventsEntry);
            }
        }

        /// <summary>
        /// Fires before a member is added to the document map.
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected void OnPreEntryAdded(PreEntryAddedEventArgs e)
        {
            if (_preEntryAddedEvent != null)
            {
                _preEntryAddedEvent(this, e);
            }
        }

        /// <summary>
        /// Event fired before a MemberRef is added to the DocumentMap.
        /// </summary>
        public event EventHandler<PreEntryAddedEventArgs> PreEntryAdded
        {
            add
            {
                _preEntryAddedEvent += value;
            }
            remove
            {
                _preEntryAddedEvent -= value;
            }
        }

        /// <summary>
        /// The currently documented files.
        /// </summary>
        /// <remarks>If this is updated you need to call <see cref="GenerateMap()"/> to update the DocumentMap.</remarks>
        protected List<DocumentedAssembly> CurrentFiles
        {
            get
            {
                return _currentFiles;
            }
            set
            {
                _currentFiles = value;
            }
        }

        /// <summary>
        /// Indicates if an observable collection should be used for the DocumentMap.
        /// </summary>
        protected bool UseObservableCollection
        {
            get
            {
                return this._useObservableCollection;
            }
            set
            {
                this._useObservableCollection = value;
            }
        }

        /// <summary>
        /// The EntryCreator to initialise new Entry instances in the DocumentMap
        /// </summary>
        protected EntryCreator EntryCreator
        {
            get
            {
                return _entryCreator;
            }
            set
            {
                _entryCreator = value;
            }
        }
    }
}