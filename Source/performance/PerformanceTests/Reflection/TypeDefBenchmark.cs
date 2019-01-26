
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
        private readonly TypeDef _withFields;

        public TypeDefBenchmark()
        {
            _assembly = AssemblyDef.Create(TestFile);
            _genericType = _assembly.FindType("DocumentationTest", "GenericClass`3");
            _withFields = _assembly.FindType("DocumentationTest.BenchmarkClasses", "TypeDefWithFields");
        }

        [Benchmark]
        public void GetFieldsWithoutSystemGenerated()
        {
            _withFields.GetFields();
        }

        [Benchmark]
        public void GetFieldsWithSystemGenerated()
        {
            _withFields.GetFields(true);
        }

        [Benchmark]
        public void GetMethodsWithoutSystemGenerated()
        {
            _withFields.GetMethods();
        }

        [Benchmark]
        public void GetMethodsWithSystemGenerated()
        {
            _withFields.GetMethods(true);
        }
    }
}
