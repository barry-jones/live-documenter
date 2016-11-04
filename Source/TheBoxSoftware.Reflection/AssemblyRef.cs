
namespace TheBoxSoftware.Reflection
{
    using System;
    using Core.COFF;

    /// <summary>
    /// Represents a reference to an external library.
    /// </summary>
    public sealed class AssemblyRef : ReflectedMember
    {
        private Version _version;
        private string _culture;

        /// <summary>
        /// Initialises a new instance of the AssemblyRef class from the provided details.
        /// </summary>
        /// <param name="references">Container of all the references required to build this type.</param>
        /// <param name="row">The row that provides the assembly reference details.</param>
        /// <returns>A populated AssemblyRef instance.</returns>
        internal static AssemblyRef CreateFromMetadata(BuildReferences references, AssemblyRefMetadataTableRow row)
        {
            AssemblyRef assemblyRef = new AssemblyRef();

            assemblyRef.Version = new Version(
                row.MajorVersion,
                row.MinorVersion,
                row.BuildNumber,
                row.RevisionNumber);
            assemblyRef.Culture = references.Assembly.StringStream.GetString(row.Culture.Value);
            assemblyRef.UniqueId = references.Assembly.CreateUniqueId();
            assemblyRef.Name = references.Assembly.StringStream.GetString(row.Name.Value);
            assemblyRef.Assembly = references.Assembly;

            return assemblyRef;
        }

        /// <summary>
        /// The full version details of the referenced assembly.
        /// </summary>
        public Version Version
        {
            get { return _version; }
            set { _version = value; }
        }

        /// <summary>
        /// The string representing the culture of the assembly.
        /// </summary>
        public string Culture
        {
            get { return _culture; }
            set { _culture = value; }
        }
    }
}
