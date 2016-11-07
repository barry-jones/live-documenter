
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using NUnit.Framework;
    using Helpers;
    using Reflection.Core;
    using Reflection.Core.COFF;

    [TestFixture]
    public class FieldMarshalMetadataTableRowTests
    {
        [Test]
        public void FieldMarshal_WhenCreated_FieldsAreReadCorrectly()
        {
            ICodedIndexResolver resolver = IndexHelper.CreateCodedIndexResolver(2);
            IIndexDetails indexDetails = IndexHelper.CreateIndexDetails(2);
            byte[] contents = new byte[] {
                0x01, 0x00,
                0x01, 0x00
            };

            FieldMarshalMetadataTableRow row = new FieldMarshalMetadataTableRow(contents, 0, resolver, indexDetails);

            Assert.AreEqual(1, row.NativeType);
            Assert.IsNotNull(row.Parent);
        }

        [Test]
        public void FieldMarshal_WhenCreated_OffsetIsMovedOn()
        {
            byte[] contents = new byte[10];
            Offset offset = 0;
            ICodedIndexResolver resolver = IndexHelper.CreateCodedIndexResolver(2);
            IIndexDetails indexDetails = IndexHelper.CreateIndexDetails(2);

            FieldMarshalMetadataTableRow row = new FieldMarshalMetadataTableRow(contents, offset, resolver, indexDetails);

            Assert.AreEqual(4, offset.Current);
        }
    }
}
