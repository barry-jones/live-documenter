
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Core.COFF;
    using Reflection.Signitures;
    using Helpers;

    [TestFixture]
    public class FieldMetadataTableRowTests
    {
        [Test]
        public void Field_WhenCreated_FieldsAreReadCorrectly()
        {
            ICodedIndexResolver resolver = CodedIndexHelper.CreateCodedIndexResolver(2);
            byte[] contents = new byte[] {
                0x06, 0x00,
                0x01, 0x00,
                0x01, 0x00
            };

            FieldMetadataTableRow row = new FieldMetadataTableRow(contents, 0, 2, 2);

            Assert.AreEqual(FieldAttributes.Public, row.Flags);
            Assert.AreEqual(1, row.Name.Value);
            Assert.AreEqual(1, row.Signiture.Value);
            Assert.AreEqual(Signitures.Field, row.Signiture.SignitureType);
        }

        [Test]
        public void Field_WhenCreated_OffsetIsMovedOn()
        {
            ICodedIndexResolver resolver = CodedIndexHelper.CreateCodedIndexResolver(2);
            byte[] contents = new byte[20];
            Offset offset = 0;

            FieldMetadataTableRow row = new FieldMetadataTableRow(contents, offset, 2, 2);

            Assert.AreEqual(6, offset.Current);
        }
    }
}
