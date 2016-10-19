using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TheBoxSoftware.Reflection.Signitures;

namespace TheBoxSoftware.Reflection.Tests.Integration
{
    [TestFixture]
    public class SignatureTests
    {
        private const string TestFile = @"source\solution items\documentationtest.dll";

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
            Signiture sig = method.Signiture;

            Assert.AreEqual(string.Empty, sig.ToString());
        }
    }
}
