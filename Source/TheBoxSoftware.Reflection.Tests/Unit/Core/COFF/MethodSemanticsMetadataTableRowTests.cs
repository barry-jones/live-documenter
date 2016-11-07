
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Core.COFF;
    using Helpers;

    [TestFixture]
    public class MethodSemanticsMetadataTableRowTests
    {
        [Test]
        public void MethodSemantics_WhenCreated_FieldsAreReadCorrectly()
        {
            ICodedIndexResolver resolver = IndexHelper.CreateCodedIndexResolver(2);
            IIndexDetails indexDetails = IndexHelper.CreateIndexDetails(2);
            byte[] contents = new byte[] {
                0x01, 0x00,
                0x01, 0x00,
                0x00, 0x00
            };

            MethodSemanticsMetadataTableRow row = new MethodSemanticsMetadataTableRow(contents, 0, resolver, indexDetails);

            Assert.AreEqual(MethodSemanticsAttributes.Setter, row.Semantics);
            Assert.AreEqual(1, row.Method.Value);
            Assert.IsNotNull(row.Association);
        }

        [Test]
        public void MethodSemantics_WhenCreated_OffsetIsMovedOn()
        {
            byte[] contents = new byte[6];
            IIndexDetails indexDetails = IndexHelper.CreateIndexDetails(2);
            ICodedIndexResolver resolver = IndexHelper.CreateCodedIndexResolver(2);
            Offset offset = 0;

            MethodSemanticsMetadataTableRow row = new MethodSemanticsMetadataTableRow(contents, offset, resolver, indexDetails);

            Assert.AreEqual(6, offset.Current);
        }
    }
}
