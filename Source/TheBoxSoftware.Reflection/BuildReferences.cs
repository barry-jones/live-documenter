
namespace TheBoxSoftware.Reflection
{
    using Core;
    using Core.COFF;

    internal class BuildReferences
    {
        public AssemblyDef Assembly { get; set; }

        public PeCoffFile PeCoffFile { get; set; }

        public MetadataDirectory Metadata { get; set; }

        public MetadataToDefinitionMap Map { get; set; }
    }
}