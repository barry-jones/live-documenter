using System;
using NUnit.Framework;
using TheBoxSoftware.Reflection.Core.PE;

namespace TheBoxSoftware.Reflection.Tests.Unit.Core.PE
{
    [TestFixture]
    public class DataDirectoryTests
    {
        [Test]
        public void DataDirectory_CreatedWithNullData_ThrowsException()
        {
            byte[] data = null;
            DataDirectories directoryToInstantiate = DataDirectories.CertificateTable;

            Assert.Throws<NullReferenceException>(delegate ()
            {
                new DataDirectory(data, directoryToInstantiate);
            });
        }

        [Test]
        public void DataDirectory_CreatedWithIncorrectDataSize_ThrowsException()
        {
            byte[] data = new byte[3];
            DataDirectories directoryToInstantiate = DataDirectories.CertificateTable;

            Assert.Throws<ArgumentException>(delegate ()
            {
                new DataDirectory(data, directoryToInstantiate);
            });
        }

        [Test]
        public void DataDirectory_CreatedWithValidData_CorrectlyPopulated()
        {
            byte[] data = {
                1, 0, 0, 0,
                1, 0, 0, 0
            };
            DataDirectories directoryToInstantiate = DataDirectories.CertificateTable;

            DataDirectory directory = new DataDirectory(data, directoryToInstantiate);

            Assert.AreEqual(1, directory.VirtualAddress);
            Assert.AreEqual(1, directory.Size);
        }

        [Test]
        public void DataDirectory_WhenVirtualAddressAndSizeAreZero_DirectoryIsNotUsed()
        {
            byte[] data = {
                0, 0, 0, 0,
                0, 0, 0, 0
            };
            DataDirectories directoryToInstantiate = DataDirectories.CertificateTable;

            DataDirectory directory = new DataDirectory(data, directoryToInstantiate);

            Assert.IsFalse(directory.IsUsed);
        }

        [Test]
        public void DataDirectory_WhenVirtualAddressAndSizeAreNonZero_DirectoryIsUsed()
        {
            byte[] data = {
                1, 0, 0, 0,
                1, 0, 0, 0
            };
            DataDirectories directoryToInstantiate = DataDirectories.CertificateTable;

            DataDirectory directory = new DataDirectory(data, directoryToInstantiate);

            Assert.IsTrue(directory.IsUsed);
        }
    }
}
