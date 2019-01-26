
namespace TheBoxSoftware.Reflection.Core
{
    using System;
    using System.Collections.Generic;
    using PE;
    using TheBoxSoftware.Reflection.Core.COFF;

    /// <summary>
    /// Provides access to the details of a .NET PE/COFF file, implementation is from
    /// the pecoff_v8 Microsoft document.
    /// </summary>
    /// <seealso cref="TheBoxSoftware.Reflection.AssemblyDef" />
    public sealed class PeCoffFile
    {
        private const int PeSignitureOffsetLocation = 0x3c;

        private readonly IFileSystem _fileSystem;

        private byte[] _fileContents;
        private COFF.MetadataDirectory _metadataDirectory;
        private MetadataToDefinitionMap _map;
        private bool _isMetadataLoaded;
        private Dictionary<DataDirectories, Directory> _directories;
        private List<SectionHeader> _sectionHeaders;
        private string _fileName;

        /// <summary>
        /// Initialises a new instance of the PeCoffFile
        /// </summary>
        /// <param name="filePath">The physical location of the file</param>
        /// <param name="fileSystem">Object which can be used to access data from the file system</param>
        public PeCoffFile(string filePath, IFileSystem fileSystem)
        {
            if(string.IsNullOrEmpty(filePath))
                throw new ArgumentException(nameof(filePath));

            _fileName = filePath;
            _fileSystem = fileSystem;
        }

        public void Initialise()
        {
            _map = new MetadataToDefinitionMap();
            _isMetadataLoaded = false;
            ReadFileContents();
            _fileContents = null;
            _isMetadataLoaded = true;
        }

        /// <summary>
        /// Helper method to obtain the .NET metadata directory
        /// </summary>
        /// <returns>The .NET metadata directory</returns>
        public COFF.MetadataDirectory GetMetadataDirectory()
        {
            if(_metadataDirectory == null)
            {
                COFF.CLRDirectory directory = Directories[DataDirectories.CommonLanguageRuntimeHeader] as COFF.CLRDirectory;
                _metadataDirectory = directory.Metadata;
            }
            return _metadataDirectory;
        }

        public bool CanGetAddressFromRva(uint rva)
        {
            return FindHeaderForRva(rva) != null;
        }

        /// <summary>
        /// Resolves a metadata token to a <see cref="ReflectedMember"/> defined in this
        /// PeCoffFile.
        /// </summary>
        /// <param name="metadataToken">The token to resolve.</param>
        /// <returns>The found member or null if not found.</returns>
        public ReflectedMember ResolveMetadataToken(uint metadataToken)
        {
            MetadataStream metadataStream = GetMetadataDirectory().GetMetadataStream();

            // Get the details in the token
            ILMetadataToken token = (ILMetadataToken)(metadataToken & 0xff000000);
            uint index = metadataToken & 0x00ffffff;

            ReflectedMember returnItem = null;

            switch (token)
            {
                // Method related tokens
                case ILMetadataToken.MethodDef:
                    returnItem = _map.GetDefinition(MetadataTables.MethodDef, metadataStream.GetEntryFor(MetadataTables.MethodDef, index));
                    break;
                case ILMetadataToken.MemberRef:
                    returnItem = _map.GetDefinition(MetadataTables.MemberRef, metadataStream.GetEntryFor(MetadataTables.MemberRef, index));
                    break;
                case ILMetadataToken.MethodSpec:
                    returnItem = _map.GetDefinition(MetadataTables.MethodSpec, metadataStream.GetEntryFor(MetadataTables.MethodSpec, index));
                    break;
                // Type related tokens
                case ILMetadataToken.TypeDef:
                    returnItem = _map.GetDefinition(MetadataTables.TypeDef, metadataStream.GetEntryFor(MetadataTables.TypeDef, index));
                    break;
                case ILMetadataToken.TypeRef:
                    returnItem = _map.GetDefinition(MetadataTables.TypeRef, metadataStream.GetEntryFor(MetadataTables.TypeRef, index));
                    break;
                case ILMetadataToken.TypeSpec:
                    returnItem = _map.GetDefinition(MetadataTables.TypeSpec, metadataStream.GetEntryFor(MetadataTables.TypeSpec, index));
                    break;
                case ILMetadataToken.FieldDef:
                    returnItem = _map.GetDefinition(MetadataTables.Field, metadataStream.GetEntryFor(MetadataTables.Field, index));
                    break;
            }

            return returnItem;
        }

