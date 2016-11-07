
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Core.COFF;
    using Helpers;

    [TestFixture]
    public class TypeDefMetadataTableRowTests
    {
        [Test]
        public void TypeDef_WhenCreated_FieldsAreReadCorrectly()
        {
            IIndexDetails indexDetails = IndexHelper.CreateIndexDetails(2);
            byte[] contents = new byte[] {
                0x80, 0x00, 0x00, 0x00,
                0x01, 0x00, 
                0x02, 0x00,
                0x05, 0x00,
                0x03, 0x00,
                0x04, 0x00
            };
            ICodedIndexResolver resolver = IndexHelper.CreateCodedIndexResolver(2);
            Offset offset = 0;

            TypeDefMetadataTableRow row = new TypeDefMetadataTableRow(contents, offset, resolver, indexDetails);

            Assert.AreEqual(TypeAttributes.Abstract, row.Flags);
            Assert.AreEqual(1, row.Name.Value);
            Assert.AreEqual(2, row.Namespace.Value);
            Assert.IsNotNull(row.Extends);
            Assert.AreEqual(3, row.FieldList.Value);
            Assert.AreEqual(4, row.MethodList.Value);
        }

        [Test]
        public void TypeDef_WhenCreated_OffsetIsMovedOn()
        {
            IIndexDetails indexDetails = IndexHelper.CreateIndexDetails(2);
            byte[] contents = new byte[14];
            ICodedIndexResolver resolver = IndexHelper.CreateCodedIndexResolver(2);
            Offset offset = 0;

            TypeDefMetadataTableRow row = new TypeDefMetadataTableRow(contents, offset, resolver, indexDetails);

            Assert.AreEqual(14, offset.Current);
        }
    }
}
