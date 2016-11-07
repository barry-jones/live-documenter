
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Core.COFF;
    using Helpers;

    [TestFixture]
    public class MethodImplMetadataTableRowTests
    {
        [Test]
        public void MethodImpl_WhenCreated_FieldsAreReadCorrectly()
        {
            byte[] contents = new byte[] {
                0x01, 0x00, 
                0x00, 0x00,
                0x00, 0x00
            };
            ICodedIndexResolver resolver = IndexHelper.CreateCodedIndexResolver(2);
            IIndexDetails indexDetails = IndexHelper.CreateIndexDetails(2);

            MethodImplMetadataTableRow row = new MethodImplMetadataTableRow(contents, 0, resolver, indexDetails);

            Assert.AreEqual(1, row.Class.Value);
            Assert.IsNotNull(row.MethodBody);
            Assert.IsNotNull(row.MethodDeclaration);
        }

        [Test]
        public void MethodImpl_WhenCreated_OffsetIsMovedOn()
        {
            byte[] contents = new byte[6];
            Offset offset = 0;
            ICodedIndexResolver resolver = IndexHelper.CreateCodedIndexResolver(2);
            IIndexDetails indexDetails = IndexHelper.CreateIndexDetails(2);

            MethodImplMetadataTableRow row = new MethodImplMetadataTableRow(contents, offset, resolver, indexDetails);

            Assert.AreEqual(6, offset.Current);
        }
    }
}
