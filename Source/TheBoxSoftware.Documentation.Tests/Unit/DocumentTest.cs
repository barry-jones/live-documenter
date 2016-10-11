using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TheBoxSoftware.Reflection.Comments;

namespace TheBoxSoftware.Documentation.Tests.Unit
{
    [TestFixture]
    public class DocumentTest
    {
        [Test]
        public void Document_WhenCreated_HasNoFiles()
        {
            Document document = new Document(null);

            Assert.IsFalse(document.HasFiles);
        }

        [Test]
        public void Document_Find_WhenPathIsNull_ReturnsNull()
        {
            Document document = new Document(null);

            Entry found = document.Find(null);

            Assert.IsNull(found);
        }

        [Test]
        public void Document_Find_WhenPathTypeIsError_ReturnsNull()
        {
            Document document = new Document(null);
            CRefPath path = new CRefPath();
            path.PathType = CRefTypes.Error;

            Entry found = document.Find(path);

            Assert.IsNull(found);
        }
    }
}
