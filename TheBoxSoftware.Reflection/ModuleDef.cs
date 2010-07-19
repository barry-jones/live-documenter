using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection{
	using TheBoxSoftware.Reflection.Core;
	using TheBoxSoftware.Reflection.Core.COFF;

	/// <summary>
	/// Provides a definition for a module in an assembly.
	/// </summary>
	public class ModuleDef : ReflectedMember {
		#region Methods
		/// <summary>
		/// Instantiates a module from the specified row in the metadata
		/// </summary>
		/// <param name="assembly">The assembly that contains and defines the module.</param>
		/// <param name="metadataDirectory">The metadata directory</param>
		/// <param name="row">The row to instantiate</param>
		/// <returns>The instantiated module</returns>
		internal static ModuleDef CreateFromMetadata(AssemblyDef assembly, 
													MetadataDirectory metadataDirectory,
													ModuleMetadataTableRow row) {
			ModuleDef module = new ModuleDef();
			module.Name = assembly.StringStream.GetString(row.Name.Value);
			module.UniqueId = assembly.GetUniqueId();
			module.ModuleVersionId = ((GuidStream)metadataDirectory.Streams[Streams.GuidStream]).GetGuid(row.Mvid);
			module.Assembly = assembly;
			return module;
		}
		#endregion

		#region Properties
		/// <summary>
		/// The name of the module
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The GUID that represents the Modules version identifier.
		/// </summary>
		public Guid ModuleVersionId { get; set; }
		#endregion
	}
}
