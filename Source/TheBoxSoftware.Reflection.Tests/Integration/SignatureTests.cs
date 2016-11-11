
namespace TheBoxSoftware.Reflection.Tests.Integration
{
    using System.Linq;
    using NUnit.Framework;
    using Signatures;

    [TestFixture]
    public class SignatureTests
    {
        private const string TestFile = @"source\testoutput\documentationtest.dll";

        private AssemblyDef _assemblyDef;

        [OneTimeSetUp]
        public void InitialiseAssemblyDefFileUsedForTests()
        {
            _assemblyDef = AssemblyDef.Create(TestFile);
        }

        [Test, Category("Integration")]
        public void ByRef_TokenIsSetForRefParameters()
        {
            TypeDef typeDef = _assemblyDef.FindType("DocumentationTest", "AllOutputTypesClass");
            MethodDef method = typeDef.GetMethods().First(p => p.Name == "bb");
            ParamDef refParam = method.Parameters.Find(p => p.Name == "y");
            Signature sig = method.Signiture;

            // get the details of the byref parameter
            ParamSignatureToken byRefToken = method.Signiture.GetParameterTokens()[refParam.Sequence - 1];
            ParamSignatureToken notRefToken = method.Signiture.GetParameterTokens()[0];

            Assert.IsTrue(byRefToken.IsByRef);
            Assert.IsFalse(notRefToken.IsByRef);
        }
    }
}
