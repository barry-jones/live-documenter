
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using Helpers;
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Core.COFF;

    [TestFixture]
    public class EventMetadataTableRowTests
    {
        [Test]
        public void Event_WhenCreated_ReadValuesCorrectly()
        {
            ICodedIndexResolver resolver = CodedIndexHelper.CreateCodedIndexResolver(2);
            byte[] contents = new byte[] {
                0x00, 0x02,
                0x01, 0x00,
                0x01, 0x00
            };

            EventMetadataTableRow row = new EventMetadataTableRow(contents, 0, resolver, 2);

            Assert.AreEqual(EventAttributes.SpecialName, row.EventFlags);
            Assert.IsNotNull(row.EventType);
            Assert.AreEqual(1, row.Name.Value);
        }

        [Test]
        public void Event_WhenCreated_MovesOffsetOn()
        {
            ICodedIndexResolver resolver = CodedIndexHelper.CreateCodedIndexResolver(2);
            byte[] contents = new byte[20];
            Offset offset = 0;

            EventMetadataTableRow row = new EventMetadataTableRow(contents, offset, resolver, 2);

            Assert.AreEqual(6, offset.Current);
        }
    }
}
