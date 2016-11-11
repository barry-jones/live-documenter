
namespace TheBoxSoftware.Reflection.Tests.Unit.Signitures
{
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Signitures;

    [TestFixture]
    public class SignitureTokenTests
    {
        [Test]
        public void SignitureToken_GetCompressedValue_WhenCompressedByte_ReturnsCorrectValue()
        {
            // bytes are not really compressed, anything under 127 will be read as a byte
            byte[] content = new byte[] { 126 };
            Offset offset = 0;

            uint result = SignatureToken.GetCompressedValue(content, offset);

            Assert.AreEqual(126, result);
        }

        [Test]
        public void SignitureToken_GetCompressedValue_WhenCompressedShort_ReturnsCorrectValue()
        {
            // first byte is greater than 126 and less than 190 and contains two bytes in reverse order
            byte[] content = new byte[] { 190, 10 };
            Offset offset = 0;

            uint result = SignatureToken.GetCompressedValue(content, offset);

            Assert.AreEqual(15882, result);
        }

        [Test]
        public void SignitureToken_GetCompressedValue_WhenCompressedInt_ReturnsCorrectValue()
        {
            // first byte = 190 - 222 is compressed integer
            byte[] content = new byte[] { 222, 10, 17, 180 };
            Offset offset = 0;

            uint result = SignatureToken.GetCompressedValue(content, offset);

            Assert.AreEqual(503976372, result);
        }
    }
}
