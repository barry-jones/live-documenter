﻿
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using NUnit.Framework;
    using Helpers;
    using Reflection.Core;
    using Reflection.Core.COFF;

    [TestFixture]
    public class MemberRefMetadataTableRowTests
    {
        [Test]
        public void MemberRef_WhenCreated_ReadFieldsCorrectly()
        {
            byte[] contents = new byte[] {
                0x00, 0x00, 
                0x01, 0x00,
                0x02, 0x00
            };
            ICodedIndexResolver resolver = IndexHelper.CreateCodedIndexResolver(2);
            IIndexDetails indexDetails = IndexHelper.CreateIndexDetails(2);

            MemberRefMetadataTableRow row = new MemberRefMetadataTableRow(contents, 0, resolver, indexDetails);

            Assert.IsNotNull(row.Class);
            Assert.AreEqual(1, row.Name.Value);
            Assert.AreEqual(2, row.Signiture.Value);
        }

        [Test]
        public void MemberRef_WhenCreated_OffsetIsMovedOn()
        {
            byte[] contents = new byte[10];
            ICodedIndexResolver resolver = IndexHelper.CreateCodedIndexResolver(2);
            IIndexDetails indexDetails = IndexHelper.CreateIndexDetails(2);
            Offset offset = 0;

            MemberRefMetadataTableRow row = new MemberRefMetadataTableRow(contents, offset, resolver, indexDetails);

            Assert.AreEqual(6, offset.Current);
        }
    }
}
