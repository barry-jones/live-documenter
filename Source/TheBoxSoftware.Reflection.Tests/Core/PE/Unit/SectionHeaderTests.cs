using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TheBoxSoftware.Reflection.Core;
using TheBoxSoftware.Reflection.Core.PE;

namespace TheBoxSoftware.Reflection.Tests.Core.PE.Unit
{
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
