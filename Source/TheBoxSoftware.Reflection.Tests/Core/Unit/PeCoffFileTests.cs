using System;
using NUnit.Framework;
using TheBoxSoftware.Reflection.Core;
using TheBoxSoftware.Reflection.Core.COFF;

namespace TheBoxSoftware.Reflection.Tests.Core.Unit
{
    [TestFixture]
    public class PeCoffFileTests
    {
        private const string TEST_LIBRARY = "theboxsoftware.reflection.tests\\bin\\debug\\theboxsoftware.reflection.dll";

        [Test]
        public void PeCoffFile_WhenCreatedWithEmptyString_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(delegate() {
                PeCoffFile coffFile = new PeCoffFile(string.Empty);
                coffFile.Initialise();
                });
        }

        [Test]
        public void PeCoffFile_WhenCreatedWithValidFile_MetadataIsLoaded()
        {
            PeCoffFile coffFile = new PeCoffFile(TEST_LIBRARY);
            coffFile.Initialise();

            Assert.AreEqual(TEST_LIBRARY, coffFile.FileName);
            Assert.IsTrue(coffFile.IsMetadataLoaded);
        }

        [Test]
        public void PeCoffFile_GetMetadataDirectory_WhenLoaded_ReturnsMetadata()
        {
            PeCoffFile coffFile = new PeCoffFile(TEST_LIBRARY);
            coffFile.Initialise();

            MetadataDirectory directory = coffFile.GetMetadataDirectory();

            Assert.IsNotNull(directory);
        }
    }
}
