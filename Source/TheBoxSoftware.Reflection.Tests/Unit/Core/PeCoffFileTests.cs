
namespace TheBoxSoftware.Reflection.Tests.Unit.Core
{
    using System;
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Core.COFF;

    [TestFixture]
    public class PeCoffFileTests
    {
        private const string TEST_LIBRARY = @"source\solution items\documentationtest.dll";

        [Test]
        public void PeCoffFile_WhenInitialisedWithEmptyString_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(delegate() {
                PeCoffFile coffFile = new PeCoffFile(string.Empty);
                coffFile.Initialise();
                });
        }

        [Test]
        public void PeCoffFile_WhenInitialisedWithValidFile_MetadataIsLoaded()
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
