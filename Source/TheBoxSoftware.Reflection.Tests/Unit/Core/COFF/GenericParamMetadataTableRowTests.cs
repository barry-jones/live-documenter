
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using Helpers;
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Core.COFF;

    [TestFixture]
    public class GenericParamMetadataTableRowTests
    {
        [Test]
        public void GenericParam_WhenCreated_FieldsAreReadCorrectly()
        {
            ICodedIndexResolver resolver = IndexHelper.CreateCodedIndexResolver(2);
            IIndexDetails indexDetails = IndexHelper.CreateIndexDetails(2);
            byte[] contents = new byte[] {
                0x01, 0x00,
                0x01, 0x00,
                0x01, 0x00, 
                0x01, 0x00
            };

            GenericParamMetadataTableRow row = new GenericParamMetadataTableRow(contents, 0, resolver, indexDetails);

            Assert.AreEqual(1, row.Number);
            Assert.AreEqual(GenericParamAttributes.Covariant, row.Flags);
            Assert.IsNotNull(row.Owner);
            Assert.AreEqual(1, row.Name.Value);
        }

        [Test]
        public void GenericParam_WhenCreated_OffsetIsMovedOn()
        {
            Offset offset = 0;
            byte[] contents = new byte[10];
            IIndexDetails indexDetails = IndexHelper.CreateIndexDetails(2);
            ICodedIndexResolver resolver = IndexHelper.CreateCodedIndexResolver(2);

            GenericParamMetadataTableRow row = new GenericParamMetadataTableRow(contents, offset, resolver, indexDetails);

            Assert.AreEqual(8, offset.Current);
        }
    }
}
