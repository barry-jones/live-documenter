using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TheBoxSoftware.Documentation.Tests.Unit
{
    [TestFixture]
    public class DocumentTest
    {
        [Test]
        public void Create()
        {
            Document document = new Document(null);
            Assert.IsFalse(document.HasFiles);

            document.Search("test");
            //document.UpdateDocumentMap();
        }
    }
}
