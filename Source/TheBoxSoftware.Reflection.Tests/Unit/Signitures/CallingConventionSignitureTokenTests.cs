
namespace TheBoxSoftware.Reflection.Tests.Unit.Signitures
{
    using NUnit.Framework;
    using Reflection.Signitures;

    [TestFixture]
    public class CallingConventionSignitureTokenTests
    {
        [Test]
        public void CallingConventionToken_Create_WhenProvidedACorrectValue_ShouldReturnSameValue()
        {
            byte[] content = new byte[] { (byte)CallingConventions.Default };

            CallingConventionSignitureToken token = new CallingConventionSignitureToken(content, 0);

            Assert.AreEqual(CallingConventions.Default, token.Convention);
        }

        [Test]
        public void CallingConventionToken_ToString_WhenProvidedDefaultConvention_OutputsCorrectly()
        {
            byte[] content = new byte[] { (byte)CallingConventions.Default };

            CallingConventionSignitureToken token = new CallingConventionSignitureToken(content, 0);

            string result = token.ToString();

            Assert.AreEqual("[CallingConvention: Default]", result);
        }
    }
}
