
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using System.Collections.Generic;
    using NUnit.Framework;
    using Reflection.Core.COFF;

    [TestFixture]
    public class CodedIndexResolverTests
    {
        [Test]
        public void Resolve_WhenTypeDefOrRefCodedIndexAndStoresIn2Bytes_Resolves()
        {
            CodedIndexes codedIndexType = CodedIndexes.TypeDefOrRef;

            Dictionary<MetadataTables, int> rowsPerTable = new Dictionary<MetadataTables, int>();
            rowsPerTable.Add(MetadataTables.TypeDef, 10);
            rowsPerTable.Add(MetadataTables.TypeRef, 15);

            uint indexToResolve = 0x11; // 0001 00[01]

            CodedIndexResolver resolver = new CodedIndexResolver(rowsPerTable);

            CodedIndex index = resolver.Resolve(codedIndexType, indexToResolve);

            Assert.AreEqual(MetadataTables.TypeRef, index.Table);
            Assert.AreEqual(4, index.Index.Value);
        }
    }
}
