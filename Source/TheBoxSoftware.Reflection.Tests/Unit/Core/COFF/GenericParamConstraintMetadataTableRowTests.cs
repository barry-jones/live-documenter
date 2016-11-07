
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Core.COFF;
    using Helpers;

    [TestFixture]
    public class GenericParamConstraintMetadataTableRowTests
    {
        [Test]
        public void GenericConstraint_WhenCreated_ReadsFieldsCorrectly()
        {
            byte[] contents = new byte[] {
                0x01, 0x00,
                0x01, 0x00
            };
            ICodedIndexResolver resolver = CodedIndexHelper.CreateCodedIndexResolver(2);

            GenericParamConstraintMetadataTableRow row = new GenericParamConstraintMetadataTableRow(contents, 0, resolver, 2);

            Assert.AreEqual(1, row.Owner.Value);
            Assert.IsNotNull(row.Owner);
        }

        [Test]
        public void GenericConstraint_WhenCreated_OffsetIsMovedOn()
        {
            byte[] contents = new byte[10];
            Offset offset = 0;
            ICodedIndexResolver resolver = CodedIndexHelper.CreateCodedIndexResolver(2);

            GenericParamConstraintMetadataTableRow row = new GenericParamConstraintMetadataTableRow(contents, offset, resolver, 2);

            Assert.AreEqual(4, offset.Current);
        }
    }
}
