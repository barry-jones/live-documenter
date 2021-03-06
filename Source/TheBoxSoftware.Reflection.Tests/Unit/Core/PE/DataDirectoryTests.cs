﻿
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.PE
{
    using System;
    using NUnit.Framework;
    using Reflection.Core.PE;

    [TestFixture]
    public class DataDirectoryTests
    {
        [Test]
        public void WhenNull_Create_ThrowsException()
        {
            byte[] data = null;
            DataDirectories directoryToInstantiate = DataDirectories.CertificateTable;

            Assert.Throws<NullReferenceException>(delegate ()
            {
                new DataDirectory(data, directoryToInstantiate);
            });
        }

        [Test]
        public void WhenIncorrectDataSize_Create_ThrowsException()
        {
            byte[] data = new byte[3];
            DataDirectories directoryToInstantiate = DataDirectories.CertificateTable;

            Assert.Throws<ArgumentException>(delegate ()
            {
                new DataDirectory(data, directoryToInstantiate);
            });
        }

        [Test]
        public void WhenValid_Created_Correctly()
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
        public void WhenVirtualAddressAndSizeAreZero_IsUsed_IsFalse()
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
        public void WhenVirtualAddressAndSizeAreNonZero_IsUsed_IsTrue()
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
