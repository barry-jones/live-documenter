
namespace TheBoxSoftware.Reflection.Core.COFF
{
    using System;

    /// <seealso cref="MethodSemanticsAttributes"/>
    public class MethodSemanticsMetadataTableRow : MetadataRow
    {
        private CodedIndex _association;
        private Index _method;
        private MethodSemanticsAttributes _semantics;

        /// <summary>
        /// Initialises a new instance of the MethodSemanticsMetadataTableRow class
        /// </summary>
        /// <param name="contents">The contents of the file</param>
        /// <param name="offset">The offset of the current row</param>
        public MethodSemanticsMetadataTableRow(byte[] contents, Offset offset, ICodedIndexResolver resolver, IIndexDetails indexDetails)
        {
            this.FileOffset = offset;

            int sizeOfCodedIndex = resolver.GetSizeOfIndex(CodedIndexes.HasSemantics);
            byte sizeOfMethodDefIndex = indexDetails.GetSizeOfIndex(MetadataTables.MethodDef);

            _semantics = (MethodSemanticsAttributes)BitConverter.ToUInt16(contents, offset.Shift(2));
            _method = new Index(contents, offset, sizeOfMethodDefIndex);
            _association = resolver.Resolve(CodedIndexes.HasSemantics,
                FieldReader.ToUInt32(contents, offset.Shift(sizeOfCodedIndex), sizeOfCodedIndex)
                );
        }

        /// <summary>
        /// A 2-byte bitmask of type <see cref="MethodSemanticsAttributes"/>.
        /// </summary>
        public MethodSemanticsAttributes Semantics
        {
            get { return _semantics; }
            set { _semantics = value; }
        }

        /// <summary>
        /// An index into the MethodDef table
        /// </summary>
        public Index Method
        {
            get { return _method; }
            set { _method = value; }
        }

        /// <summary>
        /// An index into the Event or Property table, a HasSemantics coded index
        /// </summary>
        public CodedIndex Association
        {
            get { return _association; }
            set { _association = value; }
        }
    }
}