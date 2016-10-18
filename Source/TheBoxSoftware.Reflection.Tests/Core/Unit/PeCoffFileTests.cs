using System;
using NUnit.Framework;
using TheBoxSoftware.Reflection.Core;

namespace TheBoxSoftware.Reflection.Tests.Core.Unit
{
    [TestFixture]
    public class PeCoffFileTests
    {
        [Test]
        public void PeCoffFile_WhenCreatedWithEmptyString_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(delegate() {
                PeCoffFile coffFile = new PeCoffFile(string.Empty);
                });
        }

        [Test]
        public void PeCoffFile_WhenCreatedWithValidFile_MetadataIsLoaded()
        {
            PeCoffFile coffFile = new PeCoffFile("theboxsoftware.reflections.tests\\testfiles\\system.dll");

            Assert.AreEqual("theboxsoftware.reflections.tests\\testfiles\\system.dll", coffFile.FileName);
            Assert.IsTrue(coffFile.IsMetadataLoaded);
        }
    }
}
