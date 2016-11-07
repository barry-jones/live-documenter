
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
        /// <param name="contents">The contents of the file</param>
        /// <param name="offset">The offset of the this row</param>
        public DeclSecurityMetadataTableRow(byte[] contents, Offset offset, ICodedIndexResolver resolver, IIndexDetails indexDetails)
        {
            this.FileOffset = offset;

            int declSecurityIndexSize = resolver.GetSizeOfIndex(CodedIndexes.HasDeclSecurity);
            byte sizeOfBlobIndex = indexDetails.GetSizeOfBlobIndex();

            _action = BitConverter.ToUInt16(contents, offset.Shift(2));
            _parentIndex = resolver.Resolve(
                CodedIndexes.HasDeclSecurity,
                FieldReader.ToUInt32(contents, offset.Shift(declSecurityIndexSize), declSecurityIndexSize)
                );
            _permissionSet = FieldReader.ToUInt32(contents, offset.Shift(sizeOfBlobIndex), sizeOfBlobIndex);
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