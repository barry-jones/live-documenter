using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TheBoxSoftware.Reflection.Comments;

namespace TheBoxSoftware.Reflections.Tests.Comments.Unit
{
    [TestFixture]
    public class CRefPathTests
    {
        [Test]
        public void CRefPath_WhenEmptyCreated_ShouldReturnNPath()
        {
            // it doesnt make sense to return an empty namespace string in these instances
            // I think it should return an empty string instead, but that is not a valid
            // crefpath - parhaps an error? Need to look in to this and change it as required.
            const string EXPECTED = "N:";

            CRefPath path = new CRefPath();
            
            Assert.AreEqual(EXPECTED, path.ToString());
        }

        [Test]
        public void CRefPath_Parse_WhenPathIsEmpty_ShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(delegate () {
                CRefPath.Parse(string.Empty);
            });
        }

        [Test]
        public void CRefPath_Parse_WhenPathIsInvalid_ShouldReturnEmptyString()
        {
            CRefPath path = CRefPath.Parse("invalid string");

            Assert.AreEqual(CRefTypes.Error, path.PathType);
            Assert.AreEqual(string.Empty, path.ToString());
        }

        [Test]
        public void CRefPath_Parse_ErrorPath_Should()
        {
            CRefPath path = CRefPath.Parse("E:information");

            Assert.AreEqual(CRefTypes.Error, path.PathType);
            Assert.AreEqual(string.Empty, path.ToString());
        }

        [Test]
        public void CRefPath_Parse_MethodPath_ShouldReturnMethodType()
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
        public void CRefPath_Parse_MethodPathWithOnlyTwoPartNames_ShouldReturnMethodType()
        {
            CRefPath path = CRefPath.Parse("M:string.ToUpper()");

            Assert.AreEqual(CRefTypes.Error, path.PathType); // this is currently an error as there
                // was no namespace defined
        }

        [Test]
        public void CRefPath_Parse_MethodTypeWithNamespaceSection_ShouldPopulateNamespace()
        {
            CRefPath path = CRefPath.Parse("M:System.Namespace.TypeName.MethodName()");

            Assert.AreEqual(CRefTypes.Method, path.PathType);
            Assert.AreEqual("MethodName", path.ElementName);
            Assert.AreEqual("System.Namespace", path.Namespace);
            Assert.AreEqual("TypeName", path.TypeName);
        }

        [Test]
        public void CRefPath_Parse_MethodTypeWithParameters_ShouldPopulateParameters()
        {
            CRefPath path = CRefPath.Parse("M:System.String.Format(string)");

            Assert.AreEqual(CRefTypes.Method, path.PathType);
            Assert.AreEqual("System", path.Namespace);
            Assert.AreEqual("String", path.TypeName);
            Assert.AreEqual("Format", path.ElementName);
            Assert.AreEqual("(string)", path.Parameters);
        }

        [Test]
        public void CRefPath_Parse_NamespaceType_ShouldReturnNamespace()
        {
            CRefPath path = CRefPath.Parse("N:");

            Assert.AreEqual(CRefTypes.Namespace, path.PathType);
            Assert.AreEqual(string.Empty, path.Namespace);
        }

        [Test]
        public void CRefPath_Parse_NamespaceTypeWithName_ShouldSetNamespaceName()
        {
            CRefPath path = CRefPath.Parse("N:System");

            Assert.AreEqual(CRefTypes.Namespace, path.PathType);
            Assert.AreEqual("System", path.Namespace);
        }

        [Test]
        public void CRefPath_Parse_NamespaceTypeWithMultipleSections_ShouldSetNamesapceName()
        {
            CRefPath path = CRefPath.Parse("N:System.Net.Http");

            Assert.AreEqual(CRefTypes.Namespace, path.PathType);
            Assert.AreEqual("System.Net.Http", path.Namespace);
        }

        [Test]
        public void CRefPath_Parse_TypePath_ShouldPopulate_TypeName()
        {
            CRefPath path = CRefPath.Parse("T:System.String");

            Assert.AreEqual(CRefTypes.Type, path.PathType);
            Assert.AreEqual("String", path.TypeName);
        }
    }
}
