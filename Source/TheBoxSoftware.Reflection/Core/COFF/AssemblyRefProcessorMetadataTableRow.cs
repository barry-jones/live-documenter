
namespace TheBoxSoftware.Reflection.Core.COFF
{
    // these records should not be emitted in to the PE file and if they are they should
    // treat all values as zero

    public class AssemblyRefProcessorMetadataTableRow : MetadataRow
    {
        private Index _assemblyRef;
        private uint _processor;

        /// <summary>
        /// Initialises a new instance of the AssemblyRefProcessorMetadataTableRow class
        /// </summary>
        /// <param name="contents">The contents of the file</param>
        /// <param name="offset">The offset of the current row</param>
        public AssemblyRefProcessorMetadataTableRow(byte[] contents, Offset offset, IIndexDetails indexDetails)
        {
            this.FileOffset = offset;

            byte sizeOfAssemblyRefIndex = indexDetails.GetSizeOfIndex(MetadataTables.AssemblyRef);

            offset.Shift(4);
            offset.Shift(sizeOfAssemblyRefIndex);

            _processor = 0;
            _assemblyRef = new Index();
        }

        public uint Processor
        {
            get { return _processor; }
            set { _processor = value; }
        }

        public Index AssemblyRef
        {
            get { return _assemblyRef; }
            set { _assemblyRef = value; }
        }
    }
}