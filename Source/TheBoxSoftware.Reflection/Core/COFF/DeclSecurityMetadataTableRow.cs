
namespace TheBoxSoftware.Reflection.Core.COFF
{
    using System;

    /// <summary>
    /// Security attributes which derive from SecurityAttribute, can be attached to
    /// TypeDef, a Method or an Assembly
    /// </summary>
    public class DeclSecurityMetadataTableRow : MetadataRow
    {
        private ushort _action;
        private CodedIndex _parentIndex;
        private uint _permissionSet;

        /// <summary>
        /// Initialises a new instance of the DeclSecurityMetadataTableRow
        /// </summary>
        /// <param name="stream">The stream containing the metadata</param>
        /// <param name="contents">The contents of the file</param>
        /// <param name="offset">The offset of the this row</param>
        public DeclSecurityMetadataTableRow(MetadataStream stream, byte[] contents, Offset offset)
        {
            this.FileOffset = offset;

            _action = BitConverter.ToUInt16(contents, offset.Shift(2));
            _parentIndex = new CodedIndex(stream, offset, CodedIndexes.HasDeclSecurity);
            _permissionSet = FieldReader.ToUInt32(contents, offset.Shift(stream.SizeOfBlobIndexes), stream.SizeOfBlobIndexes);
        }

        /// <summary>
        /// A 2 byte value
        /// </summary>
        public ushort Action
        {
            get { return _action; }
            set { _action = value; }
        }

        /// <summary>
        /// An index  into the MethodDef, TypeDef or Assembly tables. A HasDeclSecurity
        /// encoded index
        /// </summary>
        public CodedIndex Parent
        {
            get { return _parentIndex; }
            set { _parentIndex = value; }
        }

        /// <summary>
        /// An index in to the blob heap
        /// </summary>
        public uint PermissionSet
        {
            get { return _permissionSet; }
            set { _permissionSet = value; }
        }
    }
}