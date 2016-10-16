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
            CRefPath path = CRefPath.Parse("M:string.ToUpper()");

            Assert.AreEqual(CRefTypes.Method, path.PathType);
            Assert.AreEqual("string", path.TypeName);
            Assert.AreEqual("ToUpper", path.ElementName);
        }
    }
}
