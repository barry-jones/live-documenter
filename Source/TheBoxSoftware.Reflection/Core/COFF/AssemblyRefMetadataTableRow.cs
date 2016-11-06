
namespace TheBoxSoftware.Reflection.Core.COFF
{
    using System;

    public class AssemblyRefMetadataTableRow : MetadataRow
    {
        private uint _hashValue;
        private StringIndex _cultureIndex;
        private StringIndex _nameIndex;
        private uint _publicKeyOrToken;
        private AssemblyFlags _flags;
        private ushort _revisionNumber;
        private ushort _buildNumber;
        private ushort _minorVersion;
        private ushort _majorVersion;

        public AssemblyRefMetadataTableRow() { }

        /// <summary>
        /// Initialises an instance of the AssemblyRefMetadataTableRow class
        /// </summary>
        /// <param name="contents">The contents of the file</param>
        /// <param name="offset">The offset of the current row</param>
        /// <param name="sizeOfBlobIndexes">Size in bytes of the indexes to the blob stream</param>
        /// <param name="sizeOfStringIndexes">Size in bytes of the indexes to the string stream</param>
        public AssemblyRefMetadataTableRow(byte[] contents, Offset offset, byte sizeOfBlobIndexes, byte sizeOfStringIndexes)
        {
            this.FileOffset = offset;

            _majorVersion = FieldReader.ToUInt16(contents, offset.Shift(2));
            _minorVersion = FieldReader.ToUInt16(contents, offset.Shift(2));
            _buildNumber = FieldReader.ToUInt16(contents, offset.Shift(2));
            _revisionNumber = FieldReader.ToUInt16(contents, offset.Shift(2));
            _flags = (AssemblyFlags)FieldReader.ToUInt32(contents, offset.Shift(4));
            _publicKeyOrToken = FieldReader.ToUInt32(contents, offset.Shift(sizeOfBlobIndexes), sizeOfBlobIndexes);
            _nameIndex = new StringIndex(contents, sizeOfStringIndexes, offset);
            _cultureIndex = new StringIndex(contents, sizeOfStringIndexes, offset);
            _hashValue = FieldReader.ToUInt32(contents, offset.Shift(sizeOfBlobIndexes), sizeOfBlobIndexes);
        }

        /// <summary>
        /// Returns a populated version class with the parsed version details for this
        /// assembly.
        /// </summary>
        /// <returns>The populated <see cref="Version"/> instance.</returns>
        public Core.Version GetVersion()
        {
            return new Core.Version(
                this.MajorVersion,
                this.MinorVersion,
                this.BuildNumber,
                this.RevisionNumber);
        }

        public ushort MajorVersion
        {
            get { return _majorVersion; }
            set { _majorVersion = value; }
        }

        public ushort MinorVersion
        {
            get { return _minorVersion; }
            set { _minorVersion = value; }
        }

        public ushort BuildNumber
        {
            get { return _buildNumber; }
            set { _buildNumber = value; }
        }

        public ushort RevisionNumber
        {
            get { return _revisionNumber; }
            set { _revisionNumber = value; }
        }

        public AssemblyFlags Flags
        {
            get { return _flags; }
            set { _flags = value; }
        }

        public uint PublicKeyOrToken
        {
            get { return _publicKeyOrToken; }
            set { _publicKeyOrToken = value; }
        }

        public StringIndex Name
        {
            get { return _nameIndex; }
            set { _nameIndex = value; }
        }

        public StringIndex Culture
        {
            get { return _cultureIndex; }
            set { _cultureIndex = value; }
        }

        public uint HashValue
        {
            get { return _hashValue; }
            set { _hashValue = value; }
        }
    }
}