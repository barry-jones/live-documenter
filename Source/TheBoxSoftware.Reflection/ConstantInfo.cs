
namespace TheBoxSoftware.Reflection
{
    using Core.COFF;

    public class ConstantInfo
    {
        public static ConstantInfo CreateFromMetadata(AssemblyDef assembly, MetadataStream stream, ConstantMetadataTableRow row)
        {
            // we are not doing anything with this at the moment but it stores the details of constants
            // indirectly via the BlobIndex, it's parent is obtainable via a HasConstant CodedIndex in
            // the metadata row.
            // The values here are for fields and parameters, so we can say that the default values for
            // parameters are accessible via these records.

            return new ConstantInfo();
        }
    }
}