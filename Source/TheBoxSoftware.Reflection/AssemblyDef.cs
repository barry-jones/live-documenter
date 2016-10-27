
namespace TheBoxSoftware.Reflection
{
    using System;
    using System.Collections.Generic;
    using Core.COFF;
    using Core.PE;
    using Core;

    /// <include file='code-documentation\reflection.xml' path='docs/assemblydef/member[@name="class"]/*'/> 
    public class AssemblyDef : ReflectedMember
    {
        private PeCoffFile _file;
        private Core.Version _version;
        private int _uniqueIdCounter;

        internal AssemblyDef()
        {
            Modules = new List<ModuleDef>();
            Types = new List<TypeDef>();
            ReferencedAssemblies = new List<AssemblyRef>();
            Map = new TypeInNamespaceMap(this);
            base.Assembly = this;
        }

        /// <include file='code-documentation\reflection.xml' path='docs/assemblydef/member[@name="create"]/*'/> 
        public static AssemblyDef Create(string fileName)
        {
            if(string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(fileName);

            PeCoffFile peFile = new PeCoffFile(fileName);
            peFile.Initialise();

            if(!peFile.Directories.ContainsKey(DataDirectories.CommonLanguageRuntimeHeader))
            {
                throw new NotAManagedLibraryException($"The file '{fileName}' is not a managed library.");
            }

            return AssemblyDef.Create(peFile);
        }

        /// <include file='code-documentation\reflection.xml' path='docs/assemblydef/member[@name="create2"]/*'/> 
        public static AssemblyDef Create(PeCoffFile peCoffFile)
        {
            AssemblyDefBuilder builder = new AssemblyDefBuilder(peCoffFile);
            return builder.Build();
        }

        /// <include file='code-documentation\reflection.xml' path='docs/assemblydef/member[@name="gettypesinnamespaces"]/*'/> 
        public Dictionary<string, List<TypeDef>> GetTypesInNamespaces()
        {
            // REVIEW: see this.namespaceMap. Also appears to be wasteful
            List<string> orderedNamespaces = new List<string>();
            Dictionary<string, List<TypeDef>> temp = new Dictionary<string, List<TypeDef>>();

            foreach(TypeDef current in Types.FindAll(t => !t.IsCompilerGenerated))
            {
                if(!orderedNamespaces.Contains(current.Namespace))
                {
                    orderedNamespaces.Add(current.Namespace);
                }
            }

            orderedNamespaces.Sort();

            foreach(string current in orderedNamespaces)
            {
                temp.Add(current, new List<TypeDef>());
            }
            foreach(TypeDef current in Types)
            {
                if(temp.ContainsKey(current.Namespace))
                {
                    temp[current.Namespace].Add(current);
                }
            }
            return temp;
        }

        /// <include file='code-documentation\reflection.xml' path='docs/assemblydef/member[@name="isnamespacedefined"]/*'/> 
        public bool IsNamespaceDefined(string theNamespace)
        {
            return Map.ContainsNamespace(theNamespace);
        }

        /// <include file='code-documentation\reflection.xml' path='docs/assemblydef/member[@name="getnamespaces"]/*'/> 
        public List<string> GetNamespaces()
        {
            return Map.GetAllNamespaces();
        }

        /// <include file='code-documentation\reflection.xml' path='docs/assemblydef/member[@name="findtype"]/*'/> 
        public TypeDef FindType(string theNamespace, string theTypeName)
        {
            if(string.IsNullOrEmpty(theTypeName) || string.IsNullOrEmpty(theNamespace)) return null;
            return Map.FindTypeInNamespace(theNamespace, theTypeName);
        }

        /// <include file='code-documentation\reflection.xml' path='docs/assemblydef/member[@name="resolvemetadatatoken"]/*'/> 
        public ReflectedMember ResolveMetadataToken(int metadataToken)
        {
            MetadataToDefinitionMap map = this.File.Map;
            Core.COFF.MetadataStream metadataStream = this.File.GetMetadataDirectory().GetMetadataStream();

            // Get the details in the token
            ILMetadataToken token = (ILMetadataToken)(metadataToken & 0xff000000);
            int index = metadataToken & 0x00ffffff;

            ReflectedMember returnItem = null;

            // 
            switch(token)
            {
                // Method related tokens
                case ILMetadataToken.MethodDef:
                    returnItem = map.GetDefinition(MetadataTables.MethodDef, metadataStream.GetEntryFor(MetadataTables.MethodDef, index));
                    break;
                case ILMetadataToken.MemberRef:
                    returnItem = map.GetDefinition(MetadataTables.MemberRef, metadataStream.GetEntryFor(MetadataTables.MemberRef, index));
                    break;
                case ILMetadataToken.MethodSpec:
                    returnItem = map.GetDefinition(MetadataTables.MethodSpec, metadataStream.GetEntryFor(MetadataTables.MethodSpec, index));
                    break;
                // Type related tokens
                case ILMetadataToken.TypeDef:
                    returnItem = map.GetDefinition(MetadataTables.TypeDef, metadataStream.GetEntryFor(MetadataTables.TypeDef, index));
                    break;
                case ILMetadataToken.TypeRef:
                    returnItem = map.GetDefinition(MetadataTables.TypeRef, metadataStream.GetEntryFor(MetadataTables.TypeRef, index));
                    break;
                case ILMetadataToken.TypeSpec:
                    returnItem = map.GetDefinition(MetadataTables.TypeSpec, metadataStream.GetEntryFor(MetadataTables.TypeSpec, index));
                    break;
            }

            return returnItem;
        }

        /// <include file='code-documentation\reflection.xml' path='docs/assemblydef/member[@name="getgloballyuniqueid"]/*'/> 
        public override long GetGloballyUniqueId() => ((long)this.UniqueId) << 32;

        /// <include file='code-documentation\reflection.xml' path='docs/assemblydef/member[@name="getassemblyid"]/*'/> 
        public override long GetAssemblyId() => this.UniqueId;

#if TEST
        /// <summary>
        /// Prints all the type spec signitures to the Trace stream
        /// </summary>
        public void PrintTypeSpecSignitures()
        {
            // Read the metadata from the file and populate the entries
            MetadataDirectory metadata = this.File.GetMetadataDirectory();
            MetadataStream metadataStream = (MetadataStream)metadata.Streams[Streams.MetadataStream];
            int count = 0;

            //
            StringStream stringStream = (StringStream)metadata.Streams[Streams.StringStream];

            if (metadataStream.Tables.ContainsKey(MetadataTables.TypeSpec))
            {
                foreach (TypeSpecMetadataTableRow typeSpecRow in metadataStream.Tables[MetadataTables.TypeSpec])
                {
                    TypeSpec type = TypeSpec.CreateFromMetadata(this, metadata, typeSpecRow);
                    type.Signiture.PrintTokens();
                }
            }
        }
#endif

        /// <summary>
        /// Get the next available unique identifier for this assembly.
        /// </summary>
        /// <returns>The unique identifier</returns>
        internal int CreateUniqueId()
        {
            return _uniqueIdCounter++;
        }

        /// <summary>
        /// The <see cref="PeCoffFile"/> the assembly was reflected from.
        /// </summary>
        public PeCoffFile File { get; set; }

        /// <summary>
        /// The version details for this assembly.
        /// </summary>
        public Core.Version Version
        {
            get { return _version; }
            set { _version = value; }
        }

        /// <include file='code-documentation\reflection.xml' path='docs/assemblydef/member[@name="stringstream"]/*'/> 
        public IStringStream StringStream { get; set; }

        public List<AssemblyRef> ReferencedAssemblies { get; set; }

        public List<ModuleDef> Modules { get; set; }

        public List<TypeDef> Types { get; set; }

        internal TypeInNamespaceMap Map { get; set; }
    }
}