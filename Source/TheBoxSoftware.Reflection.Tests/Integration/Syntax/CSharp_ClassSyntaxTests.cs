
namespace TheBoxSoftware.Reflection.Tests.Integration.Syntax
{
    using NUnit.Framework;
    using Reflection.Syntax;

    [TestFixture]
    public class CSharp_ClassSyntaxTests
    {
        private const string TestFile = @"..\..\..\testoutput\documentationtest.dll";

        private AssemblyDef _assemblyDef;

        private IFormatter CreateFormatter(TypeDef type)
        {
            return SyntaxFactory.Create(type, Languages.CSharp);
        }

        [OneTimeSetUp]
        public void InitialiseAssemblyDefFileUsedForTests()
        {
            string dir = System.AppDomain.CurrentDomain.BaseDirectory;
            _assemblyDef = AssemblyDef.Create(System.IO.Path.Combine(dir, TestFile));
        }

        [TestCase("SyntaxTests.ForClass", "ClassPublic", "public class ClassPublic")]
        [TestCase("SyntaxTests.ForClass", "ClassInternal", "internal class ClassInternal")]
        [TestCase("SyntaxTests.ForClass.ClassPublic", "ClassProtected", "protected class ClassProtected")]
        [TestCase("SyntaxTests.ForClass.ClassPublic", "ClassPrivate", "private class ClassPrivate")]
        public void CSharpSyntax_Class_Visibility(string namespaceName, string typeName, string expected)
        {
            DoTest(namespaceName, typeName, expected);
        }

        [TestCase("SyntaxTests.ForClass", "AbstractClass", "public abstract class AbstractClass")]
        [TestCase("SyntaxTests.ForClass", "SealedClass", "public sealed class SealedClass")]
        [TestCase("SyntaxTests.ForClass", "StaticClass", "public static class StaticClass")]
        // Bug 48 [TestCase("SyntaxTests.ForClass.ContainerB", "NestedClass", "public new class NestedClass")]
        public void CSharpSyntax_Class_Modifiers(string namespaceName, string typeName, string expected)
        {
            DoTest(namespaceName, typeName, expected);
        }

        [TestCase("SyntaxTests.ForClass", "DerivedClass", "public class DerivedClass : BaseClass")]
        [TestCase("SyntaxTests.ForClass", "DerivedClassWithInterface", "public class DerivedClassWithInterface : BaseClass,\n\tITest")]
        public void CSharpSyntax_Class_BaseClasses(string namespaceName, string typeName, string expected)
        {
            DoTest(namespaceName, typeName, expected);
        }

        [TestCase("SyntaxTests.ForClass", "GenericClass`1", "public class GenericClass<T>")]
        [TestCase("SyntaxTests.ForClass", "GenericClass`2", "public class GenericClass<T,U>")]
        // Bug 47
        //[TestCase("SyntaxTests.ForClass", "GenericClassWhereNew`1", "public class GenericClassWithWhere<T> where T : new()")]
        //[TestCase("SyntaxTests.ForClass", "GenericClassWhereStruct`1", "public class GenericClassWithWhere<T> where T : stuct")]
        //[TestCase("SyntaxTests.ForClass", "GenericClassWhereClass`1", "public class GenericClassWithWhere<T> where T : class")]
        //[TestCase("SyntaxTests.ForClass", "GenericClassWhereClassAndInteface`1", "public class GenericClassWithWhere<T> where T : class, ITest")]
        //[TestCase("SyntaxTests.ForClass", "GenericClassWhereAll`1", "public class GenericClassWithWhere<T> where T : class, ITest, new()")]
        public void CSharpSyntax_Class_Generic(string namespaceName, string typeName, string expected)
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
