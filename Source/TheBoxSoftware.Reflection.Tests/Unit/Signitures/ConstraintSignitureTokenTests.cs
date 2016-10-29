
namespace TheBoxSoftware.Reflection.Tests.Unit.Signitures
{
    using NUnit.Framework;
    using Reflection.Signitures;

    [TestFixture]
    public class ConstraintSignitureTokenTests
    {
        [Test]
        public void ConstraintToken_Create_WhenPinned_ShouldreturnPinned()
        {
            byte[] content = new byte[] { (byte)ElementTypes.Pinned };
            ConstraintSignitureToken token = new ConstraintSignitureToken(content, 0);

            Assert.AreEqual(ElementTypes.Pinned, token.Constraint);
        }

        [Test]
        public void ConstraintToken_ToString_WhenPinned_ShouldOutputCorrectly()
        {
            byte[] content = new byte[] { (byte)ElementTypes.Pinned };
            ConstraintSignitureToken token = new ConstraintSignitureToken(content, 0);

            string result = token.ToString();

            Assert.AreEqual("[Constraint: Pinned]", result);
        }

        [Test]
        public void ConstraintToken_IsToken_WhenNotPinned_IsNotToken()
        {
            byte[] content = new byte[] { 0 };

            bool result = ConstraintSignitureToken.IsToken(content, 0);

            Assert.IsFalse(result);
        }

        [Test]
        public void ConstraintToken_IsToken_WhenPinned_IsToken()
        {
            byte[] content = new byte[] { (byte)ElementTypes.Pinned };

            bool result = ConstraintSignitureToken.IsToken(content, 0);

            Assert.IsTrue(result);
        }
    }
}
