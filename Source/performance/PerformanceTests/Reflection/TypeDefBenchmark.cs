
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
        private readonly TypeDef _noExtending;
        private readonly TypeDef _delegate;

        public TypeDefBenchmark()
        {
            _assembly = AssemblyDef.Create(TestFile);
            _genericType = _assembly.FindType("DocumentationTest", "GenericClass`3");
            _withFields = _assembly.FindType("DocumentationTest.BenchmarkClasses", "TypeDefWithFields");
            _noExtending = _assembly.FindType("DocumentationTest.BenchmarkClasses", "TypeDefNoExtendingTypes");
            _delegate = _assembly.FindType("DocumentationTest.BenchmarkClasses", "TypeDefDelegate");
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

        [Benchmark]
        public void GetExtendingTypes()
        {
            _withFields.GetExtendingTypes();
        }

        [Benchmark]
        public void GetExtendingTypesWhenThereAreNone()
        {
            _noExtending.GetExtendingTypes();
        }

        [Benchmark]
        public void GetConstructorsWithoutSystemGenerated()
        {
            _withFields.GetConstructors();
        }

        [Benchmark]
        public void GetConstructorsWithSystemGenerated()
        {
            _noExtending.GetConstructors();
        }

        [Benchmark]
        public void GetOperatorsWithoutSystemGenerated()
        {
            _withFields.GetOperators();
        }

        [Benchmark]
        public void GetOperatorsWithSystemGenerated()
        {
            _withFields.GetOperators();
        }

        [Benchmark]
        public void Namespace()
        {
            string container = _withFields.Namespace;
        }

        [Benchmark]
        public void InheritsFrom()
        {
            TypeRef inherit = _withFields.InheritsFrom;
        }

        [Benchmark]
        public void IsDelegateCheckWhenNot()
        {
            bool test = _withFields.IsDelegate;
        }

        [Benchmark]
        public void IsDelegateCheckWhenDelegate()
        {
            bool test = _delegate.IsDelegate;
        }
    }
}
