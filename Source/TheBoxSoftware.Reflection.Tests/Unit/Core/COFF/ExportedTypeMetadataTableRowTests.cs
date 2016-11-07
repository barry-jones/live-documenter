
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using Helpers;
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Core.COFF;

    [TestFixture]
    public class ExportedTypeMetadataTableRowTests
    {
        [Test]
        public void ExportedType_WhenCreated_ReadFields()
        {
            ICodedIndexResolver resolver = CodedIndexHelper.CreateCodedIndexResolver(2);
            byte[] contents = new byte[] {
                0x01, 0x00, 0x00, 0x00,
                0x05, 0x00, 0x00, 0x00,
                0x01, 0x00,
                0x01, 0x00,
                0x00, 0x00
            };

            ExportedTypeMetadataTableRow row = new ExportedTypeMetadataTableRow(contents, 0, resolver, 2);

            Assert.AreEqual(TypeAttributes.Public, row.Flags);
            Assert.AreEqual(5, row.TypeDefId);
            Assert.AreEqual(1, row.TypeName.Value);
            Assert.AreEqual(1, row.TypeNamespace.Value);
            Assert.IsNotNull(row.Implementation);
        }

        [Test]
        public void ExportedType_WhenCreated_OffsetIsMovedOn()
        {
            Offset offset = 0;
            byte[] contents = new byte[20];
            ICodedIndexResolver resolver = CodedIndexHelper.CreateCodedIndexResolver(2);

            ExportedTypeMetadataTableRow row = new ExportedTypeMetadataTableRow(contents, offset, resolver, 2);

            Assert.AreEqual(14, offset.Current);
        }
    }
}
