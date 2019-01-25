
namespace TheBoxSoftware.Reflection.Tests.Integration.Syntax
{
    using System.Linq;
    using NUnit.Framework;
    using Reflection.Syntax;

    [TestFixture]
    public class CSharp_MethodSyntaxTests
    {
        private const string TestFile = @"documentationtest.dll";
        private const string NamespaceName = "SyntaxTests";
        private const string TypeName = "ForMethod";

        private AssemblyDef _assemblyDef;

        private IFormatter CreateFormatter(MethodDef method)
        {
            return SyntaxFactory.Create(method, Languages.CSharp);
        }

        [OneTimeSetUp]
        public void InitialiseAssemblyDefFileUsedForTests()
        {
            string dir = System.AppDomain.CurrentDomain.BaseDirectory;
            _assemblyDef = AssemblyDef.Create(System.IO.Path.Combine(dir, TestFile));
        }

        [TestCase("PublicMethod", "public void PublicMethod()")]
        [TestCase("InternalMethod", "internal void InternalMethod()")]
        [TestCase("ProtectedInternalMethod", "internal protected void ProtectedInternalMethod()")]
        [TestCase("ProtectedMethod", "protected void ProtectedMethod()")]
        [TestCase("PrivateMethod", "private void PrivateMethod()")]
        public void CSharpSyntax_Method_AccessModifiers(string method, string expected)
        {
            TestIt(method, expected);
        }

        [TestCase("StaticMethod", "public static void StaticMethod()")]
        public void CSharpSyntax_Method_Static(string method, string expected)
        {
            TestIt(method, expected);
        }

        [TestCase("ReturnVoid", "public void ReturnVoid()")]
        [TestCase("ReturnWellKnownType", "public int ReturnWellKnownType()")]
        [TestCase("ReturnJaggedArray", "public int[][] ReturnJaggedArray()")]
        [TestCase("ReturnClass", "public ForMethod ReturnClass()")]
        [TestCase("ReturnArray", "public byte[] ReturnArray()")]
        [TestCase("ReturnGeneric", "public GenericClass<string, string, string> ReturnGeneric()")]
        public void CSharpSyntax_Method_ReturnTypes(string method, string expected)
        {
            TestIt(method, expected);
        }

        [TestCase("ParameterNormal", "public void ParameterNormal(\n\tint test\n\t)")]
        [TestCase("ParameterRef", "public void ParameterRef(\n\tref int test\n\t)")]
        [TestCase("ParameterOut", "public void ParameterOut(\n\tout int test\n\t)")]
        // bug 49 [TestCase("ParameterDefault", "public void ParameterDefault(\n\tint test = 3\n\t)")]
        public void CSharpSyntax_Method_ParameterModifiers(string method, string expected)
        {
            TestIt(method, expected);
        }

        [TestCase("Generic", "public void Generic<T>()")]
        // bug 50
        //[TestCase("GenericWhereClass", "public void GenericWhereClass<T>() where T : class")]
        //[TestCase("GenericWhereStruct", "public void GenericWhereStruct<T>() where T : struct")]
        //[TestCase("GenericWhereInterface", "public void GenericWhereInteface<T>() where T : ITest")]
        //[TestCase("GenericWhereNew", "public void GenericWhereNew<T>() where T : new()")]
        //[TestCase("GenericWhereAll", "public void GenericWhereAll<T>() where T : class, ITest, new()")]
        public void CSharpSyntax_Method_Generics(string method, string expected)
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
