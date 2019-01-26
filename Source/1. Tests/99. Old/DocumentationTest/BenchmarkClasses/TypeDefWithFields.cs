
namespace DocumentationTest.BenchmarkClasses
{
    public class TypeDefWithFields
    {
        private string fieldOne;
        private TypeDefWithFields fieldTwo;
        public int Field3;
        public const string ConstantField = "AValue";

        public int IncludeSystemGeneratedBackingFields { get;set; }

        public int Property1 { get; set; }

        public int Property2 { get; set; }

        public int Property3 { get; set; }

        public int Property4 { get; set; }

        public int Property5 { get; set; }
    }
}
