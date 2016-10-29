namespace TheBoxSoftware.Reflection.Core.COFF
{
    public class CLRDirectory : Directory
    {
        public CLRDirectory(byte[] fileContents, uint address)
            : base()
        {
            this.Header = new Cor20Header(fileContents, address);
        }

        public override void ReadDirectories(PeCoffFile containingFile)
        {
            base.ReadDirectories(containingFile);

            this.Metadata = new MetadataDirectory(containingFile,
                containingFile.FileAddressFromRVA(this.Header.MetaData.VirtualAddress)
                );
        }

        public Cor20Header Header { get; set; }

        public MetadataDirectory Metadata { get; set; }
    }
}