using System;

namespace TheBoxSoftware.Reflection.Core.COFF
{
    /// <remarks>Updated for 4-byte heap indexes</remarks>
    public class ModuleMetadataTableRow : MetadataRow
    {
        /// <summary>
        /// Initialises a new instance of the ModuleMetadataTableRow class
        /// </summary>
        /// <param name="stream">The metadata stream containing the details</param>
        /// <param name="contents">The byte contents of the file</param>
        /// <param name="offset">The offset of this row</param>
        public ModuleMetadataTableRow(MetadataStream stream, byte[] contents, Offset offset)
        {
            this.FileOffset = offset;
            this.Generation = FieldReader.ToUInt16(contents, offset.Shift(2));
            this.Name = new StringIndex(stream, offset);
            this.Mvid = FieldReader.ToInt32(contents, offset.Shift(stream.SizeOfGuidIndexes), stream.SizeOfGuidIndexes);
            this.EncId = FieldReader.ToInt32(contents, offset.Shift(stream.SizeOfGuidIndexes), stream.SizeOfGuidIndexes);
            this.EncBaseId = FieldReader.ToInt32(contents, offset.Shift(stream.SizeOfGuidIndexes), stream.SizeOfGuidIndexes);
        }

        /// <summary>Reserved, shall be zero</summary>
        public UInt16 Generation { get; set; }

        /// <summary>Index to the string heap</summary>
        public StringIndex Name { get; set; }

        /// <summary>
        /// An index to the Guid heap, to distinguish between two versions of
        /// the same module
        /// </summary>
        public Int32 Mvid { get; set; }

        /// <summary>An index to the Guid heap, reserved shall be zero</summary>
        public Int32 EncId { get; set; }

        /// <summary>An index to the Guid heap, reserved shall be zero</summary>
        public Int32 EncBaseId { get; set; }
    }
}