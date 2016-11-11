
namespace TheBoxSoftware.Reflection.Tests.Unit.Signatures
{
    using NUnit.Framework;
    using Reflection.Signatures;

    [TestFixture]
    public class CustomModifierTokenTests
    {
        //[Test]
        public void CustomModifierToken_Create()
        {
            // [cmod_opt|cmod_req]
            byte[] content = new byte[] {
                (byte)ElementTypes.CModOptional,
                0x49 // typeref + rowindex 12
            };

            CustomModifierToken token = new CustomModifierToken(content, 0);

            Assert.AreEqual(ElementTypes.CModOptional, token.Modifier);
            Assert.AreEqual(12, token.Index.Index);
            Assert.AreEqual(Reflection.Core.COFF.MetadataTables.TypeRef, token.Index.Table);
        }
    }
}
