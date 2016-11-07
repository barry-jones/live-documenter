
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using Helpers;
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Core.COFF;

    [TestFixture]
    public class ConstantMetadataTableRowTests
    {
        [Test]
        public void Constant_WhenCreated_ValuesAreReadCorrectly()
        {
            ICodedIndexResolver resolver = IndexHelper.CreateCodedIndexResolver(2);
            IIndexDetails indexDetails = IndexHelper.CreateIndexDetails(2);
            byte[] content = new byte[] {
                0x01,
                0x00,
                0x05, 0x00,
                0x00, 0x00
            };

            ConstantMetadataTableRow row = new ConstantMetadataTableRow(content, 0, resolver, indexDetails);

            Assert.AreEqual(Reflection.Signitures.ElementTypes.Void, row.Type);
            Assert.IsNotNull(row.Parent);
            Assert.AreEqual(0, row.Value.Value);
        }

        [Test]
        public void Constant_WhenCreated_OffsetIsMovedOn()
        {
            ICodedIndexResolver resolver = IndexHelper.CreateCodedIndexResolver(2);
            IIndexDetails indexDetails = IndexHelper.CreateIndexDetails(2);
            Offset offset = 0;
            byte[] content = new byte[30];

            ConstantMetadataTableRow row = new ConstantMetadataTableRow(content, offset, resolver, indexDetails);

            Assert.AreEqual(6, offset.Current);
        }
    }
}
