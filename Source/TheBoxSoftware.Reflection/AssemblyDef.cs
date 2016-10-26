using System;
using System.Collections.Generic;
using TheBoxSoftware.Reflection.Core.COFF;
using TheBoxSoftware.Reflection.Core.PE;
using TheBoxSoftware.Reflection.Core;

namespace TheBoxSoftware.Reflection
{
    /// <include file='code-documentation\reflection.xml' path='docs/assemblydef/member[@name="class"]/*'/> 
    public class AssemblyDef : ReflectedMember
    {
        private List<AssemblyRef> _referencedAssemblies;
        private List<ModuleDef> _modules;
        private List<TypeDef> _types;
        private PeCoffFile _file;
        private Core.Version _version;
        private int _uniqueIdCounter;
        private TypeInNamespaceMap _namspaceMap;
        private IStringStream _stringStream;

        internal AssemblyDef()
        {
            _modules = new List<ModuleDef>();
            _types = new List<TypeDef>();
            _referencedAssemblies = new List<AssemblyRef>();
            _namspaceMap = new TypeInNamespaceMap(this);
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
                peFile = null;  // would be nice to get the memory back
                throw new NotAManagedLibraryException($"The file '{fileName}' is not a managed library.");
            }

            return AssemblyDef.Create(peFile);
        }

        /// <include file='code-documentation\reflection.xml' path='docs/assemblydef/member[@name="create2"]/*'/> 
        public static AssemblyDef Create(PeCoffFile peCoffFile)
        {
            if(peCoffFile == null)
                throw new ArgumentNullException("peCoffFile");

            if(!peCoffFile.Directories.ContainsKey(DataDirectories.CommonLanguageRuntimeHeader))
                throw new NotAManagedLibraryException($"The file '{peCoffFile.FileName}' is not a managed library.");

            AssemblyDef assembly = new AssemblyDef();
            assembly.File = peCoffFile;
            MetadataToDefinitionMap map = peCoffFile.Map;
            map.Assembly = assembly;

            // Read the metadata from the file and populate the entries
            MetadataDirectory metadata = peCoffFile.GetMetadataDirectory();
            MetadataStream metadataStream = (MetadataStream)metadata.Streams[Streams.MetadataStream];

            assembly.StringStream = (IStringStream)metadata.Streams[Streams.StringStream]; // needs to be populated first
            assembly.LoadAssemblyMetadata(metadataStream);
            assembly.LoadAssemblyRefMetadata(map, metadata, metadataStream);
            assembly.LoadModuleMetadata(map, metadata, metadataStream);
            assembly.LoadTypeRefMetadata(map, metadata, metadataStream);
            assembly.LoadTypeDefMetadata(map, metadata, metadataStream);
            assembly.LoadMemberRefMetadata(map, metadata, metadataStream);
            assembly.LoadTypeSpecMetadata(map, metadata, metadataStream);
            assembly.LoadNestedClassMetadata(map, metadataStream);
            assembly.LoadInterfaceImplMetadata(map, metadataStream);
            assembly.LoadConstantMetadata(map, metadataStream);
            assembly.LoadCustomAttributeMetadata(map, metadataStream);

            return assembly;
        }

        /// <include file='code-documentation\reflection.xml' path='docs/assemblydef/member[@name="gettypesinnamespaces"]/*'/> 
        public Dictionary<string, List<TypeDef>> GetTypesInNamespaces()
        {
            // REVIEW: see this.namespaceMap. Also appears to be wasteful
            List<string> orderedNamespaces = new List<string>();
            Dictionary<string, List<TypeDef>> temp = new Dictionary<string, List<TypeDef>>();

            foreach(TypeDef current in this._types.FindAll(t => !t.IsCompilerGenerated))
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
            foreach(TypeDef current in this._types)
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
            return _namspaceMap.ContainsNamespace(theNamespace);
        }

        /// <include file='code-documentation\reflection.xml' path='docs/assemblydef/member[@name="getnamespaces"]/*'/> 
        public List<string> GetNamespaces()
        {
            return _namspaceMap.GetAllNamespaces();
        }

