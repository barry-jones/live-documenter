
namespace TheBoxSoftware.Reflection.Tests.Integration.Syntax
{
    using System.Linq;
    using NUnit.Framework;
    using Reflection.Syntax;

    [TestFixture]
    public class VB_MethodSyntaxTests
    {
        private const string TestFile = @"source\testoutput\documentationtest.dll";

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

        [TestCase("RefParameters", "public void RefParameters(\n\tfirst As Int\n\t)")]
        [TestCase("NormalParameters", "Public Sub NormalParameters(\n\tfirst As Int\n\t)")]
        [TestCase("OutParameter", "Public Sub OutParameter(\n\tfirst As Int\n\t)")]
        [TestCase("DefaultParameter", "Public Sub DefaultParameter(\n\tfirst As Int\n\t)")] // TODO: But default parameters - no default value provided
        [TestCase("MultipleParameters", "Public Sub MultipleParameters(\n\tfirst As Int,\n\tsecond As Int,\n\ttest As String,\n\tallOut As AllOutputTypesClass\n\t)")]
        public void Syntax_TestParametersAreCorrectlyCreated(string method, string expected)
        {
            TypeDef container = _assemblyDef.FindType("DocumentationTest", "AllOutputTypesClass");
            MethodDef testMethod = container.Methods.First(p => p.Name == method);
            IFormatter formatter = CreateFormatter(testMethod);

            SyntaxTokenCollection tokens = formatter.Format();

            Assert.AreEqual(expected, tokens.ToString());
        }
    }
}
