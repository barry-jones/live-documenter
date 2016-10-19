using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Running;

namespace PerformanceTests
{
    class Program
    {
        static void Main(string[] args)
        {
#if !DEBUG
            BenchmarkRunner.Run<Reflection.Core.PeCoffFileBenchmark>();
            BenchmarkRunner.Run<Reflection.Core.COFF.StringStreamBenchmark>();
#else
            new Reflection.Core.COFF.StringStreamBenchmark().GetStringFromStream();
#endif

        }
    }
}
