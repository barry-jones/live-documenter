
namespace TheBoxSoftware.Reflection.Tests.Integration.Syntax
{
    using System.Linq;
    using NUnit.Framework;
    using Reflection.Syntax;

    [TestFixture]
    public class VB_MethodSyntaxTests
    {
        private const string TestFile = @"source\testoutput\documentationtest.dll";
        private const string NamespaceName = "SyntaxTests";
        private const string TypeName = "ForMethod";

        private AssemblyDef _assemblyDef;

        private IFormatter CreateFormatter(MethodDef method)
        {
            return SyntaxFactory.Create(method, Languages.VisualBasic);
        }

        [OneTimeSetUp]
        public void InitialiseAssemblyDefFileUsedForTests()
        {
            _assemblyDef = AssemblyDef.Create(TestFile);
        }

        [TestCase("ParameterNormal", "Public Sub ParameterNormal(\n\ttest As Int\n\t)")]
        [TestCase("ParameterRef", "Public Sub ParameterRef(\n\tByRef test As Int\n\t)")]
        [TestCase("ParameterOut", "Public Sub ParameterOut(\n\tByRef test As Int\n\t)")]
        // bug 49 [TestCase("ParameterDefault", "public void ParameterDefault(\n\tint test = 3\n\t)")]
        public void VBSyntax_Method_ParameterModifiers(string method, string expected)
        {
            TestIt(method, expected);
        }

        [TestCase("PublicMethod", "Public Sub PublicMethod()")]
        [TestCase("InternalMethod", "Friend Sub InternalMethod()")]
        [TestCase("ProtectedInternalMethod", "Friend Protected Sub ProtectedInternalMethod()")]
        [TestCase("ProtectedMethod", "Protected Sub ProtectedMethod()")]
        [TestCase("PrivateMethod", "Private Sub PrivateMethod()")]
        public void VBSyntax_Method_AccessModifiers(string method, string expected)
        {
            TestIt(method, expected);
        }

        [TestCase("StaticMethod", "Public Shared Sub StaticMethod()")]
        public void VBSyntax_Method_Static(string method, string expected)
        {
            TestIt(method, expected);
        }

        [TestCase("ReturnVoid", "Public Sub ReturnVoid()")]
        [TestCase("ReturnWellKnownType", "Public Function ReturnWellKnownType() As Int")]
        [TestCase("ReturnJaggedArray", "Public Function ReturnJaggedArray() As Int()()")]
        [TestCase("ReturnClass", "Public Function ReturnClass() As ForMethod")]
        [TestCase("ReturnArray", "Public Function ReturnArray() As Byte()")]
        [TestCase("ReturnGeneric", "Public Function ReturnGeneric() As GenericClass(Of String)")]
        public void VBSyntax_Method_ReturnTypes(string method, string expected)
        {
            TestIt(method, expected);
        }

        [TestCase("Generic", "Public Sub Generic(Of T)()")]
        // bug 50
        //[TestCase("GenericWhereClass", "public void GenericWhereClass<T>() where T : class")]
        //[TestCase("GenericWhereStruct", "public void GenericWhereStruct<T>() where T : struct")]
        //[TestCase("GenericWhereInterface", "public void GenericWhereInteface<T>() where T : ITest")]
        //[TestCase("GenericWhereNew", "public void GenericWhereNew<T>() where T : new()")]
        //[TestCase("GenericWhereAll", "public void GenericWhereAll<T>() where T : class, ITest, new()")]
        public void VBSyntax_Method_Generics(string method, string expected)
        {
            TestIt(method, expected);
        }

        private void TestIt(string method, string expected)
        {
            TypeDef container = _assemblyDef.FindType(NamespaceName, TypeName);
            MethodDef testMethod = container.Methods.First(p => p.Name == method);
            IFormatter formatter = CreateFormatter(testMethod);

            SyntaxTokenCollection tokens = formatter.Format();

            Assert.AreEqual(expected, tokens.ToString());
        }
    }
}
