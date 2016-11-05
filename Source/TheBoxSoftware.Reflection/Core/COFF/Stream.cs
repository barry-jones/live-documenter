
namespace TheBoxSoftware.Reflection.Core.COFF
{
    public class Stream
    {
        private Streams _streamType;
        private string _name;

        public static Stream Create(PeCoffFile file, uint address, int size, string name)
        {
            Stream created = null;

            switch(name)
            {
                case "#~":
                    created = new MetadataStream(file, address);
                    created.StreamType = Streams.MetadataStream;
                    break;

                case "#Strings":
                    created = new StringStream(file.FileContents, address, size);
                    created.StreamType = Streams.StringStream;
                    break;

                case "#GUID":
                    created = new GuidStream(file, address, size);
                    created.StreamType = Streams.GuidStream;
                    break;

                case "#Blob":
                    created = new BlobStream(file.FileContents, address, size);
                    created.StreamType = Streams.BlobStream;
                    break;

                case "#US":
                    created = new Stream();
                    created.StreamType = Streams.USStream;
                    break;
            }

            created.Name = name;

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