        /// <summary>
        /// Converts a Relative Virtual Address to a file offset
        /// </summary>
        /// <param name="rva">The RVA to convert</param>
        /// <returns>The file offset address</returns>
        internal uint GetAddressFromRVA(uint rva)
        {
            SectionHeader found = FindHeaderForRva(rva);
            if (found == null)
            {
                throw new InvalidOperationException();
            }

            uint virtualOffset = found.VirtualAddress;
            uint fileLocation = found.PointerToRawData;

            return fileLocation + (rva - virtualOffset);
        }

        private SectionHeader FindHeaderForRva(uint rva)
        {
            SectionHeader found = null;
            int numSectionHeaders = _sectionHeaders.Count;

            // determine which section the RVA belongs too
            for(int i = 0; i < numSectionHeaders; i++)
            {
                SectionHeader header = _sectionHeaders[i];

                // p277 or ECMA 335
                // our RVA r, header RVA s, header size l, header pointer p
                // s <= r < s + l then p + (r - s)

                uint minAddress = header.VirtualAddress;
                uint maxAddress = header.VirtualAddress + header.SizeOfRawData;

                if(minAddress <= rva && rva < maxAddress)
                {
                    found = header;
                }
            }

            return found;
        }

        /// <summary>
        /// Reads the contents of the PeCoff file in to our custom data structures
        /// </summary>
        /// <exception Cref="NotAManagedLibraryException">
        /// Thrown when a file which is not a managed PE file is loaded.
        /// </exception>
        private void ReadFileContents()
        {
            _fileContents = _fileSystem.ReadAllBytes(_fileName);

            Offset offset = _fileContents[PeCoffFile.PeSignitureOffsetLocation];
            offset += 4; // skip past the PE signature bytes

            FileHeader fileHeader = new FileHeader(_fileContents, offset);
            PEHeader peHeader = new PEHeader(_fileContents, offset);

            ReadSectionHeaders(fileHeader.NumberOfSections, offset);
            ReadDirectories(peHeader.DataDirectories);
        }

        /// <summary>
        /// Reads the headers for all of the defined sections in the file
        /// </summary>
        /// <param name="numberOfSections">The number of sections in the file header to initialise.</param>
        /// <param name="offset">The offset to the section headers</param>
        private void ReadSectionHeaders(ushort numberOfSections, Offset offset)
        {
            _sectionHeaders = new List<SectionHeader>();

            for(int i = 0; i < numberOfSections; i++)
            {
                _sectionHeaders.Add(new SectionHeader(_fileContents, offset));
            }
        }

        /// <summary>
        /// Reads the contents of the directories specified in the file header
        /// </summary>
        /// <param name="dataDirectories">The data directories to initialise in hte PE header.</param>
        private void ReadDirectories(Dictionary<DataDirectories, DataDirectory> dataDirectories)
        {
            _directories = new Dictionary<DataDirectories, Directory>();

            foreach(KeyValuePair<DataDirectories, DataDirectory> current in dataDirectories)
            {
                DataDirectory directory = current.Value;

                if(!directory.IsUsed) continue;

                if(!CanGetAddressFromRva(directory.VirtualAddress))
                {
                    if(directory.Directory == DataDirectories.CommonLanguageRuntimeHeader)
                    {
                        throw new ClrDirectoryNotFoundException(_fileName);
                    }
                }
                else
                {
                    uint address = GetAddressFromRVA(directory.VirtualAddress);

                    Directory created = Directory.Create(directory.Directory, _fileContents, address);
                    created.ReadDirectories(this);
                    _directories.Add(current.Key, created);
                }
            }
        }

        /// <summary>
        /// The full path and filename for the disk location of this PE/COFF file.
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        /// <summary>
        /// The headers for all the sections defined in the file
        /// </summary>
        public List<SectionHeader> SectionHeaders
        {
            get { return _sectionHeaders; }
            set { _sectionHeaders = value; }
        }

        /// <summary>
        /// All of the directories for the PE/COFF file.
        /// </summary>
        public Dictionary<DataDirectories, Directory> Directories
        {
            get { return _directories; }
            set { _directories = value; }
        }

        /// <summary>
        /// Indicates if the metadata has been loaded in its entirety from the
        /// PE/COFF file.
        /// </summary>
        public bool IsMetadataLoaded
        {
            get { return _isMetadataLoaded; }
            set { _isMetadataLoaded = value; }
        }

        /// <summary>
        /// The byte contents of the file.
        /// </summary>
        internal byte[] FileContents
        {
            get { return _fileContents; }
        }

        /// <summary>
        /// Internal mapping of metadata to reflected definitions.
        /// </summary>
        internal MetadataToDefinitionMap Map
        {
            get { return _map; }
            set { _map = value; }
        }
    }
}