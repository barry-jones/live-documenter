
using System;

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

        public void Method1()
        {
            // should create a compiler generated methods
            Func<int, int, int> add = (int a, int b) => a + b;
        }

        public string Method2() => "return info";

        public string Generic<T>() => "return";

        public void AnotherMethod() { }

        public class InheritedOne : TypeDefWithFields { }

        public class InheritedTwo : TypeDefWithFields { }

        public class InhertiedThree : TypeDefWithFields { }

        public class InheritedFour : TypeDefWithFields { }

        public class SecondLevelInherited : TypeDefWithFields.InheritedFour { }

        public TypeDefWithFields() { }

        public TypeDefWithFields(string first) { }

        public TypeDefWithFields(int another) { }
    }
}
