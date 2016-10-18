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
            PeCoffFile coffFile = new PeCoffFile("theboxsoftware.reflection.tests\\bin\\debug\\theboxsoftware.reflection.dll");

            Assert.AreEqual("theboxsoftware.reflection.tests\\bin\\debug\\theboxsoftware.reflection.dll", coffFile.FileName);
            Assert.IsTrue(coffFile.IsMetadataLoaded);
        }
    }
}
