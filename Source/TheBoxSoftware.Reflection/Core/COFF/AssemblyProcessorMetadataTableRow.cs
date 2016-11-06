
namespace TheBoxSoftware.Reflection.Core.COFF
{
    // this record should not appear in the PE file but if present should be zero

    public class AssemblyProcessorMetadataTableRow : MetadataRow
    {
        private uint _processor;

        /// <summary>
        /// Initialises a new instance of the AssemblyProcessorMetadataTableRow class
        /// </summary>
        /// <param name="contents">The contents of the file</param>
        /// <param name="offset">The offset to the current row</param>
        public AssemblyProcessorMetadataTableRow(byte[] contents, Offset offset)
        {
            this.FileOffset = offset;

            offset.Shift(4);

            _processor = 0;
        }

        /// <summary>
        /// 4-byte constant
        /// </summary>
        public uint Processor
        {
            get { return _processor; }
            set { _processor = value; }
        }
    }
}