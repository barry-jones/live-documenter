namespace TheBoxSoftware.Reflection.Core.COFF
{
    public class Stream
    {
        private Streams _streamType;
        private string _name;

        public static Stream Create(PeCoffFile file, uint address, MetadataStreamHeader header)
        {
            Stream created = null;

            switch(header.Name)
            {
                case "#~":
                    created = new MetadataStream(file, address);
                    created.StreamType = Streams.MetadataStream;
                    break;

                case "#Strings":
                    created = new StringStream(file.FileContents, address, (int)header.Size);
                    created.StreamType = Streams.StringStream;
                    break;

                case "#GUID":
                    created = new GuidStream(file, address, (int)header.Size);
                    created.StreamType = Streams.GuidStream;
                    break;

                case "#Blob":
                    created = new BlobStream(file.FileContents, address, header.Size);
                    created.StreamType = Streams.BlobStream;
                    break;

                case "#US":
                    created = new Stream();
                    created.StreamType = Streams.USStream;
                    break;
            }

            created.Name = header.Name;
            return created;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public Streams StreamType
        {
            get { return _streamType; }
            set { _streamType = value; }
        }
    }
}