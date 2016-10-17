using NUnit.Framework;
using TheBoxSoftware.Reflection.Core;

namespace TheBoxSoftware.Reflections.Tests.Core.Unit
{
    [TestFixture]
    public class PeCoffFileTests
    {
        [Test]
        public void Create()
        {
            PeCoffFile coffFile = new PeCoffFile(string.Empty);
        }
    }
}
