
namespace TheBoxSoftware.Documentation.Tests.Integration
{
    using System.Collections.Generic;
    using NUnit.Framework;
    using Moq;
    using Reflection;

    [TestFixture]
    public class GroupedNamespaceFirstDocumentMapperTests
    {
        private const string DocumentationFile = @"source\testoutput\documentationtest.dll";

        // [Test]
        public void Create()
        {
            Mock<IFileSystem> fileSystem = new Mock<IFileSystem>();

            EntryCreator creator = new EntryCreator();
            bool useObservableCollection = false;
            List<DocumentedAssembly> documentedAssemblies = new List<DocumentedAssembly>();
            DocumentedAssembly documentedAssembly = new DocumentedAssembly();
            AssemblyDef assembly = new AssemblyDef();
            TypeInNamespaceMap map = assembly.Map;

            documentedAssemblies.Add(new DocumentedAssembly { FileName = DocumentationFile, LoadedAssembly = assembly });

            // add the type we want to test
            TypeDef container = new TypeDef();
            container.Name = "Parent";
            container.Namespace = "Testing";
            container.Assembly = assembly;

            TypeDef child = new TypeDef();
            child.Name = "Nested";
            child.ContainingClass = container;
            container.Assembly = assembly;

            TypeDef noNamespace = new TypeDef();
            noNamespace.Name = "NoNamespace";
            noNamespace.Namespace = string.Empty;
            container.Assembly = assembly;

            map.Add(container);
            map.Add(child);
            map.Add(noNamespace);

            GroupedNamespaceDocumentMapper mapper = new GroupedNamespaceDocumentMapper(
                documentedAssemblies, useObservableCollection, creator, fileSystem.Object
                );

            DocumentMap result = mapper.GenerateMap();
        }
    }
}
