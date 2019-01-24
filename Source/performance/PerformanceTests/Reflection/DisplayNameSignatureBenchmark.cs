
namespace PerformanceTests.Reflection
{
    using BenchmarkDotNet.Attributes;
    using TheBoxSoftware.Reflection;
    using TheBoxSoftware.Reflection.Comments;
    using TheBoxSoftware.Reflection.Signatures;

    [MemoryDiagnoser]
    public class DisplayNameSignatureBenchmark
    {
        private const string TestFile = "documentationtest.dll";
        private const string TestMethod1 = "M:DocumentationTest.GenericClass`1.GenericReturnWellKnownType";
        private const string TestMethod2 = "M:DocumentationTest.GenericClass`1.GenericReturnDefinedInLibrary";

        private readonly AssemblyDef _assembly;
        private readonly MethodDef _method1;
        private readonly MethodDef _method2;

        public DisplayNameSignatureBenchmark()
        {
            _assembly = AssemblyDef.Create(TestFile);

            CRefPath path = CRefPath.Parse(TestMethod1);
            _method1 = _assembly.FindType(path.Namespace, path.TypeName).Methods.Find(m => m.Name == path.ElementName);

            path = CRefPath.Parse(TestMethod2);
            _method2 = _assembly.FindType(path.Namespace, path.TypeName).Methods.Find(m => m.Name == path.ElementName);
        }

        [Benchmark(Description = "DisplayName Generic return type 1")]
        public void DisplayNameOfGenericMethodReturnWellKnownType()
        {
            DisplayNameSignitureConvertor convertor = new DisplayNameSignitureConvertor(_method1, true, true);
            convertor.Convert();
        }

        [Benchmark(Description = "DisplayName Generic return type 2")]
        public void DisplayNameOfGenericMethodReturnInLibrary()
        {
            DisplayNameSignitureConvertor convertor = new DisplayNameSignitureConvertor(_method2, true, true);
            convertor.Convert();
        }
    }
}
