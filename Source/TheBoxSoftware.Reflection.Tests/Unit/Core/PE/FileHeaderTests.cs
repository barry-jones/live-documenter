
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.PE
{
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Core.PE;

    [TestFixture]
    public class FileHeaderTests
    {
        [Test]
        public void WhenCreated_ReadsAllFields()
        {
            byte[] real = new byte[] {
                0x64, 0x86,
                0x02, 0x00,
                0x87, 0xFA, 0x5C, 0x57,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0xF0, 0x00, 0x22, 0x20
            };
            Offset offset = 0;

            FileHeader header = new FileHeader(real, offset);

            Assert.AreEqual(MachineTypes.AMD64, header.Machine);
            Assert.AreEqual(2, header.NumberOfSections);
            Assert.AreEqual(0x575cfa87, header.TimeDateStamp);
            Assert.AreEqual(0, header.PointerToSymbolTable);
            Assert.AreEqual(0, header.NumberOfSymbols);
            Assert.AreEqual(240, header.SizeOfOptionalHeader);
            Assert.AreEqual(
                FileCharacteristics.ExecutableImage | FileCharacteristics.LargeAddressAware | FileCharacteristics.Dll,
                header.Characteristics);
        }

        [Test]
        public void WhenCreated_OffsetIsMovedOn()
        {
            byte[] real = new byte[20];
            Offset offset = 0;

            FileHeader header = new FileHeader(real, offset);

            Assert.AreEqual(20, offset.Current);
        }
    }
}
