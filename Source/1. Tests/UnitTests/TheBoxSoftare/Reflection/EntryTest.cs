using TheBoxSoftware.Documentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TheBoxSoftware.Reflection.Comments;
using System.Collections.Generic;

namespace UnitTests
{
    [TestClass()]
    public class EntryTest
    {
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        /// <summary>
        ///A test for FindByKey
        ///</summary>
        [TestMethod()]
        public void FindByKeyTest_NullDisplayName()
        {
            // test if passing a null string works
            Entry target = new Entry(new object(), null, new XmlCodeCommentFile()); // default
            target.Key = 1;

            Entry found = null;
            
            // test null string
            found = target.FindByKey(1, null);
            Assert.AreEqual(found, target);

            // test empty string
            found = target.FindByKey(1, string.Empty);
            Assert.AreEqual(found, target);

            found = target.FindByKey(1, "name");
            Assert.IsNull(found);                       // shouldnt find anything
        }

        [TestMethod()]
        public void FindByKeyTest_WithSubKey()
        {
            // test if passing a null string works
            Entry target = new Entry(new object(), "test", new XmlCodeCommentFile()); // default
            target.Key = 1;
            target.SubKey = "test";

            Entry found = null;

            // test null string
            found = target.FindByKey(1, null);
            Assert.IsNull(found);                       // shouldnt find anything

            // test empty string
            found = target.FindByKey(1, string.Empty);
            Assert.IsNull(found);                       // shouldnt find anything

            found = target.FindByKey(1, "name");
            Assert.AreEqual(found, target);             // should find the target entry
        }

        [TestMethod()]
        public void FindNamespaceTest()
        {
            // checks the start entry and parent entries for an Entry that represents a namespace
        }


        /// <summary>
        ///A test for Search
        ///</summary>
        [TestMethod()]
        public void SearchTest()
        {
            object item = null; // TODO: Initialize to an appropriate value
            string displayName = string.Empty; // TODO: Initialize to an appropriate value
            XmlCodeCommentFile xmlComments = null; // TODO: Initialize to an appropriate value
            Entry target = new Entry(item, displayName, xmlComments); // TODO: Initialize to an appropriate value
            string searchText = string.Empty; // TODO: Initialize to an appropriate value
            List<Entry> expected = null; // TODO: Initialize to an appropriate value
            List<Entry> actual;
            actual = target.Search(searchText);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
