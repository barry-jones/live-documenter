
namespace TheBoxSoftware.Reflection.Tests.Unit
{
    using NUnit.Framework;

    [TestFixture]
    public class AssemblyDefTests
    {
        // Big methods to test on this class are the create methods which use the PeCoffFile which
        // currently can not be mocked out without some work.

        [Test]
        public void AssemblyDef()
        {
            AssemblyDef assemblyDef = new AssemblyDef();

            long id = assemblyDef.GetAssemblyId();

            Assert.AreEqual(0, id);
        }

        [Test]
        public void AssemblyDef_GetTypesInNamespace_WhenNoTypesOrNamespaces_ShouldReturnAZeroLengthList()
        {
            AssemblyDef assemblyDef = new AssemblyDef();

            var result = assemblyDef.GetTypesInNamespaces();

            Assert.AreEqual(0, result.Count);
        }
    }
}