        /// <include file='code-documentation\reflection.xml' path='docs/assemblydef/member[@name="findtype"]/*'/> 
        public TypeDef FindType(string theNamespace, string theTypeName)
        {
            if(string.IsNullOrEmpty(theTypeName)) return null;
            return _namspaceMap.FindTypeInNamespace(theNamespace, theTypeName);
        }

        /// <summary>
        /// Get the next available unique identifier for this assembly.
        /// </summary>
        /// <returns>The unique identifier</returns>
        internal int CreateUniqueId()
        {
            return _uniqueIdCounter++;
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

        /// <include file='code-documentation\reflection.xml' path='docs/assemblydef/member[@name="resolvecodedindex"]/*'/> 
        public object ResolveCodedIndex(CodedIndex index)
        {
            object resolvedReference = null;

            MetadataDirectory metadata = this.File.GetMetadataDirectory();
            MetadataStream metadataStream = (MetadataStream)metadata.Streams[Streams.MetadataStream];
            if(metadataStream.Tables.ContainsKey(index.Table))
            {
                if(metadataStream.Tables[index.Table].Length + 1 > index.Index)
                {
                    MetadataToDefinitionMap map = this.File.Map;
                    MetadataRow metadataRow = metadataStream.GetEntryFor(index);
                    resolvedReference = map.GetDefinition(index.Table, metadataRow);
                }
            }

            return resolvedReference;
        }

        /// <include file='code-documentation\reflection.xml' path='docs/assemblydef/member[@name="getgloballyuniqueid"]/*'/> 
        public override long GetGloballyUniqueId()
        {
            return ((long)this.UniqueId) << 32;
        }

        /// <include file='code-documentation\reflection.xml' path='docs/assemblydef/member[@name="getassemblyid"]/*'/> 
        public override long GetAssemblyId()
        {
            return this.UniqueId;
        }

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

        private void LoadCustomAttributeMetadata(MetadataToDefinitionMap map, MetadataStream metadataStream)
        {
            if(!metadataStream.Tables.ContainsKey(MetadataTables.CustomAttribute))
                return;

            MetadataRow[] customAttributes = metadataStream.Tables[MetadataTables.CustomAttribute];
            for(int i = 0; i < customAttributes.Length; i++)
            {
                CustomAttributeMetadataTableRow customAttributeRow = customAttributes[i] as CustomAttributeMetadataTableRow;

                ReflectedMember attributeTo = map.GetDefinition(customAttributeRow.Parent.Table,
                    metadataStream.GetEntryFor(customAttributeRow.Parent)
                    );
                MemberRef ofType = (MemberRef)map.GetDefinition(customAttributeRow.Type.Table,
                    metadataStream.GetEntryFor(customAttributeRow.Type)
                    );

                if(attributeTo != null)
                {
                    CustomAttribute attribute = new CustomAttribute(ofType);
                    attributeTo.Attributes.Add(attribute);
                }
            }
        }

        private void LoadConstantMetadata(MetadataToDefinitionMap map, MetadataStream metadataStream)
        {
            if(!metadataStream.Tables.ContainsKey(MetadataTables.Constant))
                return;

            MetadataRow[] constants = metadataStream.Tables[MetadataTables.Constant];
            for(int i = 0; i < constants.Length; i++)
            {
                ConstantMetadataTableRow constantRow = constants[i] as ConstantMetadataTableRow;
                ConstantInfo constant = ConstantInfo.CreateFromMetadata(this, metadataStream, constantRow);

                switch(constantRow.Parent.Table)
                {
                    case MetadataTables.Field:
                        FieldDef field = (FieldDef)map.GetDefinition(MetadataTables.Field,
                            metadataStream.GetEntryFor(MetadataTables.Field, constantRow.Parent.Index)
                            );
                        field.Constants.Add(constant);
                        break;
                    case MetadataTables.Property:
                        break;
                    case MetadataTables.Param:
                        ParamDef parameter = (ParamDef)map.GetDefinition(MetadataTables.Param,
                            metadataStream.GetEntryFor(MetadataTables.Param, constantRow.Parent.Index)
                            );
                        parameter.Constants.Add(constant);
                        break;
                }
            }
        }

        private void LoadInterfaceImplMetadata(MetadataToDefinitionMap map, MetadataStream metadataStream)
        {
            if(!metadataStream.Tables.ContainsKey(MetadataTables.InterfaceImpl))
                return;
            
            MetadataRow[] interfaceImplementations = metadataStream.Tables[MetadataTables.InterfaceImpl];
            for(int i = 0; i < interfaceImplementations.Length; i++)
            {
                InterfaceImplMetadataTableRow interfaceImplRow = interfaceImplementations[i] as InterfaceImplMetadataTableRow;
                TypeDefMetadataTableRow implementingClassRow = (TypeDefMetadataTableRow)metadataStream.Tables.GetEntryFor(
                    MetadataTables.TypeDef, interfaceImplRow.Class
                    );
                MetadataRow interfaceRow = metadataStream.Tables.GetEntryFor(
                    interfaceImplRow.Interface.Table,
                    interfaceImplRow.Interface.Index);

                TypeDef implementingClass = (TypeDef)map.GetDefinition(MetadataTables.TypeDef, implementingClassRow);
                TypeRef implementedClass = (TypeRef)map.GetDefinition(interfaceImplRow.Interface.Table, interfaceRow);
                if(implementedClass is TypeSpec)
                {
                    ((TypeSpec)implementedClass).ImplementingType = implementingClass;
                }
                implementingClass.Implements.Add((TypeRef)map.GetDefinition(interfaceImplRow.Interface.Table, interfaceRow));
            }
        }

        private void LoadNestedClassMetadata(MetadataToDefinitionMap map, MetadataStream metadataStream)
        {
            if(!metadataStream.Tables.ContainsKey(MetadataTables.NestedClass))
                return;

            MetadataRow[] nestedClasses = metadataStream.Tables[MetadataTables.NestedClass];
            for(int i = 0; i < nestedClasses.Length; i++)
            {
                NestedClassMetadataTableRow nestedClassRow = nestedClasses[i] as NestedClassMetadataTableRow;
                TypeDefMetadataTableRow nestedClass = (TypeDefMetadataTableRow)metadataStream.Tables.GetEntryFor(
                    MetadataTables.TypeDef, nestedClassRow.NestedClass
                    );
                TypeDef container = (TypeDef)map.GetDefinition(MetadataTables.TypeDef, metadataStream.Tables.GetEntryFor(
                    MetadataTables.TypeDef, nestedClassRow.EnclosingClass
                    ));
                TypeDef nested = (TypeDef)map.GetDefinition(MetadataTables.TypeDef, nestedClass);
                nested.ContainingClass = container;
                _namspaceMap.Add(nested.Namespace, MetadataTables.TypeDef, nestedClass);
            }
        }

        private void LoadTypeSpecMetadata(MetadataToDefinitionMap map, MetadataDirectory metadata, MetadataStream metadataStream)
        {
            if(!metadataStream.Tables.ContainsKey(MetadataTables.TypeSpec))
                return;
            
            foreach(TypeSpecMetadataTableRow typeSpecRow in metadataStream.Tables[MetadataTables.TypeSpec])
            {
                TypeSpec typeRef = TypeSpec.CreateFromMetadata(this, metadata, typeSpecRow);
                map.Add(MetadataTables.TypeSpec, typeSpecRow, typeRef);
            }
        }

        private void LoadMemberRefMetadata(MetadataToDefinitionMap map, MetadataDirectory metadata, MetadataStream metadataStream)
        {
            if(!metadataStream.Tables.ContainsKey(MetadataTables.MemberRef))
                return;
            
            int count = metadataStream.Tables[MetadataTables.MemberRef].Length;
            for(int i = 0; i < count; i++)
            {
                MemberRefMetadataTableRow memberRefRow = (MemberRefMetadataTableRow)metadataStream.Tables[MetadataTables.MemberRef][i];
                MemberRef memberRef = MemberRef.CreateFromMetadata(this, metadata, memberRefRow);
                map.Add(MetadataTables.MemberRef, memberRefRow, memberRef);
            }
        }

        private void LoadTypeDefMetadata(MetadataToDefinitionMap map, MetadataDirectory metadata, MetadataStream metadataStream)
        {
            int count = metadataStream.Tables[MetadataTables.TypeDef].Length;
            for(int i = 0; i < count; i++)
            {
                TypeDefMetadataTableRow typeDefRow = (TypeDefMetadataTableRow)metadataStream.Tables[MetadataTables.TypeDef][i];
                TypeDef type = TypeDef.CreateFromMetadata(this, metadata, typeDefRow);
                map.Add(MetadataTables.TypeDef, typeDefRow, type);
                _namspaceMap.Add(type.Namespace, MetadataTables.TypeDef, typeDefRow);
                _types.Add(type);
            }
        }

        private void LoadTypeRefMetadata(MetadataToDefinitionMap map, MetadataDirectory metadata, MetadataStream metadataStream)
        {
            if(!metadataStream.Tables.ContainsKey(MetadataTables.TypeRef))
                return;
            
            foreach(TypeRefMetadataTableRow typeRefRow in metadataStream.Tables[MetadataTables.TypeRef])
            {
                TypeRef typeRef = TypeRef.CreateFromMetadata(this, metadata, typeRefRow);
                map.Add(MetadataTables.TypeRef, typeRefRow, typeRef);
            }
        }

        private void LoadModuleMetadata(MetadataToDefinitionMap map, MetadataDirectory metadata, MetadataStream metadataStream)
        {
            foreach(ModuleMetadataTableRow moduleRow in metadataStream.Tables[MetadataTables.Module])
            {
                ModuleDef module = ModuleDef.CreateFromMetadata(this, metadata, moduleRow);
                map.Add(MetadataTables.Module, moduleRow, module);
                _modules.Add(module);
            }
        }

        private void LoadAssemblyRefMetadata(MetadataToDefinitionMap map, MetadataDirectory metadata, MetadataStream metadataStream)
        {
            if(!metadataStream.Tables.ContainsKey(MetadataTables.AssemblyRef))
                return;
            
            MetadataRow[] items = metadataStream.Tables[MetadataTables.AssemblyRef];
            for(int i = 0; i < items.Length; i++)
            {
                AssemblyRefMetadataTableRow assemblyRefRow = items[i] as AssemblyRefMetadataTableRow;
                AssemblyRef assemblyRef = AssemblyRef.CreateFromMetadata(this, assemblyRefRow);
                map.Add(MetadataTables.AssemblyRef, assemblyRefRow, assemblyRef);
                _referencedAssemblies.Add(assemblyRef);
            }
        }

        private void LoadAssemblyMetadata(MetadataStream metadataStream)
        {
            if(!metadataStream.Tables.ContainsKey(MetadataTables.Assembly))
                return;
            
            // Always one and only
            AssemblyMetadataTableRow assemblyRow = (AssemblyMetadataTableRow)metadataStream.Tables[MetadataTables.Assembly][0];
            Name = StringStream.GetString(assemblyRow.Name.Value);
            Version = assemblyRow.GetVersion();
        }

        /// <summary>
        /// The <see cref="PeCoffFile"/> the assembly was reflected from.
        /// </summary>
        public TheBoxSoftware.Reflection.Core.PeCoffFile File
        {
            get { return _file; }
            set { _file = value; }
        }

        /// <summary>
        /// The version details for this assembly.
        /// </summary>
        public TheBoxSoftware.Reflection.Core.Version Version
        {
            get { return this._version; }
            set { this._version = value; }
        }

        /// <include file='code-documentation\reflection.xml' path='docs/assemblydef/member[@name="stringstream"]/*'/> 
        public IStringStream StringStream
        {
            get { return _stringStream; }
            set { _stringStream = value; }
        }
    }
}