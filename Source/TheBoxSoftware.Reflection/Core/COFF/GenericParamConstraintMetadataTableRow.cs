namespace TheBoxSoftware.Reflection.Core.COFF
{
    public class GenericParamConstraintMetadataTableRow : MetadataRow
    {
        /// <summary>
        /// Initialises a new instance of the GenericParamConstraintMetadataTableRow class
        /// </summary>
        /// <param name="stream">The stream containing the metadata</param>
        /// <param name="contents">The contents of the file</param>
        /// <param name="offset">The offset of the current row</param>
        public GenericParamConstraintMetadataTableRow(MetadataStream stream, byte[] contents, Offset offset)
        {
            this.FileOffset = offset;
            this.Owner = new Index(stream, contents, offset, MetadataTables.GenericParam);
            this.Constraint = new CodedIndex(stream, offset, CodedIndexes.TypeDefOrRef);
        }

        /// <summary>
        /// An index into the GenericParam table
        /// </summary>
        public Index Owner { get; set; }

        /// <summary>
        /// An index in to the TypeDef, TypeRef, TypeSpec table or more precisely
        /// a TypeDefOrRef coded index
        /// </summary>
        public CodedIndex Constraint { get; set; }
    }
}