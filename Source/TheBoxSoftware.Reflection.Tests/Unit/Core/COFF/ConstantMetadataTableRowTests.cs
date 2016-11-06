
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using Moq;
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Core.COFF;

    [TestFixture]
    public class ConstantMetadataTableRowTests
    {
        [Test]
        public void Constant_WhenCreated_ValuesAreReadCorrectly()
        {
            Mock<ICodedIndexResolver> resolver = CreateResolver();

            byte[] content = new byte[] {
                0x01,
                0x00,
                0x05, 0x00,
                0x00, 0x00
            };
            ConstantMetadataTableRow row = new ConstantMetadataTableRow(content, 0, resolver.Object, 2);

            Assert.AreEqual(Reflection.Signitures.ElementTypes.Void, row.Type);
            Assert.AreEqual(MetadataTables.Param, row.Parent.Table);
            Assert.AreEqual(1, row.Parent.Index.Value);
            Assert.AreEqual(0, row.Value.Value);
        }

        [Test]
        public void Constant_WhenCreated_OffsetIsMovedOn()
        {
            Mock<ICodedIndexResolver> resolver = CreateResolver();
            Offset offset = 0;
            byte[] content = new byte[30];

            ConstantMetadataTableRow row = new ConstantMetadataTableRow(content, offset, resolver.Object, 2);

            Assert.AreEqual(6, offset.Current);
        }

        private Mock<ICodedIndexResolver> CreateResolver()
        {
            Mock<ICodedIndexResolver> resolver = new Mock<ICodedIndexResolver>();
            CodedIndex codedIndex = new CodedIndex();

            codedIndex.Index = new Index(1);
            codedIndex.Table = MetadataTables.Param;

            resolver.Setup(p => p.GetSizeOfIndex(It.IsAny<CodedIndexes>())).Returns(2);
            resolver.Setup(p => p.Resolve(It.IsAny<CodedIndexes>(), It.IsAny<uint>())).Returns(codedIndex);

            return resolver;
        }
    }
}
