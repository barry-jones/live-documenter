using System;
using TheBoxSoftware.Reflection.Core.COFF;

namespace TheBoxSoftware.Reflection
{
    /// <summary>
    /// Provides a definition for a module in an assembly.
    /// </summary>
    public class ModuleDef : ReflectedMember
    {
        /// <summary>
        /// Instantiates a module from the specified row in the metadata
        /// </summary>
        /// <param name="assembly">The assembly that contains and defines the module.</param>
        /// <param name="metadataDirectory">The metadata directory</param>
        /// <param name="row">The row to instantiate</param>
        /// <returns>The instantiated module</returns>
        internal static ModuleDef CreateFromMetadata(AssemblyDef assembly,
                                                    MetadataDirectory metadataDirectory,
                                                    ModuleMetadataTableRow row)
        {
            ModuleDef module = new ModuleDef();
            module.Name = assembly.StringStream.GetString(row.Name.Value);
            module.UniqueId = assembly.CreateUniqueId();
            module.ModuleVersionId = ((GuidStream)metadataDirectory.Streams[Streams.GuidStream]).GetGuid(row.Mvid);
            module.Assembly = assembly;
            return module;
        }

        /// <summary>
        /// The GUID that represents the Modules version identifier.
        /// </summary>
        public Guid ModuleVersionId { get; set; }
    }
}