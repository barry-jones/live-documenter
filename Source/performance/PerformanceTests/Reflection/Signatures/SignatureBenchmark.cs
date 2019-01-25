

namespace PerformanceTests.Reflection.Signatures
{
    using BenchmarkDotNet.Attributes;
    using System.Collections.Generic;
    using TheBoxSoftware;
    using TheBoxSoftware.Reflection.Core;
    using TheBoxSoftware.Reflection.Core.COFF;
    using TheBoxSoftware.Reflection.Signatures;

    [MemoryDiagnoser]
    public class SignatureBenchmark
    {
        private const string TestFile = @"documentationtest.dll";
        private readonly PeCoffFile _file;
        private readonly MetadataDirectory _metadata;
        private readonly byte[] _source;

        public SignatureBenchmark()
        {
            _file = new PeCoffFile(TestFile, new FileSystem());
            _file.Initialise();

            _metadata = _file.GetMetadataDirectory();

            BlobStream blob = (BlobStream)_metadata.Streams[Streams.BlobStream];
            _source = blob.GetRange(0, (uint)blob.GetLength()); // copy full length of stream
        }

        [Benchmark(Description = "Create signature")]
        public void Create()
        {
            Signature.Create(_source, new Offset(OffsetValue), Signatures.MethodDef);
        }

        [ParamsSource(nameof(Values))]
        public int OffsetValue { get; set; }

        /// <summary>
        /// Source values as every method signature location in the files.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> Values()
        {
            MetadataRow[] rows = _metadata.GetMetadataStream().Tables[MetadataTables.MethodDef];
            return new List<int>()
            {
                // 3 methods picked at random
                (int)((MethodMetadataTableRow)rows[10]).Signiture.Value,
                (int)((MethodMetadataTableRow)rows[70]).Signiture.Value,
                (int)((MethodMetadataTableRow)rows[110]).Signiture.Value
            };
        }
    }
}
