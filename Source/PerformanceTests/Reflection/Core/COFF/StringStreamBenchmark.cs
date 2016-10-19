using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using TheBoxSoftware.Reflection.Core;
using TheBoxSoftware.Reflection.Core.COFF;

namespace PerformanceTests.Reflection.Core.COFF
{
    public class StringStreamBenchmark
    {
        private const string TestFile = @"theboxsoftware.reflection.dll";
        private StringStream _stream;

        public StringStreamBenchmark()
        {
            PeCoffFile file = new PeCoffFile(TestFile);
            file.Initialise();
            MetadataDirectory directory = file.GetMetadataDirectory();
            _stream = directory.Streams[Streams.StringStream] as StringStream;
        }

        [Benchmark]
        public string GetStringFromStream()
        {
            return _stream.GetString(23);
        }

        [Benchmark]
        public Dictionary<int, string> GetAllStringsFromStream()
        {
            return _stream.GetAllStrings();
        }
    }
}
