
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
                0x03, 0x00, 0x00, 0x01
            };

            BlobStream stream = new BlobStream(contents, 0, contents.Length);
            SignatureBuilder builder = new SignatureBuilder(stream);

            Signiture result = builder.Read(0);

            Assert.AreEqual(Signitures.MethodDef, result.Type);
        }

        [Test]
        public void GenericMethodNoParamsReturnsVoid_Read_IsCorrect()
        {
            byte[] contents = new byte[] { 0x04, 0x30, 0x01, 0x00, 0x01 };
            SignatureBuilder builder = CreateBuilder(contents);

            Signiture result = builder.Read(0);

            Assert.AreEqual(
                CallingConventions.Generic | CallingConventions.HasThis,
                ((CallingConventionSignitureToken)result.Tokens[0]).Convention);
            Assert.AreEqual("[GenParamCount: 1]", result.Tokens[1].ToString());
            Assert.AreEqual("[ParamCount: 0]", result.Tokens[2].ToString());
            Assert.AreEqual("[ElementType: System.Void] ", result.Tokens[3].ToString());
        }

        [Test]
        public void NoParamsJaggedArrayReturn_Read_IsCorrect()
        {
            // int[][] a()
            byte[] contents = new byte[] { 0x05, 0x20, 0x00, 0x1d, 0x1d, 0x08 };
            SignatureBuilder builder = CreateBuilder(contents);

            Signiture result = builder.Read(0);

            Assert.AreEqual("[CallingConvention: HasThis]", result.Tokens[0].ToString());
            Assert.AreEqual("[ParamCount: 0]", result.Tokens[1].ToString());
            Assert.AreEqual("[ElementType: SZArray]", result.Tokens[2].ToString());
            Assert.AreEqual("[ElementType: SZArray]", result.Tokens[3].ToString());
            Assert.AreEqual("[ElementType: I4]", result.Tokens[4].ToString());
        }

        private SignatureBuilder CreateBuilder(byte[] contents)
        {
            BlobStream stream = new BlobStream(contents, 0, contents.Length);
            SignatureBuilder builder = new SignatureBuilder(stream);
            return builder;
        }
    }
}
