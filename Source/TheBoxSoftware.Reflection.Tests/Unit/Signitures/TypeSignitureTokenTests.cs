
namespace TheBoxSoftware.Reflection.Tests.Unit.Signitures
{
    using NUnit.Framework;
    using Reflection.Core.COFF;
    using Reflection.Signitures;

    [TestFixture]
    public class TypeSignitureTokenTests
    {
        // the first part of the signiture is always an ElementTypeSignitureToken
        //  this then determines how the rest of the signiture is handled.

        [Test]
        public void Create_WhenSignitureIsWellKnownType()
        {
            byte[] content = new byte[] {
                (byte)ElementTypes.Boolean
            };

            TypeSignitureToken token = new TypeSignitureToken(content, 0);

            Assert.AreSame(WellKnownTypeDef.Boolean, token.ElementType.Definition);
        }

        [Test]
        public void Create_WhenSignitureIsSZArray()
        {
            // should define an array of char
            byte[] content = new byte[] {
                (byte)ElementTypes.SZArray,
                (byte)ElementTypes.CModRequired,
                (byte)MetadataTables.TypeDef,
                (byte)ElementTypes.Char
            };

            TypeSignitureToken token = new TypeSignitureToken(content, 0);

            Assert.AreEqual(3, token.Tokens.Count);
            Assert.AreSame(WellKnownTypeDef.Char,
                ((ElementTypeSignitureToken)((TypeSignitureToken)token.Tokens[2]).Tokens[0]).Definition
                );
        }
    }
}
