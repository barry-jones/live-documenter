using System.Collections.Generic;

namespace TheBoxSoftware.Reflection.Core.COFF
{
    public class MetadataDirectory : Directory
    {
        /// <summary>
        /// Initialises a new instance of the MetadataDirectory
        /// </summary>
        /// <param name="file">The contents of the file</param>
        /// <param name="address">The base address of the directory</param>
        public MetadataDirectory(PeCoffFile file, uint address)
        {
            this.Header = new MetadataHeader(file.FileContents, address);
            this.Streams = new Dictionary<Streams, Stream>();

            for(int i = 0; i < this.Header.NumberOfMetaDataStreams; i++)
            {
                Stream current = Stream.Create(
                    file,
                    Header.Headers[i].Offset + address,
                    this.Header.Headers[i]);

                // Calculate the nice enumerated value which describes the stream

                this.Streams.Add(current.StreamType, current);
            }
        }

        /// <summary>
        /// Helper method to obtain the stream of .NET metadata
        /// </summary>
        /// <returns>The .NET metadata stream</returns>
        internal COFF.MetadataStream GetMetadataStream()
        {
            return (COFF.MetadataStream)this.Streams[COFF.Streams.MetadataStream];
        }

        public MetadataHeader Header { get; set; }

        public Dictionary<Streams, Stream> Streams { get; set; }
    }
}