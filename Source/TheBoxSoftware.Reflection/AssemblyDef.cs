
namespace TheBoxSoftware.Reflection
{
    using System;
    using System.Collections.Generic;
    using Core.COFF;
    using Core.PE;
    using Core;

    /// <summary>
    /// The AssemblyDef provides the top level information and entry an point to
    /// all types, methods etc reflected from a .NET executable.
    /// </summary>
    /// <include file='code-documentation\reflection.xml' path='docs/assemblydef/member[@name="class"]/*'/> 
    public class AssemblyDef : ReflectedMember
    {
        private Core.Version _version;
        private int _uniqueIdCounter;

        private List<ModuleDef> _modules;
        private List<TypeDef> _types;
        private List<AssemblyRef> _referencedAssemblies;

        // attempt to remove the PeCoffFile references
        private PeCoffFile _peCoffFile;
        private string _fileName;
        private TypeInNamespaceMap _map;

        internal AssemblyDef()
        {
            _modules = new List<ModuleDef>();
            _types = new List<TypeDef>();
            _referencedAssemblies = new List<AssemblyRef>();
            _map = new TypeInNamespaceMap();
            base.Assembly = this;
        }

        internal AssemblyDef(PeCoffFile peCoffFile) : this()
        {
            _peCoffFile = peCoffFile;
        }

        /// <summary>
        /// Creates and instantiates an AssemblyDef based on the provided filename.
        /// </summary>
        /// <param name="fileName">The file name of the assembly to reflect.</param>
        /// <returns>The insantiated AssemblyDef</returns>
        /// <include file='code-documentation\reflection.xml' path='docs/assemblydef/member[@name="create"]/*'/> 
        public static AssemblyDef Create(string fileName)
        {
            if(string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(fileName);

            PeCoffFile peFile = new PeCoffFile(fileName, new FileSystem());
            peFile.Initialise();

            if(!peFile.Directories.ContainsKey(DataDirectories.CommonLanguageRuntimeHeader))
            {
                throw new NotAManagedLibraryException($"The file '{fileName}' is not a managed library.");
            }

            return Create(peFile);
        }

        /// <summary>
        /// Initialises and instantiates an AssemblyDef instance for the provided
        /// <see cref = "PeCoffFile" /> (assembly).
        /// </summary>
        /// <param name="peCoffFile">The PeCoffFile to load the AssemblyDef from.</param>
        /// <returns>The instantiated AssemblyDef.</returns>
        /// <include file='code-documentation\reflection.xml' path='docs/assemblydef/member[@name="create2"]/*'/> 
        public static AssemblyDef Create(PeCoffFile peCoffFile)
        {
            AssemblyDef created = new AssemblyDefBuilder(peCoffFile).Build();

            created._fileName = peCoffFile.FileName;

            return created;
        }

        /// <summary>
        /// Returns a dictionary of all of the types in this assembly in their 
        /// respective namespaces.
        /// </summary>
        public Dictionary<string, List<TypeDef>> GetTypesInNamespaces()
        {
            return _map.GetAllTypesInNamespaces();
        }

        /// <summary>
        /// Gets a list of all of the namespaces defined in this assembly.
        /// </summary>
        public List<string> GetNamespaces()
        {
            return _map.GetAllNamespaces();
        }

        /// <summary>
        /// Searches the assembly for the named type in the specified assembly.
        /// </summary>
        /// <param name="theNamespace">The namespace to search for the type in.</param>
        /// <param name="theTypeName">The name of the type</param>
        /// <returns>The resolved type definition or null if not found.</returns>
        public TypeDef FindType(string theNamespace, string theTypeName)
        {
            if(string.IsNullOrEmpty(theTypeName)) return null;
            return _map.FindTypeInNamespace(theNamespace, theTypeName);
        }

        /// <summary>
        /// Helps to resolve tokens from the metadata to there associated types and elements inside
        /// this assembly.
        /// </summary>
        /// <param name="metadataToken">The metadata token to resolve</param>
        /// <returns>A resolved token reference or null if not found in this assembly.</returns>
        /// <include file='code-documentation\reflection.xml' path='docs/assemblydef/member[@name="resolvemetadatatoken"]/*'/> 
        public ReflectedMember ResolveMetadataToken(uint metadataToken)
        {
            return _peCoffFile.ResolveMetadataToken(metadataToken);
        }

        /// <include file='code-documentation\reflection.xml' path='docs/assemblydef/member[@name="getgloballyuniqueid"]/*'/> 
        public override long GetGloballyUniqueId() => ((long)UniqueId) << 32;

        internal Signatures.Signature GetSigniture(BlobIndex fromIndex)
        {
            BlobStream stream = _peCoffFile.GetMetadataDirectory().Streams[Streams.BlobStream] as BlobStream;
            return stream.GetSigniture(fromIndex.Value, fromIndex.SignitureType);
        }

        internal object ResolveCodedIndex(CodedIndex index)
        {
            MetadataStream stream = _peCoffFile.GetMetadataDirectory().GetMetadataStream();
            object resolvedReference = null;

            if(stream.Tables.ContainsKey(index.Table))
            {
                if(stream.Tables[index.Table].Length + 1 > index.Index)
                {
                    MetadataRow metadataRow = stream.GetEntryFor(index);
                    resolvedReference = _peCoffFile.Map.GetDefinition(index.Table, metadataRow);
                }
            }

            return resolvedReference;
        }

        internal byte[] GetFileContents()
        {
            return _peCoffFile.FileContents;
        }

        internal uint FileAddressFromRVA(uint rva)
        {
            return _peCoffFile.GetAddressFromRVA(rva);
        }

        /// <summary>
        /// Get the next available unique identifier for this assembly.
        /// </summary>
        /// <returns>The unique identifier</returns>
        internal int CreateUniqueId() => _uniqueIdCounter++;

#if TEST
        /// <summary>
        /// Prints all the type spec Signatures to the Trace stream
        /// </summary>
        public void PrintTypeSpecSignatures()
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
        /// The filename of the underlying assembly used to build this assembly definition.
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
        }

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

        public List<AssemblyRef> ReferencedAssemblies
        {
            get { return _referencedAssemblies; }
            set { _referencedAssemblies = value; }
        }

        public List<ModuleDef> Modules
        {
            get { return _modules; }
            set { _modules = value; }
        }

        public List<TypeDef> Types
        {
            get { return _types; }
            set { _types = value; }
        }

        internal TypeInNamespaceMap Map
        {
            get { return _map; }
            set { _map = value; }
        }

        /// <summary>
        /// Obtain a reference to the underlying PeCoffFile
        /// </summary>
        internal PeCoffFile PeCoffFile { get => _peCoffFile; }
    }
}