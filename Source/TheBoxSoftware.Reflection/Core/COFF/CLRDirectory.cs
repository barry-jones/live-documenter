
namespace TheBoxSoftware.Reflection.Core.COFF
{
    public class CLRDirectory : Directory
    {
        private Cor20Header _header;
        private MetadataDirectory _metadata;

        public CLRDirectory(byte[] fileContents, uint address)
            : base()
        {
            _header = new Cor20Header(fileContents, address);
        }

        public override void ReadDirectories(PeCoffFile containingFile)
        {
            _metadata = new MetadataDirectory(
                containingFile,
                containingFile.GetAddressFromRVA(this.Header.MetaData.VirtualAddress)
                );
        }

        public Cor20Header Header
        {
            get { return _header; }
            set { _header = value; }
        }

        public MetadataDirectory Metadata
        {
            get { return _metadata; }
            set { _metadata = value; }
        }
    }
}