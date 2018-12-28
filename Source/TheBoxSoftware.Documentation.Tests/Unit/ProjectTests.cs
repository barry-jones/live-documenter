
namespace TheBoxSoftware.Documentation.Tests.Unit
{
    using NUnit.Framework;
    using System.Collections.Generic;

    [TestFixture]
    public class ProjectTests
    {
        private Project CreateProject()
        {
            return new Project();
        }

        [Test]
        public void GetAssemblies_WhenProjectIsEmpty_ReturnsNothing()
        {
            Project project = CreateProject();

            List<DocumentedAssembly> result = project.GetAssemblies();

            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void GetAssemblies_WithASingleFile_ReturnsDetails()
        {
            Project project = CreateProject();

            project.AddFiles(new string[] { "test.dll" });

            List<DocumentedAssembly> result = project.GetAssemblies();

            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public void GetMissingFiles_WhenNoFiles_ReturnsNothing()
        {
            Project project = CreateProject();

            string[] result = project.GetMissingFiles();

            Assert.AreEqual(0, result.Length);
        }

        [Test]
        public void GetMissingFiles_WithOneMissingFile_ReturnsFilename()
        {
            const string FILENAME = "missing.dll";

            Project project = CreateProject();

            project.AddFiles(new string[] { FILENAME });

            string[] result = project.GetMissingFiles();

            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(FILENAME, result[0]);
        }
    }
}
