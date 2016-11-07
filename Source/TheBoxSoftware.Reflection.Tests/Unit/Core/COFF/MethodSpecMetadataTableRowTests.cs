
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Core.COFF;
    using Helpers;

    [TestFixture]
    public class MethodSpecMetadataTableRowTests
    {
        [Test]
        public void MethodSpec_WhenCreated_FieldsAreReadCorrectly()
        {
            ICodedIndexResolver resolver = IndexHelper.CreateCodedIndexResolver(2);
            byte[] contents = new byte[] {
                0x01, 0x00,
                0x01, 0x00
            };
            Offset offset = 0;
            IIndexDetails indexDetails = IndexHelper.CreateIndexDetails(2);

            MethodSpecMetadataTableRow row = new MethodSpecMetadataTableRow(contents, offset, resolver, indexDetails);

            Assert.IsNotNull(row.Method);
            Assert.AreEqual(1, row.Instantiation);
        }

        [Test]
        public void MethodSpec_WhenCreated_OffsetIsMovedOn()
        {
            ICodedIndexResolver resolver = IndexHelper.CreateCodedIndexResolver(2);
            IIndexDetails indexDetails = IndexHelper.CreateIndexDetails(2);
            byte[] contents = new byte[4];
            Offset offset = 0;

            MethodSpecMetadataTableRow row = new MethodSpecMetadataTableRow(contents, offset, resolver, indexDetails);

            Assert.AreEqual(4, offset.Current);
        }
    }
}
