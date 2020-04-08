
namespace TheBoxSoftware.Reflection
{
    using Core;
    using Core.COFF;

    /// <summary>
    /// Utility class that contains useful references for building members
    /// from metadata.
    /// </summary>
    internal class BuildReferences
    {
        public AssemblyDef Assembly { get; set; }

        public PeCoffFile PeCoffFile { get; set; }

        public MetadataDirectory Metadata { get; set; }

        public MetadataToDefinitionMap Map { get; set; }
    }
}