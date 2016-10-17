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

        private string filePath;
        private string fileName;
        private FileHeader fileHeader;
        private PEHeader peHeader;
        private List<SectionHeader> sectionHeaders;
        private Dictionary<DataDirectories, Directory> directories;
        private byte[] fileContents;
        private bool isMetadataLoaded;
        private MetadataToDefinitionMap map;

        /// <summary>
        /// Initialises a new instance of the PeCoffFile
        /// </summary>
        /// <param name="filePath">The physical location of the file</param>
        public PeCoffFile(string filePath)
        {
            TraceHelper.WriteLine("loading pecoff file ({0})", filePath);
            TraceHelper.Indent();
            this.FileName = filePath;
            this.Map = new MetadataToDefinitionMap();
            this.IsMetadataLoaded = false;
            this.filePath = filePath;
            this.ReadFileContents();
            this.FileContents = null;
            this.IsMetadataLoaded = true;
            TraceHelper.Unindent();
        }

        /// <summary>
        /// Helper method to obtain the .NET metadata directory
        /// </summary>
        /// <returns>The .NET metadata directory</returns>
        public COFF.MetadataDirectory GetMetadataDirectory()
        {
            return ((COFF.CLRDirectory)this.Directories[
                DataDirectories.CommonLanguageRuntimeHeader
                ]).Metadata;
        }

        /// <summary>
        /// Reads the contents of the PeCoff file in to our custom data structures
        /// </summary>
        /// <exception Cref="NotAManagedLibraryException">
        /// Thrown when a file which is not a managed PE file is loaded.
        /// </exception>
        private void ReadFileContents()
        {
            List<byte> contents = new List<byte>(File.ReadAllBytes(filePath));
            byte[] contentsAsArray = contents.ToArray();
            this.FileContents = contentsAsArray;

            // Load the offset for the PE file
            Offset offset = contents[PeCoffFile.PeSignitureOffsetLocation];

            // Read and check the signiture of the file to verify it
            //string signiture = Convert.ToString(contents.GetRange(offset, offset.Shift(4)).ToArray());
            //if (signiture != "PE") {
            //    throw new ApplicationException("Invalid signiture in PE/COFF file");
            //}
            offset += 4;

            // Read the image_file_header
            this.FileHeader = new FileHeader(contentsAsArray, offset);
            this.PeHeader = new PEHeader(contentsAsArray, offset);

            this.ReadSectionHeaders(offset);
            this.ReadDirectories();
        }

        /// <summary>
        /// Reads the headers for all of the defined sections in the file
        /// </summary>
        /// <param name="offset">The offset to the section headers</param>
        private void ReadSectionHeaders(Offset offset)
        {
            this.SectionHeaders = new List<SectionHeader>();
            for(int i = 0; i < this.FileHeader.NumberOfSections; i++)
            {
                this.SectionHeaders.Add(new SectionHeader(this.FileContents, offset));
            }
        }

        /// <summary>
        /// Reads the contents of the directories specified in the file header
        /// </summary>
        private void ReadDirectories()
        {
            this.Directories = new Dictionary<DataDirectories, Directory>();
            foreach(KeyValuePair<DataDirectories, DataDirectory> current in this.PeHeader.DataDirectories)
            {
                DataDirectory directory = current.Value;

                if(directory.IsUsed)
                {
                    int address = this.FileAddressFromRVA((int)directory.VirtualAddress);
                    Directory created = Directory.Create(directory.Directory, this.FileContents, address);
                    created.ReadDirectories(this);
                    this.Directories.Add(current.Key, created);
                }
            }
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

            // determine which section the RVA belongs too
            for(int i = 0; i < this.SectionHeaders.Count; i++)
            {
                int minAddress = (int)this.SectionHeaders[i].VirtualAddress;
                int maxAddress = (i + 1 < this.SectionHeaders.Count)
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
        /// The full path and filename for the disk location of this PE/COFF file.
        /// </summary>
        public string FileName
        {
            get { return this.fileName; }
            private set { this.fileName = value; }
        }

        /// <summary>
        /// This files header details.
        /// </summary>
        public FileHeader FileHeader
        {
            get { return this.fileHeader; }
            private set { this.fileHeader = value; }
        }

        /// <summary>
        /// The PE Header.
        /// </summary>
        public PEHeader PeHeader
        {
            get { return this.peHeader; }
            private set { this.peHeader = value; }
        }

        /// <summary>
        /// The headers for all the sections defined in the file
        /// </summary>
        public List<SectionHeader> SectionHeaders
        {
            get { return this.sectionHeaders; }
            private set { this.sectionHeaders = value; }
        }

        /// <summary>
        /// All of the directories for the PE/COFF file.
        /// </summary>
        public Dictionary<DataDirectories, Directory> Directories
        {
            get { return this.directories; }
            private set { this.directories = value; }
        }

        /// <summary>
        /// The byte contents of the file.
        /// </summary>
        internal byte[] FileContents
        {
            get { return this.fileContents; }
            private set { this.fileContents = value; }
        }

        /// <summary>
        /// Indicates if the metadata has been loaded in its entirety from the
        /// PE/COFF file.
        /// </summary>
        public bool IsMetadataLoaded
        {
            get { return this.isMetadataLoaded; }
            private set { this.isMetadataLoaded = value; }
        }

        /// <summary>
        /// Internal mapping of metadata to reflected definitions.
        /// </summary>
        internal MetadataToDefinitionMap Map
        {
            get { return this.map; }
            private set { this.map = value; }
        }
    }
}