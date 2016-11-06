
namespace TheBoxSoftware.Reflection.Core.COFF
{
    using System;

    /// <summary>
    /// Represents the details of an Assembly stored in metadata.
    /// </summary>
    /// <remarks>
    /// Each PE/COFF file can only contain a reference to zero or one Assembly
    /// metadata row.
    /// </remarks>
    public class AssemblyMetadataTableRow : MetadataRow
    {
        private StringIndex _culture;
        private StringIndex _nameIndex;
        private uint _publicKey;
        private AssemblyFlags _flags;
        private ushort _revisionNumber;
        private ushort _buildNumber;
        private ushort _minorVersion;
        private ushort _majorVersion;
        private AssemblyHashAlgorithms _hashAlgId;

        /// <summary>
        /// Initialises a new instance of the AssemblyMetadataTableRow class
        /// </summary>
        /// <param name="contents">The contents of the file</param>
        /// <param name="offset">The offset of the current row</param>
        /// <param name="sizeOfBlobIndexes">Size in bytes of the indexes to the blob stream</param>
        /// <param name="sizeOfStringIndexes">Size in bytes of the indexes to the string stream</param>
        public AssemblyMetadataTableRow(byte[] contents, Offset offset, byte sizeOfBlobIndexes, byte sizeOfStringIndexes)
        {
            FileOffset = offset;

            _hashAlgId = (AssemblyHashAlgorithms)FieldReader.ToUInt32(contents, offset.Shift(4));
            _majorVersion = FieldReader.ToUInt16(contents, offset.Shift(2));
            _minorVersion = FieldReader.ToUInt16(contents, offset.Shift(2));
            _buildNumber = FieldReader.ToUInt16(contents, offset.Shift(2));
            _revisionNumber = FieldReader.ToUInt16(contents, offset.Shift(2));
            _flags = (AssemblyFlags)FieldReader.ToUInt32(contents, offset.Shift(4));
            _publicKey = FieldReader.ToUInt32(contents, offset.Shift(sizeOfBlobIndexes), sizeOfBlobIndexes);
            _nameIndex = new StringIndex(contents, sizeOfStringIndexes, offset);
            _culture = new StringIndex(contents, sizeOfStringIndexes, offset);
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

        /// <summary>
        /// 4-byte constant of AssemblyHashAlgorithm
        /// </summary>
        public AssemblyHashAlgorithms HashAlgId
        {
            get { return _hashAlgId; }
            set { _hashAlgId = value; }
        }

        /// <summary>
        /// Version details
        /// </summary>
        public ushort MajorVersion
        {
            get { return _majorVersion; }
            set { _majorVersion = value; }
        }

        /// <summary>
        /// Version details
        /// </summary>
        public ushort MinorVersion
        {
            get { return _minorVersion; }
            set { _minorVersion = value; }
        }

        /// <summary>
        /// Version details
        /// </summary>
        public ushort BuildNumber
        {
            get { return _buildNumber; }
            set { _buildNumber = value; }
        }

        /// <summary>
        /// Version details
        /// </summary>
        public ushort RevisionNumber
        {
            get { return _revisionNumber; }
            set { _revisionNumber = value; }
        }

        /// <summary>
        /// 4-byte bitmask of AssemblyFlags
        /// </summary>
        public AssemblyFlags Flags
        {
            get { return _flags; }
            set { _flags = value; }
        }

        /// <summary>
        /// An index in to the blob heap
        /// </summary>
        public uint PublicKey
        {
            get { return _publicKey; }
            set { _publicKey = value; }
        }

        /// <summary>
        /// An index in to the string heap
        /// </summary>
        public StringIndex Name
        {
            get { return _nameIndex; }
            set { _nameIndex = value; }
        }

        /// <summary>
        /// An index in to the string heap
        /// </summary>
        public StringIndex Culture
        {
            get { return _culture; }
            set { _culture = value; }
        }
    }
}