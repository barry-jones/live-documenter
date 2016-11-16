
namespace TheBoxSoftware.Reflection.Tests.Integration.Syntax
{
    using NUnit.Framework;
    using Reflection.Syntax;

    [TestFixture]
    public class VB_ClassSyntaxTests
    {
        private const string TestFile = @"..\..\..\testoutput\documentationtest.dll";

        private AssemblyDef _assemblyDef;

        private IFormatter CreateFormatter(TypeDef type)
        {
            return SyntaxFactory.Create(type, Languages.VisualBasic);
        }

        [OneTimeSetUp]
        public void InitialiseAssemblyDefFileUsedForTests()
        {
            string dir = System.AppDomain.CurrentDomain.BaseDirectory;
            _assemblyDef = AssemblyDef.Create(System.IO.Path.Combine(dir, TestFile));
        }

        [TestCase("SyntaxTests.ForClass", "ClassPublic", "Public Class ClassPublic")]
        [TestCase("SyntaxTests.ForClass", "ClassInternal", "Friend Class ClassInternal")]
        [TestCase("SyntaxTests.ForClass.ClassPublic", "ClassProtected", "Protected Class ClassProtected")]
        [TestCase("SyntaxTests.ForClass.ClassPublic", "ClassPrivate", "Private Class ClassPrivate")]
        public void VBSyntax_Class_Visibility(string namespaceName, string typeName, string expected)
        {
            DoTest(namespaceName, typeName, expected);
        }

        [TestCase("SyntaxTests.ForClass", "AbstractClass", "Public MustInherit Class AbstractClass")]
        [TestCase("SyntaxTests.ForClass", "SealedClass", "Public NotInheritable Class SealedClass")]
        [TestCase("SyntaxTests.ForClass", "StaticClass", "Public Shared Class StaticClass")]
        // Bug 48 [TestCase("SyntaxTests.ForClass.ContainerB", "NestedClass", "public new class NestedClass")]
        public void VBSyntax_Class_Modifiers(string namespaceName, string typeName, string expected)
        {
            DoTest(namespaceName, typeName, expected);
        }

        [TestCase("SyntaxTests.ForClass", "DerivedClass", "Public Class DerivedClass _\n\tDerives BaseClass")]
        [TestCase("SyntaxTests.ForClass", "DerivedClassWithInterface", "Public Class DerivedClassWithInterface _\n\tDerives BaseClass _\n\tImplements ITest")]
        public void VBSyntax_Class_BaseClasses(string namespaceName, string typeName, string expected)
        {
            DoTest(namespaceName, typeName, expected);
        }

        [TestCase("GenericClass`1", "Public Class GenericClass(Of T)")]
        [TestCase("GenericClass`2", "Public Class GenericClass(Of T,U)")]
        // Bug 54
        //[TestCase("GenericClassWhereNew`1", "public class GenericClassWithWhere<T> where T : new()")]
        //[TestCase("GenericClassWhereStruct`1", "public class GenericClassWithWhere<T> where T : stuct")]
        //[TestCase("GenericClassWhereClass`1", "public class GenericClassWithWhere<T> where T : class")]
        //[TestCase("GenericClassWhereClassAndInteface`1", "public class GenericClassWithWhere<T> where T : class, ITest")]
        //[TestCase("GenericClassWhereAll`1", "public class GenericClassWithWhere<T> where T : class, ITest, new()")]
        public void VBSyntax_Class_Generic(string typeName, string expected)
        {
            DoTest("SyntaxTests.ForClass", typeName, expected);
        }

        private void DoTest(string space, string type, string expected)
        {
            TypeDef typeDef = _assemblyDef.FindType(space, type);
            IFormatter formatter = CreateFormatter(typeDef);

            string result = formatter.Format().ToString();

            Assert.AreEqual(expected, result);
        }
    }
}
