﻿
namespace TheBoxSoftware.Reflection
{
    using System;
    using Core;
    using Core.COFF;
    using Core.PE;

    internal class AssemblyDefBuilder
    {
        private PeCoffFile _peCoffFile;
        private MetadataToDefinitionMap _map;
        private MetadataDirectory _metadata;
        private MetadataStream _stream;
        private AssemblyDef _assembly;

        private BuildReferences _references;

        public AssemblyDefBuilder(PeCoffFile peCoffFile)
        {
            if(peCoffFile == null)
                throw new ArgumentNullException(nameof(peCoffFile));
            if(!peCoffFile.Directories.ContainsKey(DataDirectories.CommonLanguageRuntimeHeader))
                throw new NotAManagedLibraryException($"The file '{peCoffFile.FileName}' is not a managed library.");

            _peCoffFile = peCoffFile;
            _map = _peCoffFile.Map;

            _references = new BuildReferences();
            _references.Map = _map;
            _references.PeCoffFile = _peCoffFile;
            _references.Metadata = _peCoffFile.GetMetadataDirectory();
        }

        public AssemblyDef Build()
        {
            if(_assembly != null) return _assembly; // we have already built it return previous
            
            _assembly = new AssemblyDef(_peCoffFile);
            _references.Assembly = _assembly;
            _map.Assembly = _assembly;

            // Read the metadata from the file and populate the entries
            _metadata = _peCoffFile.GetMetadataDirectory();
            _stream = _metadata.Streams[Streams.MetadataStream] as MetadataStream;

            _assembly.StringStream = _metadata.Streams[Streams.StringStream] as IStringStream; // needs to be populated first

            LoadAssemblyMetadata();
            LoadAssemblyRefMetadata();
            LoadModuleMetadata();
            LoadTypeRefMetadata();
            LoadTypeDefMetadata();
            LoadMemberRefMetadata();
            LoadTypeSpecMetadata();
            LoadNestedClassMetadata();
            LoadInterfaceImplMetadata();
            LoadConstantMetadata();
            LoadCustomAttributeMetadata();

            // assign the built assembly locally and clear up the unused references
            _peCoffFile = null;
            _map = null;
            _metadata = null;
            _stream = null;

            return _assembly;
        }

        private void LoadCustomAttributeMetadata()
        {
            if(!_stream.Tables.ContainsKey(MetadataTables.CustomAttribute))
                return;

            MetadataRow[] customAttributes = _stream.Tables[MetadataTables.CustomAttribute];
            for(int i = 0; i < customAttributes.Length; i++)
            {
                CustomAttributeMetadataTableRow customAttributeRow = customAttributes[i] as CustomAttributeMetadataTableRow;

                ReflectedMember attributeTo = _map.GetDefinition(customAttributeRow.Parent.Table,
                    _stream.GetEntryFor(customAttributeRow.Parent)
                    );
                MemberRef ofType = _map.GetDefinition(customAttributeRow.Type.Table,
                    _stream.GetEntryFor(customAttributeRow.Type)
                    ) as MemberRef;

                if(attributeTo != null)
                {
                    CustomAttribute attribute = new CustomAttribute(ofType);
                    attributeTo.Attributes.Add(attribute);
                }
            }
        }

        private void LoadConstantMetadata()
        {
            if(!_stream.Tables.ContainsKey(MetadataTables.Constant))
                return;

            MetadataRow[] constants = _stream.Tables[MetadataTables.Constant];

            for(int i = 0; i < constants.Length; i++)
            {
                ConstantMetadataTableRow constantRow = constants[i] as ConstantMetadataTableRow;
                ConstantInfo constant = ConstantInfo.CreateFromMetadata(_assembly, _stream, constantRow);

                switch(constantRow.Parent.Table)
                {
                    case MetadataTables.Field:
                        FieldDef field = _map.GetDefinition(MetadataTables.Field,
                            _stream.GetEntryFor(MetadataTables.Field, constantRow.Parent.Index)
                            ) as FieldDef;
                        field.Constants.Add(constant);
                        break;
                    case MetadataTables.Property:
                        break;
                    case MetadataTables.Param:
                        ParamDef parameter = _map.GetDefinition(MetadataTables.Param,
                            _stream.GetEntryFor(MetadataTables.Param, constantRow.Parent.Index)
                            ) as ParamDef;
                        parameter.Constants.Add(constant);
                        break;
                }
            }
        }

        private void LoadInterfaceImplMetadata()
        {
            if(!_stream.Tables.ContainsKey(MetadataTables.InterfaceImpl))
                return;

            MetadataRow[] interfaceImplementations = _stream.Tables[MetadataTables.InterfaceImpl];

            for(int i = 0; i < interfaceImplementations.Length; i++)
            {
                InterfaceImplMetadataTableRow interfaceImplRow = interfaceImplementations[i] as InterfaceImplMetadataTableRow;
                TypeDefMetadataTableRow implementingClassRow = _stream.Tables.GetEntryFor(
                    MetadataTables.TypeDef, interfaceImplRow.Class
                    ) as TypeDefMetadataTableRow;
                MetadataRow interfaceRow = _stream.Tables.GetEntryFor(
                    interfaceImplRow.Interface.Table,
                    interfaceImplRow.Interface.Index);

                TypeDef implementingClass = _map.GetDefinition(MetadataTables.TypeDef, implementingClassRow) as TypeDef;
                TypeRef implementedClass = _map.GetDefinition(interfaceImplRow.Interface.Table, interfaceRow) as TypeRef;
                if(implementedClass is TypeSpec)
                {
                    ((TypeSpec)implementedClass).ImplementingType = implementingClass;
                }
                implementingClass.Implements.Add(_map.GetDefinition(interfaceImplRow.Interface.Table, interfaceRow) as TypeRef);
            }
        }

