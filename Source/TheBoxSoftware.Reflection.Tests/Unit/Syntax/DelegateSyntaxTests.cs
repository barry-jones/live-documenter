
namespace TheBoxSoftware.Reflection.Tests.Unit.Syntax
{
    using NUnit.Framework;
    using Reflection.Syntax;

    [TestFixture]
    public class DelegateSyntaxTests
    {
        private const string TestFile = @"source\testoutput\documentationtest.dll";

        private TypeDef GetDelegateFromTestAssembly()
        {
            // having to load a entire library each time for a test is not great, need
            // to try and refactor the code to enabled this to become less painful
            AssemblyDef assemblyDef = AssemblyDef.Create(TestFile);
            return assemblyDef.FindType("DocumentationTest.AllOutputTypesClass", "D");
        }

        [TestCase("TestName", false, "TestName")]
        [TestCase("TestName`2", true, "TestName")]
        public void DelegateSyntax_GetIdentifier(string delegateName, bool isGeneric, string expectedResult)
        {
            TypeDef testDelegate = GetDelegateFromTestAssembly();
            DelegateSyntax syntax = new DelegateSyntax(testDelegate);

            testDelegate.Name = delegateName;
            testDelegate.IsGeneric = isGeneric;

            string result = syntax.GetIdentifier();

            Assert.AreEqual(expectedResult, result);
        }
    }
}
