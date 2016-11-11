
namespace TheBoxSoftware.Reflection
{
    using System;
    using System.Collections.Generic;
    using Core.COFF;
    using Core.PE;
    using Core;
    using Signitures;

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
        private IStringStream _stringStream;
        private BlobStream _blobStream;
        private MetadataDirectory _metadataDirectory;
        private MetadataStream _metadataStream;
        private MetadataToDefinitionMap _metadataMap;
        private byte[] _fileContents;

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

        /// <include file='code-documentation\reflection.xml' path='docs/assemblydef/member[@name="create2"]/*'/> 
        public static AssemblyDef Create(PeCoffFile peCoffFile)
        {
            AssemblyDef created = new AssemblyDefBuilder(peCoffFile).Build();

            created._fileName = peCoffFile.FileName;
            created._metadataDirectory = peCoffFile.GetMetadataDirectory();
            created._metadataStream = created._metadataDirectory.GetMetadataStream();
            created._blobStream = created._metadataDirectory.Streams[Streams.BlobStream] as BlobStream;
            created._metadataMap = peCoffFile.Map;
            created._fileContents = peCoffFile.FileContents;

            return created;
        }

        /// <include file='code-documentation\reflection.xml' path='docs/assemblydef/member[@name="gettypesinnamespaces"]/*'/> 
        public Dictionary<string, List<TypeDef>> GetTypesInNamespaces()
        {
            return _map.GetAllTypesInNamespaces();
        }

        /// <include file='code-documentation\reflection.xml' path='docs/assemblydef/member[@name="getnamespaces"]/*'/> 
        public List<string> GetNamespaces()
        {
            return _map.GetAllNamespaces();
        }

        /// <include file='code-documentation\reflection.xml' path='docs/assemblydef/member[@name="findtype"]/*'/> 
        public TypeDef FindType(string theNamespace, string theTypeName)
        {
            if(string.IsNullOrEmpty(theTypeName)) return null;
            return _map.FindTypeInNamespace(theNamespace, theTypeName);
        }

        /// <include file='code-documentation\reflection.xml' path='docs/assemblydef/member[@name="resolvemetadatatoken"]/*'/> 
        public ReflectedMember ResolveMetadataToken(uint metadataToken)
        {
            MetadataToDefinitionMap map = _peCoffFile.Map;
            MetadataStream metadataStream = _peCoffFile.GetMetadataDirectory().GetMetadataStream();

            // Get the details in the token
            ILMetadataToken token = (ILMetadataToken)(metadataToken & 0xff000000);
            uint index = metadataToken & 0x00ffffff;

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
                case ILMetadataToken.FieldDef:
                    returnItem = map.GetDefinition(MetadataTables.Field, metadataStream.GetEntryFor(MetadataTables.Field, index));
                    break;
            }

            return returnItem;
        }

        /// <include file='code-documentation\reflection.xml' path='docs/assemblydef/member[@name="getgloballyuniqueid"]/*'/> 
        public override long GetGloballyUniqueId() => ((long)UniqueId) << 32;

        /// <include file='code-documentation\reflection.xml' path='docs/assemblydef/member[@name="getassemblyid"]/*'/> 
        // public override long GetAssemblyId() => UniqueId;

        internal Signitures.Signiture GetSigniture(BlobIndex fromIndex)
        {
            return _blobStream.GetSigniture(fromIndex.Value, fromIndex.SignitureType);
        }

        internal List<TypeRef> GetExtendindTypes(TypeDef type, CodedIndex ciForThisType)
        {
            List<TypeRef> inheritingTypes = new List<TypeRef>();
            List<CodedIndex> ourIndexes = new List<CodedIndex>(); // our coded index in typedef and any that appear in the type spec metadata signitures

            ourIndexes.Add(ciForThisType);

            // All types in this assembly that extend another use the TypeDef.Extends data in the metadata
            // table.
            if(type.IsGeneric)
            {
                MetadataRow[] typeSpecs = _metadataStream.Tables[MetadataTables.TypeSpec];
                for(int i = 0; i < typeSpecs.Length; i++)
                {
                    TypeSpecMetadataTableRow row = typeSpecs[i] as TypeSpecMetadataTableRow;
                    if(row != null)
                    {
                        // We need to find all of the TypeSpec references that point back to us, remember
                        // that as a generic type people can inherit from us in different ways - Type<int> or Type<string>
                        // for example. Each one of these will be a different type spec.
                        TypeSpec spec = _metadataMap.GetDefinition(MetadataTables.TypeSpec, row) as TypeSpec;
                        SignitureToken token = spec.Signiture.TypeToken.Tokens[0];

                        // First check if it is a GenericInstance as per the signiture spec in ECMA 23.2.14
                        if(token.TokenType == SignitureTokens.ElementType && ((ElementTypeSignatureToken)token).ElementType == ElementTypes.GenericInstance)
                        {
                            ElementTypeSignatureToken typeToken = spec.Signiture.TypeToken.Tokens[1] as ElementTypeSignatureToken;

                            TypeRef typeRef = typeToken.ResolveToken(Assembly);
                            if(typeRef == type)
                            {
                                ourIndexes.Add(new CodedIndex(MetadataTables.TypeSpec, (uint)i + 1));
                            }
                        }
                    }
                }
            }

            MetadataRow[] typeDefs = _metadataStream.Tables[MetadataTables.TypeDef];
            for(int i = 0; i < typeDefs.Length; i++)
            {
                for(int j = 0; j < ourIndexes.Count; j++)
                {
                    TypeDefMetadataTableRow row = (TypeDefMetadataTableRow)typeDefs[i];
                    CodedIndex ourCi = ourIndexes[j];

                    if(row.Extends == ourCi)
                    {
                        inheritingTypes.Add(
                            (TypeDef)_metadataMap.GetDefinition(MetadataTables.TypeDef, _metadataStream.Tables[MetadataTables.TypeDef][i])
                            );
                        continue; // a type can only be extending once so if we find ourselves we are done
                    }
                }
            }

            return inheritingTypes;
        }

        internal object ResolveCodedIndex(CodedIndex index)
        {
            object resolvedReference = null;

            if(_metadataStream.Tables.ContainsKey(index.Table))
            {
                if(_metadataStream.Tables[index.Table].Length + 1 > index.Index)
                {
                    MetadataRow metadataRow = _metadataStream.GetEntryFor(index);
                    resolvedReference = _metadataMap.GetDefinition(index.Table, metadataRow);
                }
            }

            return resolvedReference;
        }

        internal byte[] GetFileContents()
        {
            return _fileContents;
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
        public IStringStream StringStream
        {
            get { return _stringStream; }
            set { _stringStream = value; }
        }

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
    }
}