namespace TheBoxSoftware.Reflection.Core.COFF
{
    public abstract class MetadataRow
    {
        internal int SizeOfRow { get; set; }

        public int FileOffset { get; set; }
    }
}