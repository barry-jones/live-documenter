
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Core.COFF;
    using Reflection.Signatures;
    using Helpers;

    [TestFixture]
    public class FieldMetadataTableRowTests
    {
        [Test]
        public void Field_WhenCreated_FieldsAreReadCorrectly()
        {
            ICodedIndexResolver resolver = IndexHelper.CreateCodedIndexResolver(2);
            IIndexDetails indexDetails = IndexHelper.CreateIndexDetails(2);
            byte[] contents = new byte[] {
                0x06, 0x00,
                0x01, 0x00,
                0x01, 0x00
            };

            FieldMetadataTableRow row = new FieldMetadataTableRow(contents, 0, indexDetails);

            Assert.AreEqual(FieldAttributes.Public, row.Flags);
            Assert.AreEqual(1, row.Name.Value);
            Assert.AreEqual(1, row.Signiture.Value);
            Assert.AreEqual(Signatures.Field, row.Signiture.SignitureType);
        }

        [Test]
        public void Field_WhenCreated_OffsetIsMovedOn()
        {
            ICodedIndexResolver resolver = IndexHelper.CreateCodedIndexResolver(2);
            IIndexDetails indexDetails = IndexHelper.CreateIndexDetails(2);
            byte[] contents = new byte[20];
            Offset offset = 0;

            FieldMetadataTableRow row = new FieldMetadataTableRow(contents, offset, indexDetails);

            Assert.AreEqual(6, offset.Current);
        }
    }
}
