
namespace TheBoxSoftware.Reflection.Core.COFF
{
    public class NestedClassMetadataTableRow : MetadataRow
    {
        private Index _enclosingClass;
        private Index _nestedClass;

        /// <summary>
        /// Initialises a new instance of the NestedClassMetadataTableRow class
        /// </summary>
        /// <param name="contents">The contents of the file</param>
        /// <param name="offset">The offset of the current row</param>
        public NestedClassMetadataTableRow(byte[] contents, Offset offset, int sizeOfTypeDefIndex)
        {
            this.FileOffset = offset;

            _nestedClass = new Index(contents, offset, sizeOfTypeDefIndex);
            _enclosingClass = new Index(contents, offset, sizeOfTypeDefIndex);
        }

        /// <summary>
        /// An index in to the TypeDef table
        /// </summary>
        public Index NestedClass
        {
            get { return _nestedClass; }
            set { _nestedClass = value; }
        }

        /// <summary>
        /// An index in to teh TypeDef table
        /// </summary>
        public Index EnclosingClass
        {
            get { return _enclosingClass; }
            set { _enclosingClass = value; }
        }
    }
}