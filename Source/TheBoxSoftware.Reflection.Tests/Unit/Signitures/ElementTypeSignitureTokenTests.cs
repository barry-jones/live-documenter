
namespace TheBoxSoftware.Reflection.Tests.Unit.Signitures
{
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Signitures;

    [TestFixture]
    public class ElementTypeSignitureTokenTests
    {
        private ElementTypeSignitureToken CreateToken(byte[] content)
        {
            PeCoffFile file = new PeCoffFile(string.Empty);
            file.Map = new MetadataToDefinitionMap();
            return new ElementTypeSignitureToken(file, content, 0);
        }

        [Test]
        public void ElementTypeSignitureToken_Create_WhenClassAndTypeDef_ReturnsCorrectly()
        {
            // 0, 1 or 2 for second value
            byte[] content = new byte[] { (byte)ElementTypes.Class, 0 };

            ElementTypeSignitureToken token = CreateToken(content);

            Assert.AreEqual(ElementTypes.Class, token.ElementType);
            Assert.AreEqual(ILMetadataToken.TypeDef, (ILMetadataToken)token.Token);
        }

        [Test]
        public void ElementTypeSignitureToken_Create_WhenClassAndTypeRef_ReturnsCorrectly()
        {
            // 0, 1 or 2 for second value
            byte[] content = new byte[] { (byte)ElementTypes.Class, 1 };

            ElementTypeSignitureToken token = CreateToken(content);

            Assert.AreEqual(ElementTypes.Class, token.ElementType);
            Assert.AreEqual(ILMetadataToken.TypeRef, (ILMetadataToken)token.Token);
        }

        [Test]
        public void ElementTypeSignitureToken_Create_WhenClassAndTypeSpec_ReturnsCorrectly()
        {
            // 0, 1 or 2 for second value
            byte[] content = new byte[] { (byte)ElementTypes.Class, 2 };

            ElementTypeSignitureToken token = CreateToken(content);

            Assert.AreEqual(ElementTypes.Class, token.ElementType);
            Assert.AreEqual(ILMetadataToken.TypeSpec, (ILMetadataToken)token.Token);
        }

        [Test]
        public void ElementTypeSignitureToken_Create_WhenValueAndTypeDef_ReturnsCorrectly()
        {
            // 0, 1 or 2 for second value
            byte[] content = new byte[] { (byte)ElementTypes.ValueType, 0 };

            ElementTypeSignitureToken token = CreateToken(content);

            Assert.AreEqual(ElementTypes.ValueType, token.ElementType);
            Assert.AreEqual(ILMetadataToken.TypeDef, (ILMetadataToken)token.Token);
        }

        [Test]
        public void ElementTypeSignitureToken_Create_WhenValueAndTypeRef_ReturnsCorrectly()
        {
            // 0, 1 or 2 for second value
            byte[] content = new byte[] { (byte)ElementTypes.ValueType, 1 };

            ElementTypeSignitureToken token = CreateToken(content);

            Assert.AreEqual(ElementTypes.ValueType, token.ElementType);
            Assert.AreEqual(ILMetadataToken.TypeRef, (ILMetadataToken)token.Token);
        }

        [Test]
        public void ElementTypeSignitureToken_Create_WhenMVar_ReturnsCorrectly()
        {
            // 0, 1 or 2 for second value
            byte[] content = new byte[] { (byte)ElementTypes.MVar, 1 };

            ElementTypeSignitureToken token = CreateToken(content);

            Assert.AreEqual(ElementTypes.MVar, token.ElementType);
            Assert.AreEqual(1, token.Token);
        }

        [Test]
        public void ElementTypeSignitureToken_Create_WhenVar_ReturnsCorrectly()
        {
            // 0, 1 or 2 for second value
            byte[] content = new byte[] { (byte)ElementTypes.Var, 1 };

            ElementTypeSignitureToken token = CreateToken(content);

            Assert.AreEqual(ElementTypes.Var, token.ElementType);
            Assert.AreEqual(1, token.Token);
        }

        // unfortunately we need to convert the elementypes enum to byte as it is only internal and test methods
        // need to be public
        [TestCase((byte)ElementTypes.Boolean, (byte)ElementTypes.Boolean)]
        [TestCase((byte)ElementTypes.I, (byte)ElementTypes.I)]
        [TestCase((byte)ElementTypes.I1, (byte)ElementTypes.I1)]
        [TestCase((byte)ElementTypes.I2, (byte)ElementTypes.I2)]
        [TestCase((byte)ElementTypes.I4, (byte)ElementTypes.I4)]
        [TestCase((byte)ElementTypes.I8, (byte)ElementTypes.I8)]
        [TestCase((byte)ElementTypes.U, (byte)ElementTypes.U)]
        [TestCase((byte)ElementTypes.U1, (byte)ElementTypes.U1)]
        [TestCase((byte)ElementTypes.U2, (byte)ElementTypes.U2)]
        [TestCase((byte)ElementTypes.U4, (byte)ElementTypes.U4)]
        [TestCase((byte)ElementTypes.U8, (byte)ElementTypes.U8)]
        [TestCase((byte)ElementTypes.Char, (byte)ElementTypes.Char)]
        [TestCase((byte)ElementTypes.R4, (byte)ElementTypes.R4)]
        [TestCase((byte)ElementTypes.R8, (byte)ElementTypes.R8)]
        [TestCase((byte)ElementTypes.TypedByRef, (byte)ElementTypes.TypedByRef)]
        [TestCase((byte)ElementTypes.String, (byte)ElementTypes.String)]
        [TestCase((byte)ElementTypes.Object, (byte)ElementTypes.Object)]
        [TestCase((byte)ElementTypes.Void, (byte)ElementTypes.Void)]
        public void ElementTypeSignitureToken_Create_WhenWellKnownTypes_ReturnsCorrectly(byte type, byte expected)
        {
            byte[] content = new byte[] { type };

            ElementTypeSignitureToken token = CreateToken(content);

            Assert.AreEqual(expected, (byte)token.ElementType);
        }

        [Test]
        public void ElementTypeSignitureToken_IsToken_WhenContentIsArrayAndArrayIsValid_ReturnsTrue()
        {
            byte[] content = new byte[] { (byte)ElementTypes.Array };
            ElementTypes allowed = ElementTypes.Array;

            bool result = ElementTypeSignitureToken.IsToken(content, 0, allowed);

            Assert.IsTrue(result);
        }

        [Test]
        public void ElementTypeSignitureToken_IsToken_WhenContentIsArrayAndArrayIsNotValid_ReturnsTrue()
        {
            byte[] content = new byte[] { (byte)ElementTypes.Array };
            ElementTypes allowed = ElementTypes.TypedByRef;

            bool result = ElementTypeSignitureToken.IsToken(content, 0, allowed);

            Assert.IsFalse(result);
        }
    }
}
