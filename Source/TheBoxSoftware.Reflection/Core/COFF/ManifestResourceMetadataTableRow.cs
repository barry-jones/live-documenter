
namespace TheBoxSoftware.Reflection.Core.COFF
{
    using System;

    public class ManifestResourceMetadataTableRow : MetadataRow
    {
        private CodedIndex _implementation;
        private StringIndex _name;
        private ManifestResourceAttributes _flags;
        private uint _offset;

        /// <summary>
        /// Initialises a new instance of the ManifestResourceMetadataTableRow class
        /// </summary>
        /// <param name="contents">The contents of the file</param>
        /// <param name="offset">The offset of this current row</param>
        public ManifestResourceMetadataTableRow(byte[] contents, Offset offset, ICodedIndexResolver resolver, byte sizeOfStringIndex)
        {
            this.FileOffset = offset;

            int sizeOfImplementationIndex = resolver.GetSizeOfIndex(CodedIndexes.Implementation);

            _offset = BitConverter.ToUInt32(contents, offset.Shift(4));
            _flags = (ManifestResourceAttributes)BitConverter.ToUInt32(contents, offset.Shift(4));
            _name = new StringIndex(contents, sizeOfStringIndex, offset);
            _implementation = resolver.Resolve(
                CodedIndexes.Implementation,
                FieldReader.ToUInt32(contents, offset.Shift(sizeOfImplementationIndex), sizeOfImplementationIndex)
                );
        }

        /// <summary>
        /// A 4-byte constant
        /// </summary>
        public uint Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        /// <summary>
        /// A 4-byte bitmask of ManifestResourceAttributes
        /// </summary>
        public ManifestResourceAttributes Flags
        {
            get { return _flags; }
            set { _flags = value; }
        }

        /// <summary>
        /// An index in to the string heap
        /// </summary>
        public StringIndex Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// An index in to a File, AssemblyRef, or null; more precisely an
        /// Implementation coded index
        /// </summary>
        public CodedIndex Implementation
        {
            get { return _implementation; }
            set { _implementation = value; }
        }
    }
}