
namespace TheBoxSoftware.Reflection.Core.COFF
{
    using System.Linq;
    using System.Collections.Generic;

    public class IndexDetails : IIndexDetails
    {
        private readonly Dictionary<MetadataTables, int> _rowsInTables;
        private readonly byte _sizeOfStringIndex;
        private readonly byte _sizeOfBlobIndex;
        private readonly byte _sizeOfGuidIndex;

        public IndexDetails(Dictionary<MetadataTables, int> rowCountsInTables, byte sizeOfStringIndex, byte sizeOfGuidIndex, byte sizeOfBlobIndex)
        {
            _rowsInTables = rowCountsInTables;
            _sizeOfStringIndex = sizeOfStringIndex;
            _sizeOfBlobIndex = sizeOfBlobIndex;
            _sizeOfGuidIndex = sizeOfGuidIndex;
        }

        public byte GetSizeOfIndex(MetadataTables forTable)
        {
            byte sizeOfIndex = 2;
            if(_rowsInTables.Keys.Contains(forTable) && _rowsInTables[forTable] > ushort.MaxValue)
            {
                sizeOfIndex = 4;
            }
            return sizeOfIndex;
        }

        public byte GetSizeOfStringIndex()
        {
            return _sizeOfStringIndex;
        }

        public byte GetSizeOfBlobIndex()
        {
            return _sizeOfBlobIndex;
        }

        public byte GetSizeOfGuidIndex()
        {
            return _sizeOfGuidIndex;
        }
    }
}
