
namespace TheBoxSoftware.Reflection.Core.COFF
{
    public class PropertyMapMetadataTableRow : MetadataRow
    {
        private Index _parent;
        private Index _propertyList;

        public PropertyMapMetadataTableRow()
        {
        }

        /// <summary>
        /// Initialises a new instance of the PropertyMapMetadataTableRow
        /// </summary>
        /// <param name="contents">The contents of the file</param>
        /// <param name="offset">The offset for this row</param>
        public PropertyMapMetadataTableRow(byte[] contents, Offset offset, int sizeOfTypeDefIndex, int sizeOfPropertyIndex)
        {
            this.FileOffset = offset;

            _parent = new Index(contents, offset, sizeOfTypeDefIndex);
            _propertyList = new Index(contents, offset, sizeOfPropertyIndex);
        }

        /// <summary>
        /// An index in to the TypeDef table
        /// </summary>
        public Index Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        /// <summary>
        /// An index in to the Property table
        /// </summary>
        public Index PropertyList
        {
            get { return _propertyList; }
            set { _propertyList = value; }
        }
    }
}