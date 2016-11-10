
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
    }
}
