
namespace TheBoxSoftware.Reflection.Tests.Unit.Core
{
    using System;
    using System.Collections.Generic;
    using Moq;
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Core.COFF;
    using Reflection.Core.PE;

    [TestFixture]
    public class PeCoffFileTests
    {
        private const string TEST_LIBRARY = @"source\testoutput\documentationtest.dll";

        [Test]
        public void WhenInitialisedWithEmptyString_ThrowsArgumentException()
        {
            Mock<IFileSystem> fileSystem = new Mock<IFileSystem>();
            Assert.Throws<ArgumentException>(delegate ()
            {
                PeCoffFile coffFile = new PeCoffFile(string.Empty, fileSystem.Object);
                coffFile.Initialise();
            });
        }

        [Test]
        public void WhenInitialisedWithValidFile_MetadataIsLoaded()
        {
            PeCoffFile coffFile = new PeCoffFile(TEST_LIBRARY, new FileSystem());
            coffFile.Initialise();

            Assert.AreEqual(TEST_LIBRARY, coffFile.FileName);
            Assert.IsTrue(coffFile.IsMetadataLoaded);
        }
        
        [Test]
        public void WhenLoaded_GetMetadataDirectory_ReturnsMetadata()
        {
            PeCoffFile coffFile = new PeCoffFile(TEST_LIBRARY, new FileSystem());
            coffFile.Initialise();

            MetadataDirectory directory = coffFile.GetMetadataDirectory();

            Assert.IsNotNull(directory);
        }

        [Test]
        public void WhenNoSectionsRvaCantBeComputed_GetRva_ThrowsException()
        {
            Mock<IFileSystem> fileSystem = new Mock<IFileSystem>();
            PeCoffFile coffFile = new PeCoffFile("doesnt-matter.dll", fileSystem.Object);

            // section headers need to be set up to be able work out the rva
            coffFile.SectionHeaders = new List<SectionHeader>();

            Assert.Throws<InvalidOperationException>(delegate ()
            {
                coffFile.GetAddressFromRVA(0x00000000);
            });
        }
        
        [Test]
        public void WhenHasHeadersButRvaIsOutsideRange_GetRva_ThrowsException()
        {
            Mock<IFileSystem> fileSystem = new Mock<IFileSystem>();
            PeCoffFile coffFile = new PeCoffFile("doesnt-matter.dll", fileSystem.Object);

            // section headers need to be set up to be able work out the rva
            coffFile.SectionHeaders = new List<SectionHeader>();
            coffFile.SectionHeaders.Add(CreateSectionHeader(0x0000c000, 0x00000001, 0x0));

            Assert.Throws<InvalidOperationException>(delegate ()
            {
                coffFile.GetAddressFromRVA(0x00000b00);
            });
        }

        [Test]
        public void WhenHeaderStartIsInRangeButRvaIsOutside_GetRva_ThrowsException()
        {
            Mock<IFileSystem> fileSystem = new Mock<IFileSystem>();
            PeCoffFile coffFile = new PeCoffFile("doesnt-matter.dll", fileSystem.Object);

            // section headers need to be set up to be able work out the rva
            coffFile.SectionHeaders = new List<SectionHeader>();
            coffFile.SectionHeaders.Add(CreateSectionHeader(0x0000c000, 0x00000001, 0x0));

            Assert.Throws<InvalidOperationException>(delegate ()
            {
                coffFile.GetAddressFromRVA(0x00000c02);
            });
        }

        [TestCase(0x0000c002, 0x00000202)] // inside range
        [TestCase(0x0000c000, 0x00000200)] // bottom of range
        [TestCase(0x0000d000, 0x00001200)] // top of range
        public void WhenHeaderIsInRange_GetRva_AddressIsCalculated(int rva, int expected)
        {
            Mock<IFileSystem> fileSystem = new Mock<IFileSystem>();
            PeCoffFile coffFile = new PeCoffFile("doesnt-matter.dll", fileSystem.Object);

            // section headers need to be set up to be able work out the rva
            coffFile.SectionHeaders = new List<SectionHeader>();
            coffFile.SectionHeaders.Add(CreateSectionHeader(0x0000c000, 0x0000d000, 0x00000200));

            uint result = coffFile.GetAddressFromRVA((uint)rva);

            Assert.AreEqual(expected, result);
        }

        [TestCase(0x0000c002)] // inside range
        [TestCase(0x0000c000)] // bottom of range
        [TestCase(0x0000d000)] // top of range
        public void WhenHeaderIsInRange_CanGetRva_IsTrue(int rva)
        {
            Mock<IFileSystem> fileSystem = new Mock<IFileSystem>();
            PeCoffFile coffFile = new PeCoffFile("doesnt-matter.dll", fileSystem.Object);

            // section headers need to be set up to be able work out the rva
            coffFile.SectionHeaders = new List<SectionHeader>();
            coffFile.SectionHeaders.Add(CreateSectionHeader(0x0000c000, 0x0000d000, 0x00000200));

            bool result = coffFile.CanGetAddressFromRva((uint)rva);

            Assert.AreEqual(true, result);
        }

        [Test]
        public void WhenHeaderOutOfRange_CanGetRva_IsFalse()
        {
            Mock<IFileSystem> fileSystem = new Mock<IFileSystem>();
            PeCoffFile coffFile = new PeCoffFile("doesnt-matter.dll", fileSystem.Object);

            // section headers need to be set up to be able work out the rva
            coffFile.SectionHeaders = new List<SectionHeader>();
            coffFile.SectionHeaders.Add(CreateSectionHeader(0x0000c000, 0x0000d000, 0x00000200));

            bool result = coffFile.CanGetAddressFromRva(0x00000001);

            Assert.AreEqual(false, result);
        }

        private static SectionHeader CreateSectionHeader(uint virtualAddress, uint size, uint startOfData)
        {
            SectionHeader header = new SectionHeader();
            header.VirtualAddress = virtualAddress;
            header.SizeOfRawData = size;
            header.PointerToRawData = startOfData;
            return header;
        }
    }
}
