using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TheBoxSoftware.API.LiveDocumenter;
using System.Xml;

namespace UT.TheBoxSoftware.API.LiveDocumenter
{
    [TestFixture]
    public class Documentation_Tests
    {
        private Documentation documentation;

        [SetUp]
        public void Init()
        {
            documentation = new Documentation(@"C:\Users\Barry\Documents\Current Projects\Live Documenter\DocumentationTest\DocumentationTest.csproj");
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Documentation_InvalidStateTest()
        {
            // throws an exception because we have not called load
            documentation.GetTableOfContents();
        }

        [Test]
        public void Documentation_Load()
        {

            documentation.Load();
            documentation.GetTableOfContents(); // shouldnt throw an exception
        }

        [Test]
        public void Documentation_GetDocumentationFor_Key()
        {
            documentation.Load();
            TableOfContents toc = documentation.GetTableOfContents();

            // find a key to search for
            ContentEntry toFind = toc.GetEntryFor("T:DocumentationTest.AllOutputTypesClass");

            // test find
            string found = documentation.GetDocumentationFor(toFind.Key);

            Assert.IsNotNull(found);
            Assert.AreEqual(toFind.Key.ToString(), this.getKey(found));
        }

        [Test]
        [ExpectedException(typeof(EntryNotFoundException))]
        public void Documentation_GetDocumentationFor_Key_NotFound()
        {
            documentation.Load();
            TableOfContents toc = documentation.GetTableOfContents();

            // test find
            string found = documentation.GetDocumentationFor(0);
        }

        [Test]
        public void Documentation_GetDocumentationFor_Cref()
        {
            string cref = "T:DocumentationTest.AllOutputTypesClass";
            string name = "AllOutputTypesClass";

            documentation.Load();
            TableOfContents toc = documentation.GetTableOfContents();

            // find a key to search for
            string found = documentation.GetDocumentationFor(cref);

            // test find

            Assert.IsNotNull(found);
            Assert.AreEqual(name, getSafeName(found));
        }

        [Test]
        [ExpectedException(typeof(EntryNotFoundException))]
        public void Documentation_GetDocumentationFor_Cref_NotFound()
        {
            string cref = "T:Test.Test.Test";

            documentation.Load();

            string found = documentation.GetDocumentationFor(cref);
        }

        [Test]
        public void Documentation_GetDocumentationFor_ContentEntry()
        {
            documentation.Load();
            TableOfContents toc = documentation.GetTableOfContents();

            // find a key to search for
            ContentEntry toFind = toc.GetEntryFor("T:DocumentationTest.AllOutputTypesClass");

            // test find
            string found = documentation.GetDocumentationFor(toFind);

            Assert.IsNotNull(found);
            Assert.AreEqual(toFind.Key.ToString(), this.getKey(found));
        }

        private string getSafeName(string d)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(d);
            XmlNode node = doc.SelectNodes("member/name")[0];
            return node.Attributes["safename"].Value;
        }

        private string getKey(string d)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(d);
            XmlNode node = doc.SelectNodes("member")[0];
            return node.Attributes["id"].Value;
        }
    }
}