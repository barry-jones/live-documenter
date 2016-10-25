
namespace TheBoxSoftware.Reflection.Tests.Integration.Syntax
{
    using System.Linq;
    using NUnit.Framework;
    using Reflection.Syntax;

    [TestFixture]
    public class CSharp_MethodSyntaxTests
    {
        private const string TestFile = @"source\solution items\documentationtest.dll";

        private AssemblyDef _assemblyDef;

        private IFormatter CreateFormatter(MethodDef method)
        {
            return SyntaxFactory.Create(method, Languages.CSharp);
        }

        [OneTimeSetUp]
        public void InitialiseAssemblyDefFileUsedForTests()
        {
            _assemblyDef = AssemblyDef.Create(TestFile);
        }

        [TestCase("BasicPublicMethod", "public void BasicPublicMethod()")]
        [TestCase("BasicInternalMethod", "internal void BasicInternalMethod()")]
        [TestCase("BasicProtectedInternalMethod", "internal protected void BasicProtectedInternalMethod()")]
        [TestCase("BasicProtectedMethod", "protected void BasicProtectedMethod()")]
        [TestCase("BasicPrivateMethod", "private void BasicPrivateMethod()")]
        public void Syntax_TestMethodVisibilityIsCorrectlyCreated(string method, string expected)
        {
            TypeDef container = _assemblyDef.FindType("DocumentationTest", "AllOutputTypesClass");
            MethodDef testMethod = container.Methods.First(p => p.Name == method);
            IFormatter formatter = CreateFormatter(testMethod);

            SyntaxTokenCollection tokens = formatter.Format();

            Assert.AreEqual(expected, tokens.ToString());
        }

        [TestCase("BasicPublicMethod", "public void BasicPublicMethod()")]
        [TestCase("f", "public int f()")]
        [TestCase("JaggedReturnArray", "public string[][] JaggedReturnArray(\n\tstring[][] jaggy\n\t)")]
        [TestCase("ReturnsOurClass", "public AllOutputTypesClass ReturnsOurClass()")]
        [TestCase("BuildInLongTypeReturned", "public long BuildInLongTypeReturned()")]
        [TestCase("ArrayReturnType", "public int[] ArrayReturnType()")]
        public void Syntax_TestMethodReturnsTypesAreCorrectlyCreated(string method, string expected)
        {
            TypeDef container = _assemblyDef.FindType("DocumentationTest", "AllOutputTypesClass");
            MethodDef testMethod = container.Methods.First(p => p.Name == method);
            IFormatter formatter = CreateFormatter(testMethod);

            SyntaxTokenCollection tokens = formatter.Format();

            Assert.AreEqual(expected, tokens.ToString());
        }

        [TestCase("RefParameters", "public void RefParameters(\n\tref int first\n\t)")]
        [TestCase("NormalParameters", "public void NormalParameters(\n\tint first\n\t)")]
        [TestCase("OutParameter", "public void OutParameter(\n\tref int first\n\t)")] // TODO: Bug Out parameters - ref not out
        [TestCase("DefaultParameter", "public void DefaultParameter(\n\tint first\n\t)")] // TODO: But default parameters - no default value provided
        [TestCase("MultipleParameters", "public void MultipleParameters(\n\tint first,\n\tref int second,\n\tstring test,\n\tAllOutputTypesClass allOut\n\t)")]
        public void Syntax_TestParametersAreCorrectlyCreated(string method, string expected)
        {
            TypeDef container = _assemblyDef.FindType("DocumentationTest", "AllOutputTypesClass");
            MethodDef testMethod = container.Methods.First(p => p.Name == method);
            IFormatter formatter = CreateFormatter(testMethod);

            SyntaxTokenCollection tokens = formatter.Format();

            Assert.AreEqual(expected, tokens.ToString());
        }

        [TestCase("GenericReturnType", "public List<int> GenericReturnType()")]
        [TestCase("GenericMethodOfT", "public List<T> GenericMethodOfT<T>()")]
        public void Syntax_TestMethodsForGenerics(string method, string expected)
        {
            TypeDef container = _assemblyDef.FindType("DocumentationTest", "AllOutputTypesClass");
            MethodDef testMethod = container.Methods.First(p => p.Name == method);
            IFormatter formatter = CreateFormatter(testMethod);

            SyntaxTokenCollection tokens = formatter.Format();

            Assert.AreEqual(expected, tokens.ToString());
        }
    }
}
