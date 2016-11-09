
namespace TheBoxSoftware.Reflection.Tests.Unit.Comments
{
    using System;
    using NUnit.Framework;
    using TheBoxSoftware.Reflection.Comments;

    [TestFixture]
    public class CRefPathTests
    {
        [Test]
        public void DefaultCRefPath_ToString_ReturnsEmptyNamespace()
        {
            // it doesnt make sense to return an empty namespace string in these instances
            // I think it should return an empty string instead, but that is not a valid
            // crefpath - parhaps an error? Need to look in to this and change it as required.
            const string EXPECTED = "N:";

            CRefPath path = new CRefPath();
            
            Assert.AreEqual(EXPECTED, path.ToString());
        }

        [Test]
        public void WhenCRefIsErrorType_ToString_ShouldReturnEmptyString()
        {
            CRefPath path = CRefPath.Parse("invalid string");

            Assert.AreEqual(string.Empty, path.ToString());
        }

        [Test]
        public void WhenPassedEmptyString_Parse_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(delegate () {
                CRefPath.Parse(string.Empty);
            });
        }

        [Test]
        public void WhenPassedAnInvalidPath_Parse_TypeIsError()
        {
            CRefPath path = CRefPath.Parse("invalid string");

            Assert.AreEqual(CRefTypes.Error, path.PathType);
            Assert.AreEqual(string.Empty, path.ToString());
        }

        [Test]
        public void WhenMethodPath_Parse_TypeIsMethod()
        {
            // I think there is a slight problem here, in that there is an expection
            // that all method defenitions will all have atleast 3 parts, namespace,
            // type and then method name. This is an error as some types can be defined
            // without a namespace.

            CRefPath path = CRefPath.Parse("M:System.string.ToUpper()");

            Assert.AreEqual(CRefTypes.Method, path.PathType);
            Assert.AreEqual("string", path.TypeName);
            Assert.AreEqual("ToUpper", path.ElementName);
            Assert.AreEqual("System", path.Namespace);
        }

        [Test]
        public void WhenMethodPathWithOnlyTwoPartNames_Parse_TypeIsError()
        {
            // this fails because there is not enough elements defined, currently we expect
            // that there are more than 2 elements, which will probably cause errors when
            // types are defined without a namespace. 
            // TODO: Fix, but need to make sure all the rest of the system doesnt break.

            CRefPath path = CRefPath.Parse("M:string.ToUpper()");

            Assert.AreEqual(CRefTypes.Error, path.PathType); // this is currently an error as there
                // was no namespace defined
        }

        [Test]
        public void WhenMethodTypeWithNamespaceSection_Parse_SetsNamespace()
        {
            CRefPath path = CRefPath.Parse("M:System.Namespace.TypeName.MethodName()");

            Assert.AreEqual(CRefTypes.Method, path.PathType);
            Assert.AreEqual("MethodName", path.ElementName);
            Assert.AreEqual("System.Namespace", path.Namespace);
            Assert.AreEqual("TypeName", path.TypeName);
        }

        [Test]
        public void WhenMethodTypeWithParameters_Parse_SetsParameters()
        {
            CRefPath path = CRefPath.Parse("M:System.String.Format(string)");

            Assert.AreEqual(CRefTypes.Method, path.PathType);
            Assert.AreEqual("System", path.Namespace);
            Assert.AreEqual("String", path.TypeName);
            Assert.AreEqual("Format", path.ElementName);
            Assert.AreEqual("(string)", path.Parameters);
        }

        [Test]
        public void WhenEmptyNamespace_Parse_NamespaceIsEmptyString()
        {
            CRefPath path = CRefPath.Parse("N:");

            Assert.AreEqual(string.Empty, path.Namespace);
        }

        [Test]
        public void WhenNamespaceTypeWithName_Parse_SetsNamespace()
        {
            CRefPath path = CRefPath.Parse("N:System");

            Assert.AreEqual("System", path.Namespace);
        }

        [Test]
        public void WhenNamespaceTypeWithMultipleSections_Parse_SetsNamespace()
        {
            CRefPath path = CRefPath.Parse("N:System.Net.Http");

            Assert.AreEqual(CRefTypes.Namespace, path.PathType);
            Assert.AreEqual("System.Net.Http", path.Namespace);
        }

        [Test]
        public void WhenPassedDifferentPaths_Parse_SetsPathType()
        {
            PathTypesShouldBeSetCorrectly("F:System.Test.myfield", CRefTypes.Field);
            PathTypesShouldBeSetCorrectly("P:System.Test.Property", CRefTypes.Property);
            PathTypesShouldBeSetCorrectly("M:System.Test.Method()", CRefTypes.Method);
            PathTypesShouldBeSetCorrectly("T:System.Test", CRefTypes.Type);
            PathTypesShouldBeSetCorrectly("N:System", CRefTypes.Namespace);
            PathTypesShouldBeSetCorrectly("E:System.Test.Changed", CRefTypes.Event);
            PathTypesShouldBeSetCorrectly("!:Error", CRefTypes.Error);
        }

        [Test]
        public void WhenFieldProvided_Creates_FieldPath()
        {
            TypeDef container = new TypeDef();
            container.Name = "Container";
            container.Namespace = "Namespace";
            FieldDef field = new FieldDef();
            field.Type = container;
            field.Name = "afield";

            CRefPath result = new CRefPath(field);

            Assert.AreEqual(CRefTypes.Field, result.PathType);
            Assert.AreEqual("afield", result.ElementName);
            Assert.AreEqual("Namespace", result.Namespace);
            Assert.AreEqual("Container", result.TypeName);

            Assert.AreEqual("F:Namespace.Container.afield", result.ToString());
        }

        [Test]
        public void WhenTypeProvided_Creates_TypePath()
        {
            TypeDef type = new TypeDef();
            type.Name = "MyName";
            type.Namespace = "Namespace";

            CRefPath result = new CRefPath(type);

            Assert.AreEqual(CRefTypes.Type, result.PathType);
            Assert.AreEqual(string.Empty, result.ElementName);
            Assert.AreEqual("MyName", result.TypeName);
            Assert.AreEqual("Namespace", result.Namespace);

            Assert.AreEqual("T:Namespace.MyName", result.ToString());
        }

        public void PathTypesShouldBeSetCorrectly(string path, CRefTypes expectedType)
        {
            CRefPath converted = CRefPath.Parse(path);

            Assert.AreEqual(expectedType, converted.PathType);
        }
    }
}
