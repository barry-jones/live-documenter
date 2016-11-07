
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using NUnit.Framework;
    using Helpers;
    using Reflection.Core;
    using Reflection.Core.COFF;

    [TestFixture]
    public class TypeRefMetadataTableRowTests
    {
        [Test]
        public void TypeRef_WhenCreated_FieldsAreReadCorrectly()
        {
            byte[] contents = new byte[] {
                0x00, 0x00,
                0x01, 0x00, 
                0x02, 0x00
            };
            ICodedIndexResolver resolver = CodedIndexHelper.CreateCodedIndexResolver(2);
            Offset offset = 0;

            TypeRefMetadataTableRow row = new TypeRefMetadataTableRow(contents, offset, resolver, 2);

            Assert.IsNotNull(row.ResolutionScope);
            Assert.AreEqual(1, row.Name.Value);
            Assert.AreEqual(2, row.Namespace.Value);
        }

        [Test]
        public void TypeRef_WhenCreated_OffsetIsMovedOn()
        {
            byte[] contents = new byte[6];
            ICodedIndexResolver resolver = CodedIndexHelper.CreateCodedIndexResolver(2);
            Offset offset = 0;

            TypeRefMetadataTableRow row = new TypeRefMetadataTableRow(contents, offset, resolver, 2);

            Assert.AreEqual(6, offset.Current);
        }
    }
}
