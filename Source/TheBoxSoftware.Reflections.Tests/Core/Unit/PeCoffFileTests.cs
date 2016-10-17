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
    }
}
