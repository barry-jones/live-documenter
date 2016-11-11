
namespace TheBoxSoftware.Reflection.Tests.Unit.Signitures
{
    using NUnit.Framework;
    using Reflection.Core.COFF;
    using Reflection.Signitures;

    // tests to verify that the signiture builder can handle method based
    // signitures:
    //  those which start with C, DEFAULT, FASTCALL, STDCALL, THISCALL or VARARG

    [TestFixture]
    public class SignitureBuilder_MethodSignitures_Tests
    {
        [Test]
        public void SignitureIsMethodType_Read_ReturnsMethodBasedSigniture()
        {
            byte[] contents = new byte[] {
                0x01, 0x00
            };

            BlobStream stream = new BlobStream(contents, 0, contents.Length);
            SignatureBuilder builder = new SignatureBuilder(stream);

            Signiture result = builder.Read(0);

            Assert.AreEqual(Signitures.MethodDef, result.Type);
        }
    }
}
