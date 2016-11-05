
namespace TheBoxSoftware.Reflection.Core
{
    using System.Collections.Generic;
    using System.IO;
    using PE;

    /// <summary>
    /// Provides access to the details of a .NET PE/COFF file, implementation is from
    /// the pecoff_v8 Microsoft document.
    /// </summary>
    /// <seealso cref="TheBoxSoftware.Reflection.AssemblyDef" />
    public sealed class PeCoffFile
    {
        private const int PeSignitureOffsetLocation = 0x3c;

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
        public PeCoffFile(string filePath)
        {
            FileName = filePath;
        }

        public void Initialise()
        {
            Map = new MetadataToDefinitionMap();
            IsMetadataLoaded = false;
            ReadFileContents();
            _fileContents = null;
            IsMetadataLoaded = true;
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

        /// <summary>
        /// Resolves a coded index to it's instantiated reference.
        /// </summary>
        /// <param name="index">The coded index to resolve</param>
        /// <returns>The object referenced by the coded index or null if not found.</returns>
        public object ResolveCodedIndex(COFF.CodedIndex index)
        {
            object resolvedReference = null;

            COFF.MetadataDirectory metadata = GetMetadataDirectory();
            COFF.MetadataStream metadataStream = metadata.GetMetadataStream();

            if(metadataStream.Tables.ContainsKey(index.Table))
            {
                if(metadataStream.Tables[index.Table].Length + 1 > index.Index)
                {
                    COFF.MetadataRow metadataRow = metadataStream.GetEntryFor(index);
                    resolvedReference = Map.GetDefinition(index.Table, metadataRow);
                }
            }

            return resolvedReference;
        }

        /// <summary>
        /// Converts a Relative Virtual Address to a file offset
        /// </summary>
        /// <param name="rva">The RVA to convert</param>
        /// <returns>The file offset address</returns>
        internal uint FileAddressFromRVA(uint rva)
        {
            uint virtualOffset = 0;
            uint rawOffset = 0;
            uint max = 0;
            int numSectionHeaders = this.SectionHeaders.Count;

            // determine which section the RVA belongs too
            for(int i = 0; i < numSectionHeaders; i++)
            {
                uint minAddress = this.SectionHeaders[i].VirtualAddress;
                uint maxAddress = (i + 1 < numSectionHeaders)
                    ? this.SectionHeaders[i + 1].VirtualAddress
                    : max;

                if(rva >= minAddress)
                {
                    if(rva < maxAddress)
                    {
                        virtualOffset = SectionHeaders[i].VirtualAddress;
                        rawOffset = SectionHeaders[i].PointerToRawData;
                    }
                }
            }

            return rva - virtualOffset + rawOffset;
        }

        /// <summary>
        /// Reads the contents of the PeCoff file in to our custom data structures
        /// </summary>
        /// <exception Cref="NotAManagedLibraryException">
        /// Thrown when a file which is not a managed PE file is loaded.
        /// </exception>
        private void ReadFileContents()
        {
            _fileContents = File.ReadAllBytes(FileName);

            Offset offset = _fileContents[PeCoffFile.PeSignitureOffsetLocation];
            offset += 4; // skip past the PE signature bytes

            FileHeader fileHeader = new FileHeader(_fileContents, offset);
            PEHeader peHeader = new PEHeader(_fileContents, offset);

            this.ReadSectionHeaders(fileHeader.NumberOfSections, offset);
            this.ReadDirectories(peHeader.DataDirectories);
        }

        /// <summary>
        /// Reads the headers for all of the defined sections in the file
        /// </summary>
        /// <param name="numberOfSections">The number of sections in the file header to initialise.</param>
        /// <param name="offset">The offset to the section headers</param>
        private void ReadSectionHeaders(ushort numberOfSections, Offset offset)
        {
            this.SectionHeaders = new List<SectionHeader>();

            for(int i = 0; i < numberOfSections; i++)
            {
                this.SectionHeaders.Add(new SectionHeader(_fileContents, offset));
            }
        }

        /// <summary>
        /// Reads the contents of the directories specified in the file header
        /// </summary>
        /// <param name="dataDirectories">The data directories to initialise in hte PE header.</param>
        private void ReadDirectories(Dictionary<DataDirectories, DataDirectory> dataDirectories)
        {
            this.Directories = new Dictionary<DataDirectories, Directory>();

            foreach(KeyValuePair<DataDirectories, DataDirectory> current in dataDirectories)
            {
                DataDirectory directory = current.Value;

                if(directory.IsUsed)
                {
                    uint address = FileAddressFromRVA(directory.VirtualAddress);

                    Directory created = Directory.Create(directory.Directory, _fileContents, address);
                    created.ReadDirectories(this);
                    this.Directories.Add(current.Key, created);
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