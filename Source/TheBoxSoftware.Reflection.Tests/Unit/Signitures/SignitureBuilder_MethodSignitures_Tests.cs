
namespace TheBoxSoftware.Reflection.Tests.Unit.Signatures
{
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Core.COFF;
    using Reflection.Signatures;

    // tests to verify that the signiture builder can handle method based
    // Signatures:
    //  those which start with C, DEFAULT, FASTCALL, STDCALL, THISCALL or VARARG

    [TestFixture]
    public class SignitureBuilder_MethodSignatures_Tests
    {
        [Test]
        public void SignitureIsMethodType_Read_ReturnsMethodBasedSigniture()
        {
            byte[] contents = new byte[] {
                0x03, 0x00, 0x00, 0x01
            };

            BlobStream stream = new BlobStream(contents, 0, contents.Length);
            SignatureBuilder builder = new SignatureBuilder(stream);

            Signature result = builder.Read(0);

            Assert.AreEqual(Signatures.MethodDef, result.Type);
        }

        [Test]
        public void GenericMethodNoParamsReturnsVoid_Read_IsCorrect()
        {
            byte[] contents = new byte[] { 0x04, 0x30, 0x01, 0x00, 0x01 };
            SignatureBuilder builder = CreateBuilder(contents);

            Signature result = builder.Read(0);

            Assert.AreEqual(
                CallingConventions.Generic | CallingConventions.HasThis,
                ((CallingConventionSignatureToken)result.Tokens[0]).Convention);
            Assert.AreEqual("[GenParamCount: 1]", result.Tokens[1].ToString());
            Assert.AreEqual("[ParamCount: 0]", result.Tokens[2].ToString());
            Assert.AreEqual("[Type: [ElementType: System.Void] ] ", result.Tokens[3].ToString());
        }

        [Test]
        public void NoParamsJaggedArrayReturn_Read_IsCorrect()
        {
            // int[][] a()
            byte[] contents = new byte[] { 0x05, 0x20, 0x00, 0x1d, 0x1d, 0x08 };
            SignatureBuilder builder = CreateBuilder(contents);

            Signature result = builder.Read(0);

            Assert.AreEqual("[CallingConvention: HasThis]", result.Tokens[0].ToString());
            Assert.AreEqual("[ParamCount: 0]", result.Tokens[1].ToString());
            Assert.IsInstanceOf<TypeSignatureToken>(result.Tokens[2]);

            // TODO: should better test SZArray, SZArray, I4 element types are found.
        }

        private SignatureBuilder CreateBuilder(byte[] contents)
        {
            BlobStream stream = new BlobStream(contents, 0, contents.Length);
            SignatureBuilder builder = new SignatureBuilder(stream);
            return builder;
        }
    }

    [TestFixture]
    public class SignatureBuilder_TypeSpec_Tests
    {
        [Test]
        public void WhenNotCustomMod_CustomMod_IsInvalid()
        {

        }

        public void PointerFollowedByNoCustomModsThenVoid_IsCorrect()
        {
            ElementTypes pointer = ElementTypes.Ptr;
        }

        public CustomModifierToken CustomMod(byte[] content, Offset offset)
        {

        }
    }
}
