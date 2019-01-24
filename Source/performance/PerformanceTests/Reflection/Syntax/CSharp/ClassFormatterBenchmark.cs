
namespace PerformanceTests.Reflection.Syntax.CSharp
{
    using TheBoxSoftware.Reflection;
    using BenchmarkDotNet.Attributes;
    using TheBoxSoftware.Reflection.Syntax;

    public class ClassFormatterBenchmark
    {
        private const string TestFile = @"documentationtest.dll";
        private const string TypeNamespace = "SyntaxTests.ForClass";
        private const string TypeName = "DerivedClassWithInterface";

        private readonly IFormatter _cSharpFormatter;
        private readonly IFormatter _vbFormatter;

        public ClassFormatterBenchmark()
        {
            AssemblyDef _assemblyDef = AssemblyDef.Create(TestFile);
            TypeDef typeDef = _assemblyDef.FindType(TypeNamespace, TypeName);
            _cSharpFormatter = SyntaxFactory.Create(typeDef, Languages.CSharp);
            _vbFormatter = SyntaxFactory.Create(typeDef, Languages.VisualBasic);
        }

        [Benchmark]
        public void CSharpClassFormatter_Format()
        {
            _cSharpFormatter.Format();
        }

        [Benchmark]
        public void VBClassFormatter_Format()
        {
            _vbFormatter.Format();
        }
    }
}
