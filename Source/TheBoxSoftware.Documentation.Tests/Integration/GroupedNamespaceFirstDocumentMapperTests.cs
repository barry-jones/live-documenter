﻿
namespace TheBoxSoftware.Documentation.Tests.Integration
{
    using System.Collections.Generic;
    using NUnit.Framework;
    using Reflection;

    [TestFixture]
    public class GroupedNamespaceFirstDocumentMapperTests
    {
        private const string DocumentationFile = @"source\testoutput\documentationtest.dll";

        [Test]
        public void Create()
        {
            EntryCreator creator = new EntryCreator();
            bool useObservableCollection = false;
            List<DocumentedAssembly> documentedAssemblies = new List<DocumentedAssembly>();
            DocumentedAssembly documentedAssembly = new DocumentedAssembly();
            AssemblyDef assembly = new AssemblyDef();
            TypeInNamespaceMap map = assembly.Map;

            documentedAssemblies.Add(new DocumentedAssembly { FileName = DocumentationFile });

            GroupedNamespaceDocumentMapper mapper = new GroupedNamespaceDocumentMapper(
                documentedAssemblies, useObservableCollection, creator
                );

            mapper.GenerateMap();
        }
    }
}
