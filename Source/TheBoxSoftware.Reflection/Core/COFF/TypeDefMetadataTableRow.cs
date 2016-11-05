
namespace TheBoxSoftware.Reflection.Core.COFF
{
    /// <remarks>
    /// Updated for 4-byte heap indexes
    /// </remarks>
    public class TypeDefMetadataTableRow : MetadataRow
    {
        private Index _methodList;
        private Index _fieldList;
        private CodedIndex _extends;
        private StringIndex _namespaceIndex;
        private StringIndex _nameIndex;
        private TypeAttributes _flags;

        /// <summary>
        /// Initialises an instance of the TypeDefMetadataTableRow
        /// </summary>
        /// <param name="contents">The contents of the file</param>
        /// <param name="offset">The offset of the current row</param>
        public TypeDefMetadataTableRow(MetadataStream stream, byte[] contents, Offset offset)
        {
            this.FileOffset = offset;
            this.Flags = (TypeAttributes)FieldReader.ToUInt32(contents, offset.Shift(4));
            this.Name = new StringIndex(stream, offset);
            this.Namespace = new StringIndex(stream, offset);
            this.Extends = new CodedIndex(stream, offset, CodedIndexes.TypeDefOrRef);
            this.FieldList = new Index(stream, contents, offset, MetadataTables.Field);
            this.MethodList = new Index(stream, contents, offset, MetadataTables.MethodDef);
        }

        /// <summary>A 4-byte bitmask of TypeAttributes</summary>
        public TypeAttributes Flags
        {
            get { return _flags; }
            set { _flags = value; }
        }

        /// <summary>An index in to the string heap</summary>
        public StringIndex Name
        {
            get { return _nameIndex; }
            set { _nameIndex = value; }
        }

        /// <summary>An index in to the string heap</summary>
        public StringIndex Namespace
        {
            get { return _namespaceIndex; }
            set { _namespaceIndex = value; }
        }

        /// <summary>
        /// An index in to the TypeDef, TypeRef, or TypeSpec table, more precisely
        /// TypeDefOrRef coded index.
        /// </summary>
        public CodedIndex Extends
        {
            get { return _extends; }
            set { _extends = value; }
        }

        /// <summary>
        /// An index in to the Field table, marking the first of a continuous run
        /// of fields for the type. It continues until the smaller of, the last row
        /// in the table, the next run of fields.
        /// </summary>
        public Index FieldList
        {
            get { return _fieldList; }
            set { _fieldList = value; }
        }

        /// <summary>An index in to the MethodDef table, continuos list as above.</summary>
        public Index MethodList
        {
            get { return _methodList; }
            set { _methodList = value; }
        }
    }
}