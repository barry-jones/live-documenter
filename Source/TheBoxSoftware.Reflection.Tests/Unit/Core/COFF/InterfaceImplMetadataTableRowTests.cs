
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Core.COFF;
    using Helpers;

    [TestFixture]
    public class InterfaceImplMetadataTableRowTests
    {
        [Test]
        public void InterfaceImpl_WhenCreated_FieldsAreReadCorrectly()
        {
            byte[] contents = new byte[] {
                0x01, 0x00,
                0x00, 0x00
            };
            ICodedIndexResolver resolver = CodedIndexHelper.CreateCodedIndexResolver(2);

            InterfaceImplMetadataTableRow row = new InterfaceImplMetadataTableRow(contents, 0, resolver, 2);

            Assert.AreEqual(1, row.Class.Value);
            Assert.IsNotNull(row.Interface);
        }

        [Test]
        public void InterfaceImpl_WhenCreated_OffsetIsMovedOn()
        {
            byte[] contents = new byte[10];
            ICodedIndexResolver resolver = CodedIndexHelper.CreateCodedIndexResolver(2);
            Offset offset = 0;

            InterfaceImplMetadataTableRow row = new InterfaceImplMetadataTableRow(contents, offset, resolver, 2);

            Assert.AreEqual(4, offset.Current);
        }
    }
}
