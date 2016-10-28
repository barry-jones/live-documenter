
namespace TheBoxSoftware.Reflection
{
    using Core.COFF;

    public class ConstantInfo
    {
        public static ConstantInfo CreateFromMetadata(AssemblyDef assembly, MetadataStream stream, ConstantMetadataTableRow row)
        {
            ConstantInfo constant = new ConstantInfo();
            constant.Value = (int)row.Value.Value;
            return constant;
        }

        public int Value { get; set; }
    }
}