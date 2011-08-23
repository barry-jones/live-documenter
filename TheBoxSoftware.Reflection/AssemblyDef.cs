using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection {
	using TheBoxSoftware.Reflection.Core.COFF;
	using TheBoxSoftware.Reflection.Core.PE;
	using TheBoxSoftware.Reflection.Core;

	/// <summary>
	/// The AssemblyDef provides the top level information and entry point to
	/// all types, methods etc reflected from a .NET executable.
	/// </summary>
	/// <remarks>
	/// <para>The AssemblyDef is the starting point for obtaining reflected information
	/// about a .NET assembly. This information is obtained by parsing and discerning
	/// information about <see cref="TypeDef"/>s, <see cref="MethodDef"/>s etc from
	/// the .NET metadata stored in the <see cref="PeCoffFile"/>.</para>
	/// <para>The assembly implements a mechanism for generating unique identifiers
	/// that can be assigned to each of the elements reflected in this assembly. The
	/// unique identifier is not really required but can help other applications to
	/// store keys and find reflected elements more quickly and uses less memory than
	/// string based unique identifiers.</para>
	/// <example>
	/// <code>
	/// // Instantiate from a full file path and name
	/// AssemblyDef assembly = AssemblyDef.Create(myAssemblyPath);
	/// 
	/// // Instantiate from an already existing loaded metadata file
	/// PeCoffFile peCoffFile = new PeCoffFile(myAssemblyPath);
	/// AssemblyDef assembly = AssemblyDef.Create(peCoffFile);
	/// </code>
	/// </example>
	/// </remarks>
	/// <seealso cref="PeCoffFile"/>
	public class AssemblyDef : ReflectedMember {
		/// <summary>
		/// Counter to generate unique identifiers for each element that is reflected
		/// in this assembly.
		/// </summary>
		private int uniqueIdCounter;
		private TypeInNamespaceMap namspaceMap;

		/// <summary>
		/// Gets or sets a reference to the string stream.
		/// </summary>
		/// <remarks>
		/// This has been created to reduce the cost of obtaining this information, it is
		/// a well access field and storing it behind a property just increases cost for no
		/// reason.
		/// </remarks>
		public StringStream StringStream;

		#region Methods
		/// <summary>
		/// Creates and instantiates an AssemblyDef based on the provided library name.
		/// </summary>
		/// <param name="fileName">The file name of the assembly to reflect.</param>
		/// <returns>The instantiated AssemblyDef.</returns>
		/// <exception cref="ArgumentNullException">The filename was null or empty.</exception>
		/// <exception cref="NotAManagedLibraryException">
		/// Thrown when a PeCoff file is passed to the function and the <paramref name="peCoffFile"/>
		/// does not contain a <see cref="DataDirectories.CommonLanguageRuntimeHeader"/>.
		/// </exception>
		public static AssemblyDef Create(string fileName) {
			if (string.IsNullOrEmpty(fileName))
				throw new ArgumentNullException(fileName);

			// [#102] check for CLR directory to make sure it is a managed PE file
			PeCoffFile peFile = new PeCoffFile(fileName);

			if (!peFile.Directories.ContainsKey(DataDirectories.CommonLanguageRuntimeHeader)) {
				peFile = null;	// would be nice to get the memory back
				throw new NotAManagedLibraryException(string.Format("The file '{0}' is not a managed library.", fileName));
			}

			return AssemblyDef.Create(peFile);
		}

		/// <summary>
		/// Initialises and instantiates an AssemblyDef instance for the provided
		/// <see cref="PeCoffFile"/> (assembly).
		/// </summary>
		/// <param name="peCoffFile">The PeCoffFile to load the AssemblyDef from.</param>
		/// <returns>The instantiated AssemblyDef.</returns>
		/// <exception cref="ArgumentNullException">Thrown when the PeCoffFile is null.</exception>
		/// <exception cref="NotAManagedLibraryException">
		/// Thrown when a PeCoff file is passed to the function and the <paramref name="peCoffFile"/>
		/// does not contain a <see cref="DataDirectories.CommonLanguageRuntimeHeader"/>.
		/// </exception>
		public static AssemblyDef Create(PeCoffFile peCoffFile)
		{
			if (peCoffFile == null)
				throw new ArgumentNullException("peCoffFile");

			if (!peCoffFile.Directories.ContainsKey(DataDirectories.CommonLanguageRuntimeHeader))
			{
				peCoffFile = null;	// would be nice to get the memory back
				throw new NotAManagedLibraryException(string.Format("The file '{0}' is not a managed library.", peCoffFile.FileName));
			}

			AssemblyDef assembly = new AssemblyDef();
			assembly.Modules = new List<ModuleDef>();
			assembly.Types = new List<TypeDef>();
			assembly.ReferencedAssemblies = new List<AssemblyRef>();
			assembly.namspaceMap = new TypeInNamespaceMap(assembly);
			assembly.Assembly = assembly;

			// Store the details of the file
			assembly.File = peCoffFile;
			MetadataToDefinitionMap map = assembly.File.Map;
			map.Assembly = assembly;

			// Read the metadata from the file and populate the entries
			MetadataDirectory metadata = assembly.File.GetMetadataDirectory();
			MetadataStream metadataStream = (MetadataStream)metadata.Streams[Streams.MetadataStream];
			int count = 0;

			//
			assembly.StringStream = (StringStream)metadata.Streams[Streams.StringStream];

			if (metadataStream.Tables.ContainsKey(MetadataTables.Assembly)) {
				// Always one and only
				AssemblyMetadataTableRow assemblyRow = (AssemblyMetadataTableRow)metadataStream.Tables[MetadataTables.Assembly][0];
				assembly.Name = assembly.StringStream.GetString(assemblyRow.Name.Value);
				assembly.Version = assemblyRow.GetVersion();
			}

			if (metadataStream.Tables.ContainsKey(MetadataTables.AssemblyRef)) {
				MetadataRow[] items = metadataStream.Tables[MetadataTables.AssemblyRef];
				for (int i = 0; i < items.Length; i++) {
					AssemblyRefMetadataTableRow assemblyRefRow = items[i] as AssemblyRefMetadataTableRow;
					AssemblyRef assemblyRef = AssemblyRef.CreateFromMetadata(assembly, metadata, assemblyRefRow);
					map.Add(MetadataTables.AssemblyRef, assemblyRefRow, assemblyRef);
					assembly.ReferencedAssemblies.Add(assemblyRef);
				}
			}

			foreach (ModuleMetadataTableRow moduleRow in metadataStream.Tables[MetadataTables.Module]) {
				ModuleDef module = ModuleDef.CreateFromMetadata(assembly, metadata, moduleRow);
				map.Add(MetadataTables.Module, moduleRow, module);
				assembly.Modules.Add(module);
			}

			if (metadataStream.Tables.ContainsKey(MetadataTables.TypeRef)) {
				foreach (TypeRefMetadataTableRow typeRefRow in metadataStream.Tables[MetadataTables.TypeRef]) {
					TypeRef typeRef = TypeRef.CreateFromMetadata(assembly, metadata, typeRefRow);
					map.Add(MetadataTables.TypeRef, typeRefRow, typeRef);
				}
			}

			count = metadataStream.Tables[MetadataTables.TypeDef].Length;
			for (int i = 0; i < count; i++) {
				TypeDefMetadataTableRow typeDefRow = (TypeDefMetadataTableRow)metadataStream.Tables[MetadataTables.TypeDef][i];
				TypeDef type = TypeDef.CreateFromMetadata(assembly, metadata, typeDefRow);
				map.Add(MetadataTables.TypeDef, typeDefRow, type);
				assembly.namspaceMap.Add(type.Namespace, MetadataTables.TypeDef, typeDefRow);
				assembly.Types.Add(type);
			}

			if (metadataStream.Tables.ContainsKey(MetadataTables.MemberRef)) {
				count = metadataStream.Tables[MetadataTables.MemberRef].Length;
				for (int i = 0; i < count; i++) {
					MemberRefMetadataTableRow memberRefRow = (MemberRefMetadataTableRow)metadataStream.Tables[MetadataTables.MemberRef][i];
					MemberRef memberRef = MemberRef.CreateFromMetadata(assembly, metadata, memberRefRow);
					map.Add(MetadataTables.MemberRef, memberRefRow, memberRef);
				}
			}

			if (metadataStream.Tables.ContainsKey(MetadataTables.TypeSpec)) {
				foreach (TypeSpecMetadataTableRow typeSpecRow in metadataStream.Tables[MetadataTables.TypeSpec]) {
					TypeSpec typeRef = TypeSpec.CreateFromMetadata(assembly, metadata, typeSpecRow);
					map.Add(MetadataTables.TypeSpec, typeSpecRow, typeRef);
				}
			}

			// Sort out the nested classes
			if (metadataStream.Tables.ContainsKey(MetadataTables.NestedClass)) {
				MetadataRow[] nestedClasses = metadataStream.Tables[MetadataTables.NestedClass];
				for (int i = 0; i < nestedClasses.Length; i++) {
					NestedClassMetadataTableRow nestedClassRow = nestedClasses[i] as NestedClassMetadataTableRow;
					TypeDefMetadataTableRow nestedClass = (TypeDefMetadataTableRow)metadataStream.Tables.GetEntryFor(
						MetadataTables.TypeDef, nestedClassRow.NestedClass
						);
					TypeDef container = (TypeDef)map.GetDefinition(MetadataTables.TypeDef, metadataStream.Tables.GetEntryFor(
						MetadataTables.TypeDef, nestedClassRow.EnclosingClass
						));
					TypeDef nested = (TypeDef)map.GetDefinition(MetadataTables.TypeDef, nestedClass);
					nested.ContainingClass = container;
					assembly.namspaceMap.Add(nested.Namespace, MetadataTables.TypeDef, nestedClass);
				}
			}

			// Associate the interface references types implement with there types
			if (metadataStream.Tables.ContainsKey(MetadataTables.InterfaceImpl)) {
				MetadataRow[] interfaceImplementations = metadataStream.Tables[MetadataTables.InterfaceImpl];
				for (int i = 0; i < interfaceImplementations.Length; i++) {
					InterfaceImplMetadataTableRow interfaceImplRow = interfaceImplementations[i] as InterfaceImplMetadataTableRow;
					TypeDefMetadataTableRow implementingClassRow = (TypeDefMetadataTableRow)metadataStream.Tables.GetEntryFor(
						MetadataTables.TypeDef, interfaceImplRow.Class
						);
					MetadataRow interfaceRow = metadataStream.Tables.GetEntryFor(
						interfaceImplRow.Interface.Table,
						interfaceImplRow.Interface.Index);

					TypeDef implementingClass = (TypeDef)map.GetDefinition(MetadataTables.TypeDef, implementingClassRow);
					TypeRef implementedClass = (TypeRef)map.GetDefinition(interfaceImplRow.Interface.Table, interfaceRow);
					if (implementedClass is TypeSpec) {
						((TypeSpec)implementedClass).ImplementingType = implementingClass;
					}
					implementingClass.Implements.Add((TypeRef)map.GetDefinition(interfaceImplRow.Interface.Table, interfaceRow));
				}
			}

			if (metadataStream.Tables.ContainsKey(MetadataTables.Constant)) {
				MetadataRow[] constants = metadataStream.Tables[MetadataTables.Constant];
				for (int i = 0; i < constants.Length; i++) {
					ConstantMetadataTableRow constantRow = constants[i] as ConstantMetadataTableRow;
					ConstantInfo constant = ConstantInfo.CreateFromMetadata(assembly, metadataStream, constantRow);
					switch (constantRow.Parent.Table) {
						case MetadataTables.Field:
							FieldDef field = (FieldDef)map.GetDefinition(MetadataTables.Field,
								metadataStream.GetEntryFor(MetadataTables.Field, constantRow.Parent.Index)
								);
							field.Constants.Add(constant);
							break;
						case MetadataTables.Property:
							break;
						case MetadataTables.Param:
							break;
					}
				}
			}

			if (metadataStream.Tables.ContainsKey(MetadataTables.CustomAttribute)) {
				MetadataRow[] customAttributes = metadataStream.Tables[MetadataTables.CustomAttribute];
				for (int i = 0; i < customAttributes.Length; i++) {
					CustomAttributeMetadataTableRow customAttributeRow = customAttributes[i] as CustomAttributeMetadataTableRow;

					ReflectedMember attributeTo = map.GetDefinition(customAttributeRow.Parent.Table,
						metadataStream.GetEntryFor(customAttributeRow.Parent)
						);
					MemberRef ofType = (MemberRef)map.GetDefinition(customAttributeRow.Type.Table,
						metadataStream.GetEntryFor(customAttributeRow.Type)
						);

					if (attributeTo != null) {
						CustomAttribute attribute = new CustomAttribute(ofType);
						attributeTo.Attributes.Add(attribute);

						if (ofType.Type.Name == "ExtensionAttribute" && attributeTo is MethodDef) {
							MethodDef extensionMethod = attributeTo as MethodDef;
							if (extensionMethod != null) {
								TypeDef def = (TypeDef)extensionMethod.Parameters[0].GetTypeRef();
								def.ExtensionMethods.Add(extensionMethod);
							}
						}
					}
				}
			}

			return assembly;
		}

		/// <summary>
		/// Returns all the types in their respective namespaces.
		/// </summary>
		/// <returns>A dictionary of namespaces and its containing types</returns>
		public Dictionary<string, List<TypeDef>> GetTypesInNamespaces() {
			// REVIEW: see this.namespaceMap. Also appears to be wasteful
			List<string> orderedNamespaces = new List<string>();
			Dictionary<string, List<TypeDef>> temp = new Dictionary<string, List<TypeDef>>();
			foreach (TypeDef current in this.Types) {
				if (!orderedNamespaces.Contains(current.Namespace)) {
					orderedNamespaces.Add(current.Namespace);
				}
			}
			orderedNamespaces.Sort();
			foreach (string current in orderedNamespaces) {
				temp.Add(current, new List<TypeDef>());
			}
			foreach (TypeDef current in this.Types) {
				temp[current.Namespace].Add(current);
			}
			return temp;
		}

		/// <summary>
		/// Checks if this assembly defines the namespace specified.
		/// </summary>
		/// <param name="theNamespace">The namespace to check.</param>
		/// <returns>True if yes else false.</returns>
		/// <remarks>
		/// When using this make sure that you are aware that more than one assembly
		/// can specifiy the same namespace.
		/// </remarks>
		public bool IsNamespaceDefined(string theNamespace) {
			return this.namspaceMap.ContainsNamespace(theNamespace);
		}

		/// <summary>
		/// Obtains a collection of all the namespaces defined in this assembly.
		/// </summary>
		/// <returns>The collection of strings representing the namespaces.</returns>
		public List<string> GetNamespaces() {
			return this.namspaceMap.GetAllNamespaces();
		}

		/// <summary>
		/// Searches the assembly for the named type in the specified assembly.
		/// </summary>
		/// <param name="theNamespace">The namespace to search for the type in.</param>
		/// <param name="theTypeName">The name of the type</param>
		/// <returns>The resolved type definition or null if not found.</returns>
		public TypeDef FindType(string theNamespace, string theTypeName) {
			return this.namspaceMap.FindTypeInNamespace(theNamespace, theTypeName);
		}

		/// <summary>
		/// Get the next available unique identifier for this assembly.
		/// </summary>
		/// <returns>The unique identifier</returns>
		internal int GetUniqueId() {
			return this.uniqueIdCounter++;
		}

		/// <summary>
		/// Helps to resolve tokens from the metadata to there associated types and elements inside
		/// this assembly.
		/// </summary>
		/// <param name="metadataToken">The metadata token to resolve</param>
		/// <returns>A resolved token reference or null if not found in this assembly.</returns>
		/// <remarks>
		/// A token is specific to an assembly.
		/// </remarks>
		public ReflectedMember ResolveMetadataToken(int metadataToken) {
			MetadataToDefinitionMap map = this.File.Map;
			Core.COFF.MetadataStream metadataStream = this.File.GetMetadataDirectory().GetMetadataStream();

			// Get the details in the token
			ILMetadataToken token = (ILMetadataToken)(metadataToken & 0xff000000);
			int index = metadataToken & 0x00ffffff;

			ReflectedMember returnItem = null;

			// 
			switch (token) {
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

		/// <summary>
		/// Resolves a coded index to its instantiated reference
		/// </summary>
		/// <param name="index">The coded index to resolve</param>
		/// <returns>Null if not resolved else the object referenced by the index</returns>
		public object ResolveCodedIndex(CodedIndex index) {
			object resolvedReference = null;

			MetadataDirectory metadata = this.File.GetMetadataDirectory();
			MetadataStream metadataStream = (MetadataStream)metadata.Streams[Streams.MetadataStream];
			if (metadataStream.Tables.ContainsKey(index.Table)) {
				if (metadataStream.Tables[index.Table].Length + 1 > index.Index) {
					MetadataToDefinitionMap map = this.File.Map;
					MetadataRow metadataRow = metadataStream.GetEntryFor(index);
					resolvedReference = map.GetDefinition(index.Table, metadataRow);
				}
			}

			return resolvedReference;
		} 
		#endregion

		#region Properties
		public List<AssemblyRef> ReferencedAssemblies { get; set; }

		/// <summary>
		/// The list of <see cref="ModuleDef"/>s in this assembly.
		/// </summary>
		public List<ModuleDef> Modules { get; set; }

		/// <summary>
		/// The list of <see cref="TypeDef"/>s in this assembly.
		/// </summary>
		public List<TypeDef> Types { get; set; }

		/// <summary>
		/// The <see cref="PeCoffFile"/> the assembly was reflected from.
		/// </summary>
		public TheBoxSoftware.Reflection.Core.PeCoffFile File { get; set; }

		/// <summary>
		/// The name of the assembly.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The version details for this assembly.
		/// </summary>
		public Version Version { get; set; }
		#endregion
	}
}