
namespace TheBoxSoftware.Reflection.Tests.Unit
{
    using NUnit.Framework;
    using Reflection.Signitures;

    [TestFixture]
    public class DisplayNameSignitureConvertorTests
    {
        [Test]
        public void Convert_Type_WhenHasNoNameAndNamespace_ReturnsEmptyString()
        {
            TypeDef type = CreateTypeDef(string.Empty, string.Empty);
            var convertor = new DisplayNameSignitureConvertor(type, false);

            string result = convertor.Convert();

            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public void Convert_Type_WhenHasNameAndNoNamespace_ReturnsCorrectly()
        {
            TypeDef type = CreateTypeDef("MyName", string.Empty);

            var convertor = new DisplayNameSignitureConvertor(type, false);

            string result = convertor.Convert();

            Assert.AreEqual("MyName", result);
        }

        [Test]
        public void Convert_Type_WhenHasNameAndNoNamespaceButIncludeNamespaceSet_ReturnsCorrectly()
        {
            TypeDef type = CreateTypeDef("MyName", string.Empty);

            var convertor = new DisplayNameSignitureConvertor(type, true);

            string result = convertor.Convert();

            Assert.AreEqual("MyName", result);
        }

        [Test]
        public void Convert_Type_WhenHasNameAndNamespace_ReturnsCorrectly()
        {
            TypeDef type = CreateTypeDef("MyName", "Reflection");

            var convertor = new DisplayNameSignitureConvertor(type, true);

            string result = convertor.Convert();

            Assert.AreEqual("Reflection.MyName", result);
        }

        [Test]
        public void Convert_MethodDef_WhenNoNamespaceAndParametersSelected_ShouldOnlyReturnName()
        {
            MethodDef method = CreateMethod();

            var convertor = new DisplayNameSignitureConvertor(method, false, false);

            string result = convertor.Convert();

            Assert.AreEqual("MyMethod", result);
        }

        [Test]
        public void Convert_MethodDef_WhenNamespaceSelectedButNoParameters_ShouldReturnNameAndNamespace()
        {
            MethodDef method = CreateMethod();

            var convertor = new DisplayNameSignitureConvertor(method, true, false);

            string result = convertor.Convert();

            // include namespace does absolutely nothing when using methods
            Assert.AreEqual("MyMethod", result);
        }

        [Test]
        public void Convert_MethodDef_WhenNoParametersAndParametersIncluded_ShouldShowEmptyParaemterList()
        {
            MethodDef method = CreateMethod();

            var convertor = new DisplayNameSignitureConvertor(method, true, true);

            // we need a signiture for this to work which means we need to load the pecoffile
            string result = convertor.Convert();

            Assert.AreEqual("MyMethod()", result);
        }

        // [Test] - cant do this yet as we need signitures to be loaded, we first need to somehow
        //  mock out the signitures so we can control the return information.
        public void Convert_MethodDef_WhenParametersAndParametersIncluded_ShouldShowParameters()
        {
            MethodDef method = CreateMethod();

            ParamDef parameter = new ParamDef();
            parameter.Name = "test";
            method.Parameters.Add(parameter);
            parameter.Method = method;

            var convertor = new DisplayNameSignitureConvertor(method, true, true);

            string result = convertor.Convert();

            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public void Convert_PropertyDef_WhenPropertyHasNoParameters_ShouldReturnTheName()
        {
            PropertyDef property = new PropertyDef();
            property.Name = "MyProperty";
            MethodDef getter = new MethodDef();
            getter.Name = "get_MyProperty";
            property.Getter = getter;
            TypeDef container = new TypeDef();
            container.Properties.Add(property);
            property.OwningType = container;

            var convertor = new DisplayNameSignitureConvertor(property, false, false);

            string result = convertor.Convert();

            Assert.AreEqual("MyProperty", result);
        }

        private static MethodDef CreateMethod()
        {
            TypeDef type = CreateTypeDef("Container", "Reflection");
            type.Namespace = "Reflection";
            type.Name = "ContainingType";
            MethodDef method = new MethodDef();
            method.Name = "MyMethod";
            method.Type = type;
            return method;
        }

        private static TypeDef CreateTypeDef(string name, string namespaceName)
        {
            TypeDef type = new TypeDef();
            type.Name = name;
            type.Namespace = namespaceName;
            return type;
        }
    }
}
