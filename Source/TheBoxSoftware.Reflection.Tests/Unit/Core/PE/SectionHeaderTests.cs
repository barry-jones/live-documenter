
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.PE
{
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Core.PE;

    [TestFixture]
    public class SectionHeaderTests
    {
        [Test]
        public void SectionHeader_WhenNameIsTerminated_NameDoesntContainTerminationCharacter()
        {
            byte[] contents = {
                (byte)'t', (byte)'e', (byte)'s', (byte)'t', (byte)'\0', 0, 0, // first 8 are name
                // just padd out the rest with zeros, they are not requried for the test
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
            };
            Offset offset = 0;

            SectionHeader sectionHeader = new SectionHeader(contents, offset);

            Assert.AreEqual(4, sectionHeader.Name.Length);
        }
    }
}
