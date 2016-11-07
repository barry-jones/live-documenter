
namespace TheBoxSoftware.Reflection.Core.COFF
{
    public class EventMapMetadataTableRow : MetadataRow
    {
        private Index _eventListIndex;
        private Index _parentIndex;

        public EventMapMetadataTableRow()
        {
        }

        /// <summary>
        /// Initialises a new EventMapMetadataTableRow
        /// </summary>
        /// <param name="contents">The contents of the file</param>
        /// <param name="offset">The offset of the current row</param>
        public EventMapMetadataTableRow(byte[] contents, Offset offset, IIndexDetails indexDetails)
        {
            this.FileOffset = offset;

            byte typeDefIndexSize = indexDetails.GetSizeOfIndex(MetadataTables.TypeDef);
            byte eventIndexSize = indexDetails.GetSizeOfIndex(MetadataTables.Event);

            _parentIndex = new Index(contents, offset, typeDefIndexSize);
            _eventListIndex = new Index(contents, offset, eventIndexSize);
        }

        /// <summary>
        /// An index into the TypeDef table
        /// </summary>
        public Index Parent
        {
            get { return _parentIndex; }
            set { _parentIndex = value; }
        }

        /// <summary>
        /// An index in to the Event table. Marking the first of a contiguos list
        /// of Events.
        /// </summary>
        public Index EventList
        {
            get { return _eventListIndex; }
            set { _eventListIndex = value; }
        }
    }
}