using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter {
	using TheBoxSoftware.Reflection;
	using TheBoxSoftware.Reflection.Comments;

	/// <summary>
	/// Class that contains helper methods for this application
	/// </summary>
	internal static class Helper {
		/// <summary>
		/// Obtains a key that uniquely identifies the member in the library, for all libraries
		/// loaded in to the documenter.
		/// </summary>
		/// <param name="assembly">The assembly</param>
		/// <param name="member">The member</param>
		/// <returns>A long that is unique in the application</returns>
		public static long GetUniqueKey(AssemblyDef assembly, ReflectedMember member) {
			long id = ((long)assembly.UniqueId) << 32;
			id += member.UniqueId;
			return id;
		}

		/// <summary>
		/// Obtains a key that uniquely identifies the assembly in the library, for all libraries
		/// and members loaded in to the documenter.
		/// </summary>
		/// <param name="assembly">The assembly to get the unique identifier for</param>
		/// <returns>A long that is unique in the application</returns>
		public static long GetUniqueKey(AssemblyDef assembly) {
			return ((long)assembly.UniqueId) << 32;
		}

		internal static EntryKey ResolveTypeAndGetUniqueIdFromCref(CrefEntryKey type) {
			// TODO: IMplement assembly reference links to external types, gettype and then
			// check if assembly is loaded - goto entry (maybe ask to load)
			EntryKey resolvedKey = new EntryKey(0);
			TypeDef resolvedType = null;
			CRefPath crefPath = CRefPath.Parse(type.CRef);

			if (crefPath.PathType == CRefTypes.Namespace) {
				return new EntryKey(Helper.GetUniqueKey(type.Assembly), crefPath.Namespace);
			}
			else {
				// Check if any of the referenced namespaces have been loaded by the documentation
				DocumentedAssembly found = null;
				// First check if this assembly contains the definition for the type, cref entries could point
				// to our assembly.
				if (type.Assembly.IsNamespaceDefined(crefPath.Namespace)) {
					resolvedType = type.Assembly.FindType(crefPath.Namespace, crefPath.TypeName);
				}
				// If not found check each of the loaded and referenced assemblies
				if (resolvedType == null) {
					foreach (AssemblyRef assemblyRef in type.Assembly.ReferencedAssemblies) {
						foreach (DocumentedAssembly documentedAssembly in LiveDocumentorFile.Singleton.Files) {
							if (documentedAssembly.Name == assemblyRef.Name) {
								// This assembly ref is loaded and referenced
								if (documentedAssembly.LoadedAssembly.IsNamespaceDefined(crefPath.Namespace)) {
									resolvedType = documentedAssembly.LoadedAssembly.FindType(crefPath.Namespace, crefPath.TypeName);
								}
								if (resolvedType != null) {
									break;
								}
								found = documentedAssembly;
								break;
							}
						}
					}
				}

				if (resolvedType != null) {
					switch (crefPath.PathType) {
						case CRefTypes.Type:
							resolvedKey = new EntryKey(Helper.GetUniqueKey(resolvedType.Assembly, resolvedType));
							break;
						case CRefTypes.Field:
							resolvedKey = new EntryKey(Helper.GetUniqueKey(resolvedType.Assembly, resolvedType.FindFieldByName(crefPath.ElementName)));
							break;
						case CRefTypes.Property:
							resolvedKey = new EntryKey(Helper.GetUniqueKey(resolvedType.Assembly, resolvedType.FindPropertyByName(crefPath.ElementName)));
							break;
						case CRefTypes.Method:
							resolvedKey = new EntryKey(Helper.GetUniqueKey(resolvedType.Assembly, resolvedType.FindMethodByName(crefPath.ElementName)));
							break;
					}
				}
				else {
					// Consider allowing references to be loaded
				}
			}

			return resolvedKey;
		}
	}
}