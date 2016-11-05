
namespace TheBoxSoftware.Reflection.Core.COFF
{
    using System.Collections.Generic;

    public class MetadataDirectory : Directory
    {
        private MetadataStream _metadata;
        private MetadataHeader _header;
        private Dictionary<Streams, Stream> _streams;

        /// <summary>
        /// Initialises a new instance of the MetadataDirectory
        /// </summary>
        /// <param name="file">The contents of the file</param>
        /// <param name="address">The base address of the directory</param>
        public MetadataDirectory(PeCoffFile file, uint address)
        {
            _header = new MetadataHeader(file.FileContents, address);
            _streams = new Dictionary<Streams, Stream>();

            for(int i = 0; i < _header.NumberOfMetaDataStreams; i++)
            {
                var streamHeader = _header.Headers[i];

                Stream current = Stream.Create(
                    file,
                    streamHeader.Offset + address,
                    (int)streamHeader.Size,
                    streamHeader.Name
                    );

                _streams.Add(current.StreamType, current);
            }
        }

        /// <summary>
        /// Helper method to obtain the stream of .NET metadata
        /// </summary>
        /// <returns>The .NET metadata stream</returns>
        internal COFF.MetadataStream GetMetadataStream()
        {
            if(_metadata == null)
            {
                _metadata = _streams[COFF.Streams.MetadataStream] as MetadataStream;
            }
            return _metadata;
        }

        public MetadataHeader Header
        {
            get { return _header; }
            set { _header = value; }
        }

        public Dictionary<Streams, Stream> Streams
        {
            get { return _streams; }
            set { _streams = value; }
        }
    }
}