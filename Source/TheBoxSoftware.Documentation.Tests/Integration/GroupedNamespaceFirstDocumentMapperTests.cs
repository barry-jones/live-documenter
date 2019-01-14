
namespace TheBoxSoftware.Documentation.Tests.Integration
{
    using System.Collections.Generic;
    using NUnit.Framework;
    using Reflection;

    [TestFixture]
    public class GroupedNamespaceFirstDocumentMapperTests
    {
        private readonly string DocumentationFile = @"documentationtest.dll";

        public GroupedNamespaceFirstDocumentMapperTests()
        {
            string dir = System.AppDomain.CurrentDomain.BaseDirectory;
            DocumentationFile = System.IO.Path.Combine(dir, DocumentationFile);
        }

        [Test]
        public void GenerateMap_WhenAssemblyHasTypesWithoutNamespace_TypesAreContainedInNoneNamespaceContainer()
        {
            const string TypeName = "Issue45_TypeWithNoNamespace";
            const string NoneNamespaceName = "NoneNamespaces";

            List<DocumentedAssembly> assemblies = new List<DocumentedAssembly>();
            DocumentedAssembly documentedAssembly = new DocumentedAssembly { FileName = DocumentationFile };
            EntryCreator creator = new EntryCreator();

            assemblies.Add(documentedAssembly);

            GroupedNamespaceDocumentMapper mapper = new GroupedNamespaceDocumentMapper(assemblies, false, creator);

            DocumentMap result = mapper.GenerateMap();

            AssemblyDef assembly = documentedAssembly.LoadedAssembly;

            TypeDef type = assembly.FindType(string.Empty, TypeName);
            Entry entry = result.FindById(type.GetGloballyUniqueId());

            Assert.AreSame(type, entry.Item); // the type has been mapped
            Assert.AreEqual(NoneNamespaceName, entry.Parent.Parent.SubKey); // is part of the nonenamespace container
        }
    }
}
