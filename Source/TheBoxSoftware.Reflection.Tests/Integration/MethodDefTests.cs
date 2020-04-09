
namespace TheBoxSoftware.Reflection.Tests.Integration
{
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public class MethodDefTests
    {
        private const string TestFile = @"documentationtest.dll";

        private AssemblyDef _assemblyDef;

        [OneTimeSetUp]
        public void InitialiseAssemblyDefFileUsedForTests()
        {
            string dir = System.AppDomain.CurrentDomain.BaseDirectory;
            _assemblyDef = AssemblyDef.Create(System.IO.Path.Combine(dir, TestFile));
        }


        [Test, Category("Integration")]
        public void Bug41_ReturnType_WhenJaggedArray_TypeReturned()
        {
            // [#41] arrays returns types but arrays of arrays and im sure,
            // arrays of arrays of arrays do not
            TypeDef type = _assemblyDef.FindType("DocumentationTest", "AllOutputTypesClass");
            MethodDef method = (from m in type.GetMethods()
                               where m.Name == "JaggedReturnArray"
                               select m).First();

            TypeRef returnType = method.GetReturnType();

            Assert.IsNotNull(method, "could not find method being tested.");
            Assert.IsNotNull(returnType, "return type could not be located.");
            Assert.AreEqual("String", returnType.Name);
        }

        [Test, Category("Integration")]
        public void ReturnType_WhenArray_TypeReturned()
        {
            // [#41] arrays returns types but arrays of arrays and im sure,
            // arrays of arrays of arrays do not
            TypeDef type = _assemblyDef.FindType("DocumentationTest", "AllOutputTypesClass");
            MethodDef method = (from m in type.GetMethods()
                                where m.Name == "ArrayReturnType"
                                select m).First();

            TypeRef returnType = method.GetReturnType();

            Assert.IsNotNull(method, "could not find method being tested.");
            Assert.IsNotNull(returnType, "return type could not be located.");
            Assert.AreEqual("Int32", returnType.Name);
        }
    }
}
