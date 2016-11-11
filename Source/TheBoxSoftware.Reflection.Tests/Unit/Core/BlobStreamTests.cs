
namespace TheBoxSoftware.Reflection.Tests.Unit.Core
{
    using NUnit.Framework;
    using Reflection.Core.COFF;

    [TestFixture]
    public class BlobStreamTests
    {
        [Test]
        public void HasNoSize_GetLength_ReturnsZero()
        {
            BlobStream stream = new BlobStream(new byte[0], 0, 0);

            int result = stream.GetLength();

            Assert.AreEqual(0, result);
        }

        [Test]
        public void HasSize_GetLength_ReturnsSize()
        {
            BlobStream stream = new BlobStream(new byte[10], 0, 10);

            int result = stream.GetLength();

            Assert.AreEqual(10, result);
        }

        [Test]
        public void WhenLoaded_GetRange_ReturnsValues()
        {
            byte[] contents = new byte[] {
                0x01, 0x02, 0x03, 0x04
            };
            BlobStream stream = new BlobStream(contents, 0, contents.Length);

            byte[] result = stream.GetRange(0, 4);

            Assert.AreEqual(4, result.Length);
            Assert.AreEqual(0x01, result[0]);
            Assert.AreEqual(0x04, result[3]);
        }
    }
}
