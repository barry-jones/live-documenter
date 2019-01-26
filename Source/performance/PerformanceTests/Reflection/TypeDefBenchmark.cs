
namespace PerformanceTests.Reflection
{
    using BenchmarkDotNet.Attributes;
    using TheBoxSoftware.Reflection;

    [MemoryDiagnoser]
    public class TypeDefBenchmark
    {
        public const string TestFile = "documentationtest.dll";
        private readonly AssemblyDef _assembly;
        private readonly TypeDef _genericType;

        public TypeDefBenchmark()
        {
            _assembly = AssemblyDef.Create(TestFile);
            _genericType = _assembly.FindType("DocumentationTest", "GenericClass`3");
        }
    }
}
