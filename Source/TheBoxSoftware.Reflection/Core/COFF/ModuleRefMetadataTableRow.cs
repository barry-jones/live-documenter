
namespace TheBoxSoftware.Reflection.Core.COFF
{
    /// <summary>
    /// Created by a .module extern directive in the assembly
    /// </summary>
    public class ModuleRefMetadataTableRow : MetadataRow
    {
        private StringIndex _name;

        /// <summary>
        /// Initialises a new instance of the ModuleRefMetadataTableRow class
        /// </summary>
        /// <param name="contents">The contents of the file</param>
        /// <param name="offset">The offset of the current row</param>
        public ModuleRefMetadataTableRow(byte[] contents, Offset offset, byte sizeOfStringIndex)
        {
            this.FileOffset = offset;

            _name = new StringIndex(contents, sizeOfStringIndex, offset);
        }

        /// <summary>
        /// An index in to the string heap
        /// </summary>
        public StringIndex Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}