
namespace TheBoxSoftware.Documentation.Tests.Unit
{
    using NUnit.Framework;
    using System.Collections.Generic;
    using TheBoxSoftware.Documentation;

    public class LibraryFileReaderTests
    {
        [Test]
        public void LibraryFileReader_Create()
        {
            new LibraryFileReader(string.Empty);
        }

        [Test]
        public void LibraryFileReader_WhenReadProvidedFileName_IsInDocumentedAssembly()
        {
            const string FileName = "this.dll";

            LibraryFileReader reader = new LibraryFileReader(FileName);

            List<DocumentedAssembly> assemblies = reader.Read();

            Assert.That(1, Is.EqualTo(assemblies.Count));
            Assert.That(FileName, Is.EqualTo(assemblies[0].FileName));
        }
    }
}
