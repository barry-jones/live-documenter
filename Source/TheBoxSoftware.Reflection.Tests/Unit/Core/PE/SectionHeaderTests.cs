
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

        [Test]
        public void SectionHeader_WhenNameUsesAll8BytesWithNoTerminator_NameIs8CharactersLong()
        {
            byte[] contents = new byte[] {
                (byte)'a',(byte)'b',(byte)'c',(byte)'d',(byte)'e',(byte)'f',(byte)'g',(byte)'h', 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0
            };
            Offset offset = 0;

            SectionHeader sectionHeader = new SectionHeader(contents, offset);

            Assert.AreEqual(8, sectionHeader.Name.Length);
            Assert.AreEqual("abcdefgh", sectionHeader.Name);
        }

        [Test]
        public void SectionHeader_WhenCreated_OffsetIsMovedOn()
        {
            byte[] contents = new byte[50];
            Offset offset = 0;

            SectionHeader header = new SectionHeader(contents, offset);

            Assert.AreEqual(40, offset.Current);
        }
    }
}
