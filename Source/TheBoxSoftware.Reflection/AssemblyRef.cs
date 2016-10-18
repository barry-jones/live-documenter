using System;
using TheBoxSoftware.Reflection.Core.COFF;

namespace TheBoxSoftware.Reflection
{
    /// <summary>
    /// Represents a reference to an external library.
    /// </summary>
    public sealed class AssemblyRef : ReflectedMember
    {
        /// <summary>
        /// Initialises a new instance of the AssemblyRef class from the provided details.
        /// </summary>
        /// <param name="assembly">The assembly this reference is made in.</param>
        /// <param name="metadata">The metadata details for the assembly.</param>
        /// <param name="row">The row that provides the assembly reference details.</param>
        /// <returns>A populated AssemblyRef instance.</returns>
        public static AssemblyRef CreateFromMetadata(AssemblyDef assembly, MetadataDirectory metadata, AssemblyRefMetadataTableRow row)
        {
            AssemblyRef assemblyRef = new AssemblyRef();

            assemblyRef.Version = new Version(
                row.MajorVersion,
                row.MinorVersion,
                row.BuildNumber,
                row.RevisionNumber);
            assemblyRef.Culture = assembly.StringStream.GetString(row.Culture.Value);
            assemblyRef.UniqueId = assembly.CreateUniqueId();
            assemblyRef.Name = assembly.StringStream.GetString(row.Name.Value);
            assemblyRef.Assembly = assembly;

            return assemblyRef;
        }

        /// <summary>
        /// The full version details of the referenced assembly.
        /// </summary>
        public Version Version { get; set; }

        /// <summary>
        /// The string representing the culture of the assembly.
        /// </summary>
        public string Culture { get; set; }
    }
}
