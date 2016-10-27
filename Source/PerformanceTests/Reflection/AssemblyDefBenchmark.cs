
namespace PerformanceTests.Reflection
{
    using TheBoxSoftware.Reflection;
    using BenchmarkDotNet.Attributes;

    public class AssemblyDefBenchmark
    {
        private const string TestFile = @"theboxsoftware.reflection.dll";

        [Benchmark]
        public void LoadTest()
        {
            AssemblyDef def = AssemblyDef.Create(TestFile);
        }
    }
}
