
namespace TheBoxSoftware.Reflection
{
    using System;
    using Core;
    using Core.COFF;
    using Core.PE;

    internal class AssemblyDefBuilder
    {
        private readonly PeCoffFile _peCoffFile;

        public AssemblyDefBuilder(PeCoffFile peCoffFile)
        {
            if(peCoffFile == null)
                throw new ArgumentNullException(nameof(peCoffFile));
            if(!peCoffFile.Directories.ContainsKey(DataDirectories.CommonLanguageRuntimeHeader))
                throw new NotAManagedLibraryException($"The file '{peCoffFile.FileName}' is not a managed library.");

            _peCoffFile = peCoffFile;
        }

        public AssemblyDef Build()
        {
            AssemblyDef assembly = new AssemblyDef();
            assembly.File = _peCoffFile;
            MetadataToDefinitionMap map = _peCoffFile.Map;
            map.Assembly = assembly;

            // Read the metadata from the file and populate the entries
            MetadataDirectory metadata = _peCoffFile.GetMetadataDirectory();
            MetadataStream metadataStream = (MetadataStream)metadata.Streams[Streams.MetadataStream];

            assembly.StringStream = (IStringStream)metadata.Streams[Streams.StringStream]; // needs to be populated first
            LoadAssemblyMetadata(assembly, metadataStream);
            LoadAssemblyRefMetadata(assembly, map, metadata, metadataStream);
            LoadModuleMetadata(assembly, map, metadata, metadataStream);
            LoadTypeRefMetadata(assembly, map, metadata, metadataStream);
            LoadTypeDefMetadata(assembly, map, metadata, metadataStream);
            LoadMemberRefMetadata(assembly, map, metadata, metadataStream);
            LoadTypeSpecMetadata(assembly, map, metadata, metadataStream);
            LoadNestedClassMetadata(assembly, map, metadataStream);
            LoadInterfaceImplMetadata(map, metadataStream);
            LoadConstantMetadata(assembly, map, metadataStream);
            LoadCustomAttributeMetadata(map, metadataStream);

            return assembly;
        }

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

        private void LoadConstantMetadata(AssemblyDef assembly, MetadataToDefinitionMap map, MetadataStream metadataStream)
        {
            if(!metadataStream.Tables.ContainsKey(MetadataTables.Constant))
                return;

            MetadataRow[] constants = metadataStream.Tables[MetadataTables.Constant];
            for(int i = 0; i < constants.Length; i++)
            {
                ConstantMetadataTableRow constantRow = constants[i] as ConstantMetadataTableRow;
                ConstantInfo constant = ConstantInfo.CreateFromMetadata(assembly, metadataStream, constantRow);

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

        private void LoadNestedClassMetadata(AssemblyDef assembly, MetadataToDefinitionMap map, MetadataStream metadataStream)
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
                assembly.Map.Add(nested);
            }
        }

        private void LoadTypeSpecMetadata(AssemblyDef assembly, MetadataToDefinitionMap map, MetadataDirectory metadata, MetadataStream metadataStream)
        {
            if(!metadataStream.Tables.ContainsKey(MetadataTables.TypeSpec))
                return;

            foreach(TypeSpecMetadataTableRow typeSpecRow in metadataStream.Tables[MetadataTables.TypeSpec])
            {
                TypeSpec typeRef = TypeSpec.CreateFromMetadata(assembly, metadata, typeSpecRow);
                map.Add(MetadataTables.TypeSpec, typeSpecRow, typeRef);
            }
        }

        private void LoadMemberRefMetadata(AssemblyDef assembly, MetadataToDefinitionMap map, MetadataDirectory metadata, MetadataStream metadataStream)
        {
            if(!metadataStream.Tables.ContainsKey(MetadataTables.MemberRef))
                return;

            int count = metadataStream.Tables[MetadataTables.MemberRef].Length;
            for(int i = 0; i < count; i++)
            {
                MemberRefMetadataTableRow memberRefRow = (MemberRefMetadataTableRow)metadataStream.Tables[MetadataTables.MemberRef][i];
                MemberRef memberRef = MemberRef.CreateFromMetadata(assembly, metadata, memberRefRow);
                map.Add(MetadataTables.MemberRef, memberRefRow, memberRef);
            }
        }

        private void LoadTypeDefMetadata(AssemblyDef assembly, MetadataToDefinitionMap map, MetadataDirectory metadata, MetadataStream metadataStream)
        {
            int count = metadataStream.Tables[MetadataTables.TypeDef].Length;
            for(int i = 0; i < count; i++)
            {
                TypeDefMetadataTableRow typeDefRow = (TypeDefMetadataTableRow)metadataStream.Tables[MetadataTables.TypeDef][i];
                TypeDef type = TypeDef.CreateFromMetadata(assembly, metadata, typeDefRow);
                map.Add(MetadataTables.TypeDef, typeDefRow, type);
                assembly.Map.Add(type);
                assembly.Types.Add(type);
            }
        }

        private void LoadTypeRefMetadata(AssemblyDef assembly, MetadataToDefinitionMap map, MetadataDirectory metadata, MetadataStream metadataStream)
        {
            if(!metadataStream.Tables.ContainsKey(MetadataTables.TypeRef))
                return;

            foreach(TypeRefMetadataTableRow typeRefRow in metadataStream.Tables[MetadataTables.TypeRef])
            {
                TypeRef typeRef = TypeRef.CreateFromMetadata(assembly, metadata, typeRefRow);
                map.Add(MetadataTables.TypeRef, typeRefRow, typeRef);
            }
        }

        private void LoadModuleMetadata(AssemblyDef assembly, MetadataToDefinitionMap map, MetadataDirectory metadata, MetadataStream metadataStream)
        {
            foreach(ModuleMetadataTableRow moduleRow in metadataStream.Tables[MetadataTables.Module])
            {
                ModuleDef module = ModuleDef.CreateFromMetadata(assembly, metadata, moduleRow);
                map.Add(MetadataTables.Module, moduleRow, module);
                assembly.Modules.Add(module);
            }
        }

        private void LoadAssemblyRefMetadata(AssemblyDef assembly, MetadataToDefinitionMap map, MetadataDirectory metadata, MetadataStream metadataStream)
        {
            if(!metadataStream.Tables.ContainsKey(MetadataTables.AssemblyRef))
                return;

            MetadataRow[] items = metadataStream.Tables[MetadataTables.AssemblyRef];
            for(int i = 0; i < items.Length; i++)
            {
                AssemblyRefMetadataTableRow assemblyRefRow = items[i] as AssemblyRefMetadataTableRow;
                AssemblyRef assemblyRef = AssemblyRef.CreateFromMetadata(assembly, assemblyRefRow);
                map.Add(MetadataTables.AssemblyRef, assemblyRefRow, assemblyRef);
                assembly.ReferencedAssemblies.Add(assemblyRef);
            }
        }

        private void LoadAssemblyMetadata(AssemblyDef assembly, MetadataStream metadataStream)
        {
            if(!metadataStream.Tables.ContainsKey(MetadataTables.Assembly))
                return;

            // Always one and only
            AssemblyMetadataTableRow assemblyRow = (AssemblyMetadataTableRow)metadataStream.Tables[MetadataTables.Assembly][0];
            assembly.Name = assembly.StringStream.GetString(assemblyRow.Name.Value);
            assembly.Version = assemblyRow.GetVersion();
        }
    }
}
