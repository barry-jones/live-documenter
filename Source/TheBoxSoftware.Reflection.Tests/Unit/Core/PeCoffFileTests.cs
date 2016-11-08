
namespace TheBoxSoftware.Reflection.Tests.Unit.Core
{
    using System;
    using Moq;
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Core.COFF;

    [TestFixture]
    public class PeCoffFileTests
    {
        private const string TEST_LIBRARY = @"source\testoutput\documentationtest.dll";

        [Test]
        public void PeCoffFile_WhenInitialisedWithEmptyString_ThrowsArgumentException()
        {
            Mock<IFileSystem> fileSystem = new Mock<IFileSystem>();
            Assert.Throws<ArgumentException>(delegate() {
                PeCoffFile coffFile = new PeCoffFile(string.Empty, fileSystem.Object);
                coffFile.Initialise();
                });
        }

        [Test]
        public void PeCoffFile_WhenInitialisedWithValidFile_MetadataIsLoaded()
        {
            PeCoffFile coffFile = new PeCoffFile(TEST_LIBRARY, new FileSystem());
            coffFile.Initialise();

            Assert.AreEqual(TEST_LIBRARY, coffFile.FileName);
            Assert.IsTrue(coffFile.IsMetadataLoaded);
        }

        [Test]
        public void PeCoffFile_GetMetadataDirectory_WhenLoaded_ReturnsMetadata()
        {
            PeCoffFile coffFile = new PeCoffFile(TEST_LIBRARY, new FileSystem());
            coffFile.Initialise();

            MetadataDirectory directory = coffFile.GetMetadataDirectory();

            Assert.IsNotNull(directory);
        }
    }
}
