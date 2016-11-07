
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using Helpers;
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Core.COFF;

    [TestFixture]
    public class MethodMetadataTableRowTests
    {
        [Test]
        public void Method_WhenCreated_FieldsAreReadCorrectly()
        {
            byte[] contents = new byte[] {
                0x01, 0x00, 0x00, 0x00,
                0x02, 0x00,
                0x01, 0x00,
                0x03, 0x00,
                0x02, 0x00, 
                0x01, 0x00
            };
            IIndexDetails indexDetails = IndexHelper.CreateIndexDetails(2);

            MethodMetadataTableRow row = new MethodMetadataTableRow(contents, 0, indexDetails);

            Assert.AreEqual(1, row.RVA);
            Assert.AreEqual(MethodImplFlags.OPTIL, row.ImplFlags);
            Assert.AreEqual(MethodAttributes.Private, row.Flags);
            Assert.AreEqual(3, row.Name.Value);
            Assert.AreEqual(2, row.Signiture.Value);
            Assert.AreEqual(1, row.ParamList.Value);
        }

        [Test]
        public void Method_WhenCreated_OffsetIsMovedOn()
        {
            byte[] contents = new byte[14];
            Offset offset = 0;
            IIndexDetails indexDetails = IndexHelper.CreateIndexDetails(2);

            MethodMetadataTableRow row = new MethodMetadataTableRow(contents, offset, indexDetails);

            Assert.AreEqual(14, offset.Current);
        }
    }
}
