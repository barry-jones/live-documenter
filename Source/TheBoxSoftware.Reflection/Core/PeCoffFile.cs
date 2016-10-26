using System.Collections.Generic;
using System.IO;
using TheBoxSoftware.Diagnostics;
using TheBoxSoftware.Reflection.Core.PE;

namespace TheBoxSoftware.Reflection.Core
{
    /// <summary>
    /// Provides access to the details of a .NET PE/COFF file, implementation is from
    /// the pecoff_v8 Microsoft document.
    /// </summary>
    /// <seealso cref="TheBoxSoftware.Reflection.AssemblyDef" />
    public sealed class PeCoffFile
    {
        private const int PeSignitureOffsetLocation = 0x3c;

        private byte[] _fileContents;
        
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
            COFF.CLRDirectory directory = Directories[DataDirectories.CommonLanguageRuntimeHeader] as COFF.CLRDirectory;
            return directory.Metadata;
        }

        public object ResolveCodedIndex(COFF.CodedIndex index)
        {
            object resolvedReference = null;

            COFF.MetadataDirectory metadata = GetMetadataDirectory();
            COFF.MetadataStream metadataStream = (COFF.MetadataStream)metadata.Streams[COFF.Streams.MetadataStream];
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
        internal int FileAddressFromRVA(int rva)
        {
            int virtualOffset = 0;
            int rawOffset = 0;
            int max = -1;
            int numSectionHeaders = this.SectionHeaders.Count;

            // determine which section the RVA belongs too
            for(int i = 0; i < numSectionHeaders; i++)
            {
                int minAddress = (int)this.SectionHeaders[i].VirtualAddress;
                int maxAddress = (i + 1 < numSectionHeaders)
                    ? (int)this.SectionHeaders[i + 1].VirtualAddress
                    : max;

                if(rva >= minAddress)
                {
                    if(maxAddress == -1 || rva < maxAddress)
                    {
                        virtualOffset = (int)this.SectionHeaders[i].VirtualAddress;
                        rawOffset = (int)this.SectionHeaders[i].PointerToRawData;
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
                    int address = this.FileAddressFromRVA((int)directory.VirtualAddress);

                    Directory created = Directory.Create(directory.Directory, _fileContents, address);
                    created.ReadDirectories(this);
                    this.Directories.Add(current.Key, created);
                }
            }
        }

        /// <summary>
        /// The full path and filename for the disk location of this PE/COFF file.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// The headers for all the sections defined in the file
        /// </summary>
        public List<SectionHeader> SectionHeaders { get; set; }

        /// <summary>
        /// All of the directories for the PE/COFF file.
        /// </summary>
        public Dictionary<DataDirectories, Directory> Directories { get; set; }

        /// <summary>
        /// Indicates if the metadata has been loaded in its entirety from the
        /// PE/COFF file.
        /// </summary>
        public bool IsMetadataLoaded { get; set; }

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
        internal MetadataToDefinitionMap Map { get; set; }
    }
}