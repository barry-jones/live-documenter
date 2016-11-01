
namespace TheBoxSoftware.Reflection.Tests.Integration.Syntax
{
    using NUnit.Framework;
    using Reflection.Syntax;

    [TestFixture]
    public class CSharp_ClassSyntaxTests
    {
        private const string TestFile = @"source\testoutput\documentationtest.dll";

        private AssemblyDef _assemblyDef;

        private IFormatter CreateFormatter(TypeDef type)
        {
            return SyntaxFactory.Create(type, Languages.CSharp);
        }

        [OneTimeSetUp]
        public void InitialiseAssemblyDefFileUsedForTests()
        {
            _assemblyDef = AssemblyDef.Create(TestFile);
        }

        [TestCase("SyntaxTests", "ClassPublic", "public class ClassPublic")]
        [TestCase("SyntaxTests", "ClassInternal", "internal class ClassInternal")]
        [TestCase("SyntaxTests.ClassPublic", "ClassProtected", "protected class ClassProtected")]
        [TestCase("SyntaxTests.ClassPublic", "ClassPrivate", "private class ClassPrivate")]
        public void CSharpSyntax_Class_Visibility(string namespaceName, string typeName, string expected)
        {
            DoTest(namespaceName, typeName, expected);
        }

        [TestCase("SyntaxTests", "AbstractClass", "public abstract class AbstractClass")]
        [TestCase("SyntaxTests", "SealedClass", "public sealed class SealedClass")]
        [TestCase("SyntaxTests", "StaticClass", "public static class StaticClass")]
        // Bug 48 [TestCase("SyntaxTests.ContainerB", "NestedClass", "public new class NestedClass")]
        public void CSharpSyntax_Class_Modifiers(string namespaceName, string typeName, string expected)
        {
            DoTest(namespaceName, typeName, expected);
        }

        [TestCase("SyntaxTests", "DerivedClass", "public class DerivedClass : BaseClass")]
        [TestCase("SyntaxTests", "DerivedClassWithInterface", "public class DerivedClassWithInterface : BaseClass,\r\nITest")]
        public void CSharp_Class_BaseClasses(string namespaceName, string typeName, string expected)
        {
            DoTest(namespaceName, typeName, expected);
        }

        [TestCase("SyntaxTests", "GenericClass`1", "public class GenericClass<T>")]
        [TestCase("SyntaxTests", "GenericClass`2", "public class GenericClass<T,U>")]
        // Bug 47
        //[TestCase("SyntaxTests", "GenericClassWhereNew`1", "public class GenericClassWithWhere<T> where T : new()")]
        //[TestCase("SyntaxTests", "GenericClassWhereStruct`1", "public class GenericClassWithWhere<T> where T : stuct")]
        //[TestCase("SyntaxTests", "GenericClassWhereClass`1", "public class GenericClassWithWhere<T> where T : class")]
        //[TestCase("SyntaxTests", "GenericClassWhereClassAndInteface`1", "public class GenericClassWithWhere<T> where T : class, ITest")]
        //[TestCase("SyntaxTests", "GenericClassWhereAll`1", "public class GenericClassWithWhere<T> where T : class, ITest, new()")]
        public void CSharp_Class_Generic(string namespaceName, string typeName, string expected)
        {
            DoTest(namespaceName, typeName, expected);
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
