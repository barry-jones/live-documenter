
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Core.COFF;
    using Helpers;

    [TestFixture]
    public class ManifestResourceMetadataTableRowTests
    {
        [Test]
        public void ManifestResource_WhenCreated_FieldsAreReadCorrectly()
        {
            ICodedIndexResolver resolver = IndexHelper.CreateCodedIndexResolver(2);
            IIndexDetails indexDetails = IndexHelper.CreateIndexDetails(2);
            byte[] contents = new byte[] {
                0x01, 0x00, 0x00, 0x00,
                0x01, 0x00, 0x00, 0x00,
                0x01, 0x00, 
                0x00, 0x00
            };

            ManifestResourceMetadataTableRow row = new ManifestResourceMetadataTableRow(contents, 0, resolver, indexDetails);

            Assert.AreEqual(1, row.Offset);
            Assert.AreEqual(ManifestResourceAttributes.Public, row.Flags);
            Assert.AreEqual(1, row.Name.Value);
            Assert.IsNotNull(row.Implementation);
        }

        [Test]
        public void ManifestResource_WhenCreated_OffsetIsMovedOn()
        {
            ICodedIndexResolver resolver = IndexHelper.CreateCodedIndexResolver(2);
            IIndexDetails indexDetails = IndexHelper.CreateIndexDetails(2);
            byte[] contents = new byte[12];
            Offset offset = 0;

            ManifestResourceMetadataTableRow row = new ManifestResourceMetadataTableRow(contents, offset, resolver, indexDetails);

            Assert.AreEqual(12, offset.Current);
        }
    }
}
