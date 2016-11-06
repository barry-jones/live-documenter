
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using Helpers;
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Core.COFF;

    [TestFixture]
    public class DeclSecurityMetadataTableRowTests
    {
        [Test]
        public void DeclSecurity_WhenCreated_ValuesAreReadCorrectly()
        {
            ICodedIndexResolver resolver = CodedIndexHelper.CreateCodedIndexResolver(2);
            byte[] contents = new byte[] {
                0x01, 0x00,
                0x00, 0x00,
                0x01, 0x00
            };

            DeclSecurityMetadataTableRow row = new DeclSecurityMetadataTableRow(contents, 0, resolver, 2);

            Assert.AreEqual(1, row.PermissionSet);
            Assert.AreEqual(1, row.Action);
            Assert.IsNotNull(row.Parent);
        }

        [TestCase(2, 2, 6)]
        [TestCase(4, 2, 8)]
        public void DeclSecurity_WhenCreated_OffsetIsMovedOn(byte blobIndexSize, int codedIndexSize, int expected)
        {
            byte[] content = new byte[20];
            Offset offset = 0;
            ICodedIndexResolver resolver = CodedIndexHelper.CreateCodedIndexResolver(codedIndexSize);

            DeclSecurityMetadataTableRow row = new DeclSecurityMetadataTableRow(content, offset, resolver, blobIndexSize);

            Assert.AreEqual(expected, offset.Current);
        }
    }
}
