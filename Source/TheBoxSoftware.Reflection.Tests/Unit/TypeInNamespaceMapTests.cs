
namespace TheBoxSoftware.Reflection.Tests.Unit
{
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    public class TypeInNamespaceMapTests
    {
        [Test]
        public void TypeInNamespaceMap_Add_WhenAddingDef_ShouldBeAddedToMap()
        {
            const int ExpectedCount = 1;

            TypeInNamespaceMap map = new TypeInNamespaceMap();
            TypeDef typeDef = new TypeDef();
            typeDef.Namespace = string.Empty;

            map.Add(typeDef);

            Assert.AreEqual(ExpectedCount, map.GetAllNamespaces().Count);
        }

        [Test]
        public void TypeInNamespaceMap_GetAllNamespaces_WhenTwoTypesOneNamespace_ShouldReturnOne()
        {
            TypeInNamespaceMap map = new TypeInNamespaceMap();
            TypeDef def = new TypeDef();
            def.Namespace = "System";

            map.Add(def);
            map.Add(def);

            List<string> namespaces = map.GetAllNamespaces();

            Assert.AreEqual(1, namespaces.Count);
            Assert.IsTrue(namespaces.Contains("System"));
        }

        [Test]
        public void TypeInNamespaceMap_GetAllNamespaces_WhenTwoTypesTwoNamespace_ShouldReturnTwo()
        {
            TypeInNamespaceMap map = new TypeInNamespaceMap();
            TypeDef def1 = new TypeDef() { Namespace = "System" };
            TypeDef def2 = new TypeDef() { Namespace = "System.IO" };

            map.Add(def1);
            map.Add(def2);

            List<string> namespaces = map.GetAllNamespaces();

            Assert.AreEqual(2, namespaces.Count);
            Assert.IsTrue(namespaces.Contains("System"));
            Assert.IsTrue(namespaces.Contains("System.IO"));
        }

        [Test]
        public void TypeInNamespaceMap_GetAllTypesInNamespaces_WhenHasTypes_ShouldReturnDataCorrectly()
        {
            TypeInNamespaceMap map = BuildTestMap();

            Dictionary<string, List<TypeDef>> all = map.GetAllTypesInNamespaces();

            Assert.AreEqual(8, all.Keys.Count);
            Assert.AreEqual(1, all[""].Count);
            Assert.AreEqual(5, all["First"].Count);
        }

        [Test]
        public void TypeInNamespaceMap_GetAllTypesInNamespaces_WhenHasTypes_ShouldReturnAShallowCopy()
        {
            TypeInNamespaceMap map = BuildTestMap();

            Dictionary<string, List<TypeDef>> all = map.GetAllTypesInNamespaces();

            map.Add(new TypeDef() { Namespace = "Another", Name = "AnotherTestClass" });

            Assert.AreNotEqual(all.Keys.Count, map.GetAllNamespaces().Count);
        }

        [Test]
        public void TypeInNamespaceMap_FindTypeInNamespace_WhenTypeNotThere_ShouldReturnNull()
        {
            TypeInNamespaceMap map = BuildTestMap();

            TypeDef found1 = map.FindTypeInNamespace(string.Empty, string.Empty);
            TypeDef found2 = map.FindTypeInNamespace("Doesnt", "Exist");

            Assert.IsNull(found1);
            Assert.IsNull(found2);
        }

        [Test]
        public void TypeInNamespaceMap_FindTypeInNamespace_WhenTypeWithoutNamespace_ShouldReturnCorrect()
        {
            TypeInNamespaceMap map = BuildTestMap();

            TypeDef found = map.FindTypeInNamespace(string.Empty, "NonNamespaceType");

            Assert.IsNotNull(found);
            Assert.AreEqual("NonNamespaceType", found.Name);
        }

        [Test]
        public void TypeInNamespaceMap_FindTypeInNamespace_WhenTypeExists_ShouldReturnCorrect()
        {
            TypeInNamespaceMap map = BuildTestMap();

            TypeDef found = map.FindTypeInNamespace("First", "Test7");

            Assert.IsNotNull(found);
            Assert.AreEqual("Test7", found.Name);
        }

        private TypeInNamespaceMap BuildTestMap()
        {
            TypeInNamespaceMap map = new TypeInNamespaceMap();

            map.Add(new TypeDef() { Namespace = "", Name = "NonNamespaceType" });
            map.Add(new TypeDef() { Namespace = "First", Name = "Test1" });
            map.Add(new TypeDef() { Namespace = "First", Name = "Test2" });
            map.Add(new TypeDef() { Namespace = "First", Name = "Test3" });
            map.Add(new TypeDef() { Namespace = "Second", Name = "Test4" });
            map.Add(new TypeDef() { Namespace = "Third", Name = "Test5" });
            map.Add(new TypeDef() { Namespace = "First", Name = "Test6" });
            map.Add(new TypeDef() { Namespace = "First", Name = "Test7" });
            map.Add(new TypeDef() { Namespace = "Fourth", Name = "Test8" });
            map.Add(new TypeDef() { Namespace = "Fifth", Name = "Test9" });
            map.Add(new TypeDef() { Namespace = "Sixth", Name = "Test10" });
            map.Add(new TypeDef() { Namespace = "Seventh", Name = "Test11" });

            return map;
        }
    }
}
