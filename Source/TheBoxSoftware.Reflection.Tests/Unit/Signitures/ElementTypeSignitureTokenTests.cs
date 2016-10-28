
namespace TheBoxSoftware.Reflection.Tests.Unit.Signitures
{
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Signitures;

    [TestFixture]
    public class ElementTypeSignitureTokenTests
    {
        [Test]
        public void ElementTypeSignitureToken_Create_WhenClassAndTypeDef_ReturnsCorrectly()
        {
            PeCoffFile file = new PeCoffFile(string.Empty);

            // 0, 1 or 2 for second value
            byte[] content = new byte[] { (byte)ElementTypes.Class, 0 };

            ElementTypeSignitureToken token = new ElementTypeSignitureToken(file, content, 0);

            Assert.AreEqual(ElementTypes.Class, token.ElementType);
            Assert.AreEqual(ILMetadataToken.TypeDef, (ILMetadataToken)token.Token);
        }

        [Test]
        public void ElementTypeSignitureToken_Create_WhenClassAndTypeRef_ReturnsCorrectly()
        {
            PeCoffFile file = new PeCoffFile(string.Empty);

            // 0, 1 or 2 for second value
            byte[] content = new byte[] { (byte)ElementTypes.Class, 1 };

            ElementTypeSignitureToken token = new ElementTypeSignitureToken(file, content, 0);

            Assert.AreEqual(ElementTypes.Class, token.ElementType);
            Assert.AreEqual(ILMetadataToken.TypeRef, (ILMetadataToken)token.Token);
        }

        [Test]
        public void ElementTypeSignitureToken_Create_WhenClassAndTypeSpec_ReturnsCorrectly()
        {
            PeCoffFile file = new PeCoffFile(string.Empty);

            // 0, 1 or 2 for second value
            byte[] content = new byte[] { (byte)ElementTypes.Class, 2 };

            ElementTypeSignitureToken token = new ElementTypeSignitureToken(file, content, 0);

            Assert.AreEqual(ElementTypes.Class, token.ElementType);
            Assert.AreEqual(ILMetadataToken.TypeSpec, (ILMetadataToken)token.Token);
        }
    }
}
