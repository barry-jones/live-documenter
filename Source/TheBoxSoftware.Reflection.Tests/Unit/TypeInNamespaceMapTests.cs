
namespace TheBoxSoftware.Reflection.Tests.Unit
{
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    public class TypeInNamespaceMapTests
    {
        [Test]
        public void Create_ShouldHaveNoEntries()
        {
            TypeInNamespaceMap map = new TypeInNamespaceMap();

            Assert.AreEqual(0, map.GetAllNamespaces().Count);
        }

        [Test]
        public void Add_WhenNewNamespace_NamespaceAdded()
        {
            const int Expected = 1;
            TypeInNamespaceMap map = new TypeInNamespaceMap();

            map.Add(CreateType("New", "Type"));

            Assert.That(Expected == map.GetAllNamespaces().Count);
        }

        [Test]
        public void Add_WhenNamespaceExists_NoNamespaceAdded()
        {
            const int Expected = 1;
            TypeInNamespaceMap map = new TypeInNamespaceMap();

            map.Add(CreateType("New", "Type"));
            map.Add(CreateType("New", "SecondType"));

            Assert.That(Expected == map.GetAllNamespaces().Count);
        }

        [Test]
        public void GetAllNamespaces_WhenTypeTypesAddedWithSameNamespace_Returns1()
        {
            TypeInNamespaceMap map = new TypeInNamespaceMap();
            map.Add(CreateType("System", "First"));
            map.Add(CreateType("System", "Second"));

            List<string> namespaces = map.GetAllNamespaces();

            Assert.AreEqual(1, namespaces.Count);
            Assert.IsTrue(namespaces.Contains("System"));
        }

        [Test]
        public void GetAllNamespaces_WhenTwoTypesWithDifferentNamespacesAdded_Returns2()
        {
            TypeInNamespaceMap map = new TypeInNamespaceMap();
            map.Add(CreateType("System", "First"));
            map.Add(CreateType("System.IO", "First"));

            List<string> namespaces = map.GetAllNamespaces();

            Assert.AreEqual(2, namespaces.Count);
            Assert.IsTrue(namespaces.Contains("System"));
            Assert.IsTrue(namespaces.Contains("System.IO"));
        }

        [Test]
        public void GetAllTypesInNamespaces_WhenMapHasEntries_IsCorrect()
        {
            TypeInNamespaceMap map = BuildTestMap();

            Dictionary<string, List<TypeDef>> all = map.GetAllTypesInNamespaces();

            Assert.AreEqual(8, all.Keys.Count);
            Assert.AreEqual(1, all[""].Count);
            Assert.AreEqual(5, all["First"].Count);
        }

        [Test]
        public void GetAllTypesInNamespaces_WhenMapHasEntries_ReturnsAShallowCopy()
        {
            TypeInNamespaceMap map = BuildTestMap();
            Dictionary<string, List<TypeDef>> all = map.GetAllTypesInNamespaces();

            map.Add(CreateType("Another", "AnotherTestClass"));

            Assert.AreNotEqual(all.Keys.Count, map.GetAllNamespaces().Count);
        }

        [Test]
        public void FindTypeInNamespace_WhenNoNamespace_FindsEntry()
        {
            TypeInNamespaceMap map = BuildTestMap();

            TypeDef found = map.FindTypeInNamespace(string.Empty, "NonNamespaceType");

            Assert.IsNotNull(found);
            Assert.AreEqual("NonNamespaceType", found.Name);
        }

        [Test]
        public void FindTypeInNamespace_WhenSearchFails_ReturnsNull()
        {
            TypeInNamespaceMap map = BuildTestMap();

            TypeDef found1 = map.FindTypeInNamespace(string.Empty, string.Empty);
            TypeDef found2 = map.FindTypeInNamespace("Doesnt", "Exist");

            Assert.IsNull(found1);
            Assert.IsNull(found2);
        }

        [Test]
        public void FindTypeInNamesapce_WhenSearchSucceeds_FindsEntry()
        {
            TypeInNamespaceMap map = BuildTestMap();

            TypeDef found = map.FindTypeInNamespace("First", "Test7");

            Assert.IsNotNull(found);
            Assert.AreEqual("Test7", found.Name);
        }

        [Test]
        public void Remove_WhenItemInvalid_DoesNothing()
        {
            TypeInNamespaceMap map = BuildTestMap();
            int expected = map.GetAllTypesInNamespaces().Count;

            map.Remove(CreateType("Nope", "Test1"));

            int result = map.GetAllTypesInNamespaces().Count;

            Assert.AreEqual(result, expected);
        }

        [Test]
        public void Remove_WhenItemValid_RemovesItem()
        {
            TypeInNamespaceMap map = BuildTestMap();
            TypeDef itemToRemove = CreateType("First", "Test");

            map.Add(itemToRemove);
            int expected = map.GetAllTypesInNamespaces()["First"].Count - 1;

            map.Remove(itemToRemove);
            int result = map.GetAllTypesInNamespaces()["First"].Count;

            Assert.AreEqual(result, expected);
        }

        [Test]
        public void Remove_WhenLastItemInNamespaceRemoved_NamespaceNotInDictionary()
        {
            TypeInNamespaceMap map = new TypeInNamespaceMap();
            TypeDef testType = CreateType("Namespace", "MyType");

            map.Add(testType);
            map.Remove(testType);

            Assert.AreEqual(0, map.GetAllNamespaces().Count);
        }

        private TypeDef CreateType(string space, string name)
        {
            return new TypeDef
            {
                Namespace = space,
                Name = name
            };
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