        private void LoadNestedClassMetadata()
        {
            if(!_stream.Tables.ContainsKey(MetadataTables.NestedClass))
                return;

            MetadataRow[] nestedClasses = _stream.Tables[MetadataTables.NestedClass];
            for(int i = 0; i < nestedClasses.Length; i++)
            {
                NestedClassMetadataTableRow nestedClassRow = nestedClasses[i] as NestedClassMetadataTableRow;
                TypeDefMetadataTableRow nestedClass = _stream.Tables.GetEntryFor(
                    MetadataTables.TypeDef, nestedClassRow.NestedClass
                    ) as TypeDefMetadataTableRow;

                TypeDef container = _map.GetDefinition(MetadataTables.TypeDef, _stream.Tables.GetEntryFor(
                    MetadataTables.TypeDef, nestedClassRow.EnclosingClass
                    )) as TypeDef;
                TypeDef nested = _map.GetDefinition(MetadataTables.TypeDef, nestedClass) as TypeDef;

                _assembly.Map.Remove(nested); // remove type originally added when loading typedef records

                nested.ContainingClass = container;
                _assembly.Map.Add(nested);
            }
        }

        private void LoadTypeSpecMetadata()
        {
            if(!_stream.Tables.ContainsKey(MetadataTables.TypeSpec))
                return;

            foreach(TypeSpecMetadataTableRow typeSpecRow in _stream.Tables[MetadataTables.TypeSpec])
            {
                TypeSpec typeRef = new TypeSpec(_assembly, typeSpecRow.Signiture);
                _map.Add(MetadataTables.TypeSpec, typeSpecRow, typeRef);
            }
        }

        private void LoadMemberRefMetadata()
        {
            if(!_stream.Tables.ContainsKey(MetadataTables.MemberRef))
                return;

            int count = _stream.Tables[MetadataTables.MemberRef].Length;
            for(int i = 0; i < count; i++)
            {
                MemberRefMetadataTableRow memberRefRow = _stream.Tables[MetadataTables.MemberRef][i] as MemberRefMetadataTableRow;
                MemberRef memberRef = MemberRef.CreateFromMetadata(_assembly, _metadata, memberRefRow);
                _map.Add(MetadataTables.MemberRef, memberRefRow, memberRef);
            }
        }

        private void LoadTypeDefMetadata()
        {
            MetadataRow[] table = _stream.Tables[MetadataTables.TypeDef];
            int count = table.Length;

            for(int i = 0; i < count; i++)
            {
                TypeDefMetadataTableRow typeDefRow = table[i] as TypeDefMetadataTableRow;
                TypeDef type = TypeDef.CreateFromMetadata(_references, typeDefRow);

                _map.Add(MetadataTables.TypeDef, typeDefRow, type);
                _assembly.Map.Add(type);
                _assembly.Types.Add(type);
            }
        }

        private void LoadTypeRefMetadata()
        {
            if(!_stream.Tables.ContainsKey(MetadataTables.TypeRef))
                return;

            MetadataRow[] table = _stream.Tables[MetadataTables.TypeRef];
            int count = table.Length;

            for(int i = 0; i < count; i++)
            {
                TypeRefMetadataTableRow row = table[i] as TypeRefMetadataTableRow;

                TypeRef typeRef = new TypeRef(
                    _assembly,
                    _assembly.StringStream.GetString(row.Name.Value),
                    _assembly.StringStream.GetString(row.Namespace.Value),
                    row.ResolutionScope
                    );

                _map.Add(MetadataTables.TypeRef, row, typeRef);
            }
        }

        private void LoadModuleMetadata()
        {
            MetadataRow[] table = _stream.Tables[MetadataTables.Module];
            int count = table.Length;

            for(int i = 0; i < count; i++)
            {
                ModuleMetadataTableRow row = table[i] as ModuleMetadataTableRow;

                GuidStream stream = _references.Metadata.Streams[Streams.GuidStream] as GuidStream;

                ModuleDef module = new ModuleDef(
                    _assembly.StringStream.GetString(row.Name.Value),
                    _assembly,
                    stream.GetGuid(row.Mvid)
                    );

                _map.Add(MetadataTables.Module, row, module);
                _assembly.Modules.Add(module);
            }
        }

        private void LoadAssemblyRefMetadata()
        {
            if(!_stream.Tables.ContainsKey(MetadataTables.AssemblyRef))
                return;

            MetadataRow[] items = _stream.Tables[MetadataTables.AssemblyRef];
            for(int i = 0; i < items.Length; i++)
            {
                AssemblyRefMetadataTableRow assemblyRefRow = items[i] as AssemblyRefMetadataTableRow;
                AssemblyRef assemblyRef = AssemblyRef.CreateFromMetadata(_references, assemblyRefRow);

                _map.Add(MetadataTables.AssemblyRef, assemblyRefRow, assemblyRef);
                _assembly.ReferencedAssemblies.Add(assemblyRef);
            }
        }

        private void LoadAssemblyMetadata()
        {
            if(!_stream.Tables.ContainsKey(MetadataTables.Assembly))
                return;

            // Always one and only
            AssemblyMetadataTableRow assemblyRow = _stream.Tables[MetadataTables.Assembly][0] as AssemblyMetadataTableRow;

            _assembly.Name = _references.Assembly.StringStream.GetString(assemblyRow.Name.Value);
            _assembly.Version = assemblyRow.GetVersion();
        }
    }
}
