
namespace TheBoxSoftware.Reflection.Core.COFF
{
    /// <summary>
    /// Used to store compile time, constant values for fields, parameters and properties
    /// </summary>
    /// <remarks>
    /// <para>
    /// Note that constant information does not directly influence runtime behaviour, although
    /// it is visible via reflection. Compilers inspect this information, at compile time, when
    /// importing metadata, but the value of the constant itself, if used, becomes embedded in
    /// into the CIL stream the compiler emits. There are no CIL instructions to access constant
    /// table at runtime.
    /// </para>
    /// </remarks>
    public class ConstantMetadataTableRow : MetadataRow
    {
        private BlobIndex _valueIndex;
        private CodedIndex _parentIndex;
        private byte _type;

        /// <summary>
        /// Initialises a new instance of the ConstantMetadataTableRow
        /// </summary>
        /// <param name="contents">The contents fo the file</param>
        /// <param name="offset">The offset for the current row</param>
        public ConstantMetadataTableRow(MetadataStream stream, byte[] contents, Offset offset)
        {
            this.FileOffset = offset;

            _type = contents[offset.Shift(1)];
            offset.Shift(1);
            _parentIndex = new CodedIndex(stream, offset, CodedIndexes.HasConstant);
            _valueIndex = new BlobIndex(stream.SizeOfBlobIndexes, contents, Signitures.Signitures.MethodDef, offset);
        }

        /// <summary>
        /// The type of field that represents the constant.
        /// </summary>
        /// <remarks>
        /// For a <b>nullref</b> value for <i>FieldInit</i> in <i>ilasm</i> is <c>ELEMENT_TYPE_CLASS</c>
        /// with a 4-byte zero. Unlike uses of <c>ELEMENT_TYPE_CLASS</c> in signitures, this one is
        /// <i>not</i> followed by a type token.
        /// </remarks>
        public byte Type
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// An index in to the Param, Field, or Property table. More precisely
        /// a HasConstant coded index
        /// </summary>
        public CodedIndex Parent
        {
            get { return _parentIndex; }
            set { _parentIndex = value; }
        }

        /// <summary>
        /// An index in to the Blob heap
        /// </summary>
        public BlobIndex Value
        {
            get { return _valueIndex; }
            set { _valueIndex = value; }
        }
    }
}