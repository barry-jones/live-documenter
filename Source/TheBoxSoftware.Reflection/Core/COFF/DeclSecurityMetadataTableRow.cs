
namespace TheBoxSoftware.Reflection.Core.COFF
{
    using System;

    /// <summary>
    /// Security attributes which derive from SecurityAttribute, can be attached to
    /// TypeDef, a Method or an Assembly
    /// </summary>
    public class DeclSecurityMetadataTableRow : MetadataRow
    {
        /// <summary>
        /// Initialises a new instance of the DeclSecurityMetadataTableRow
        /// </summary>
        /// <param name="stream">The stream containing the metadata</param>
        /// <param name="contents">The contents of the file</param>
        /// <param name="offset">The offset of the this row</param>
        public DeclSecurityMetadataTableRow(MetadataStream stream, byte[] contents, Offset offset)
        {
            this.FileOffset = offset;
            this.Action = BitConverter.ToUInt16(contents, offset.Shift(2));
            this.Parent = new CodedIndex(stream, offset, CodedIndexes.HasDeclSecurity);
            this.PermissionSet = FieldReader.ToUInt32(contents, offset.Shift(stream.SizeOfBlobIndexes), stream.SizeOfBlobIndexes);
        }

        /// <summary>
        /// A 2 byte value
        /// </summary>
        public UInt16 Action { get; set; }

        /// <summary>
        /// An index  into the MethodDef, TypeDef or Assembly tables. A HasDeclSecurity
        /// encoded index
        /// </summary>
        public CodedIndex Parent { get; set; }

        /// <summary>
        /// An index in to the blob heap
        /// </summary>
        public UInt32 PermissionSet { get; set; }
    }
}