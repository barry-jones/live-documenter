
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Core.COFF;

    [TestFixture]
    public class EventMapMetadataTableRowTests
    {
        [Test]
        public void EventMap_WhenCreated_FieldsAreReadCorrectly()
        {
            byte[] contents = new byte[] {
                0x01, 0x00,
                0x05, 0x00
            };

            EventMapMetadataTableRow row = new EventMapMetadataTableRow(contents, 0, 2, 2);

            Assert.AreEqual(1, row.Parent.Value);
            Assert.AreEqual(5, row.EventList.Value);
        }

        [Test]
        public void EventMap_WhenCreated_OffsetIsMovedOn()
        {
            byte[] contents = new byte[20];
            Offset offset = 0;

            EventMapMetadataTableRow row = new EventMapMetadataTableRow(contents, offset, 2, 2);

            Assert.AreEqual(4, offset.Current);
        }
    }
}
