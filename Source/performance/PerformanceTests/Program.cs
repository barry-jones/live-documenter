
namespace PerformanceTests
{
    using BenchmarkDotNet.Configs;
    using BenchmarkDotNet.Diagnosers;
    using BenchmarkDotNet.Running;

    class Program
    {
        static void Main(string[] args)
        {
            var config = ManualConfig.Create(DefaultConfig.Instance);

            //BenchmarkRunner.Run<Reflection.Core.PeCoffFileBenchmark>();
            //BenchmarkRunner.Run<Reflection.Core.COFF.StringStreamBenchmark>();
            //BenchmarkRunner.Run<Reflection.AssemblyDefBenchmark>();
            //BenchmarkRunner.Run<Reflection.Syntax.CSharp.ClassFormatterBenchmark>();
            //BenchmarkRunner.Run<Reflection.DisplayNameSignatureBenchmark>();
            //BenchmarkRunner.Run<Reflection.Signatures.SignatureBenchmark>();
            BenchmarkRunner.Run<Reflection.TypeDefBenchmark>();
        }
    }
}
