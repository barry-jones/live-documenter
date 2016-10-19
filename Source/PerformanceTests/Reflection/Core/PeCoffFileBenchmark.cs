using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using TheBoxSoftware.Reflection.Core;

namespace PerformanceTests.Reflection.Core
{
    public class PeCoffFileBenchmark
    {
        private const string TestFile = @"theboxsoftware.reflection.dll";

        [Benchmark]
        public PeCoffFile Load()
        {
            PeCoffFile file = new PeCoffFile(TestFile);
            file.Initialise();
            return file;
        }
    }
}
