
namespace TheBoxSoftware.Reflection.Core.COFF
{
    using System;

    /// <remarks>Updated for 4-byte heap indexes</remarks>
    public class ModuleMetadataTableRow : MetadataRow
    {
        private int _encBaseId;
        private int _encId;
        private int _mvid;
        private StringIndex _name;
        private ushort _generation;

        /// <summary>
        /// Initialises a new instance of the ModuleMetadataTableRow class
        /// </summary>
        /// <param name="contents">The byte contents of the file</param>
        /// <param name="offset">The offset of this row</param>
        public ModuleMetadataTableRow(byte[] contents, Offset offset, IIndexDetails indexDetails)
        {
            this.FileOffset = offset;

            byte sizeOfGuidIndex = indexDetails.GetSizeOfGuidIndex();

            _generation = FieldReader.ToUInt16(contents, offset.Shift(2));
            _name = new StringIndex(contents, indexDetails.GetSizeOfStringIndex(), offset);
            _mvid = FieldReader.ToInt32(contents, offset.Shift(sizeOfGuidIndex), sizeOfGuidIndex);
            _encId = FieldReader.ToInt32(contents, offset.Shift(sizeOfGuidIndex), sizeOfGuidIndex);
            _encBaseId = FieldReader.ToInt32(contents, offset.Shift(sizeOfGuidIndex), sizeOfGuidIndex);
        }

        /// <summary>Reserved, shall be zero</summary>
        public ushort Generation
        {
            get { return _generation; }
            set { _generation = value; }
        }

        /// <summary>Index to the string heap</summary>
        public StringIndex Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// An index to the Guid heap, to distinguish between two versions of
        /// the same module
        /// </summary>
        public int Mvid
        {
            get { return _mvid; }
            set { _mvid = value; }
        }

        /// <summary>An index to the Guid heap, reserved shall be zero</summary>
        public int EncId
        {
            get { return _encId; }
            set { _encId = value; }
        }

        /// <summary>An index to the Guid heap, reserved shall be zero</summary>
        public int EncBaseId
        {
            get { return _encBaseId; }
            set { _encBaseId = value; }
        }
    }
}