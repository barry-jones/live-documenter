
namespace TheBoxSoftware.Reflection
{
    using System;
    using Core.COFF;

    /// <summary>
    /// Provides a definition for a module in an assembly.
    /// </summary>
    public class ModuleDef : ReflectedMember
    {
        /// <summary>
        /// Instantiates a module from the specified row in the metadata
        /// </summary>
        /// <param name="references">Container of all the required references to build the type.</param>
        /// <param name="row">The row to instantiate</param>
        /// <returns>The instantiated module</returns>
        internal static ModuleDef CreateFromMetadata(BuildReferences references, ModuleMetadataTableRow row)
        {
            GuidStream stream = references.Metadata.Streams[Streams.GuidStream] as GuidStream;
            ModuleDef module = new ModuleDef();

            module.Name = references.Assembly.StringStream.GetString(row.Name.Value);
            module.UniqueId = references.Assembly.CreateUniqueId();
            module.ModuleVersionId = stream.GetGuid(row.Mvid);
            module.Assembly = references.Assembly;

            return module;
        }

        /// <summary>
        /// The GUID that represents the Modules version identifier.
        /// </summary>
        public Guid ModuleVersionId { get; set; }
    }
}