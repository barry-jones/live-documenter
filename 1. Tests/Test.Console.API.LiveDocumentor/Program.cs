using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using TheBoxSoftware.API.LiveDocumenter;

namespace Test.Console.API.LiveDocumentor {
    class Program {
        private Documentation docs;

        static void Main(string[] args) {
            Program p = new Program();
            
            p.Initialise();
            p.SearchByCref();
            p.Search();
            p.GetFullTableOfContents();
            p.OutputTree();
            p.OutputParents();
        }

        internal static void Log(string details)
        {
            System.Console.Write(details);
        }

        internal void Initialise()
        {
            this.docs = new Documentation(@"C:\Users\Barry\Documents\Current Projects\Live Documenter\The Box Software Developer Suite.sln");
            Log("Loading documentation ... ");
            this.docs.Load();
            Log("[done]\n");
        }

        private void SearchByCref() {
            Log("\nTest searching by crefpath\n");

            XmlDocument d;

            Log(" F:DocumentationTest.CommentTests.SeeAlsoElement.SeeAlsoOnField ... ");
            d = this.docs.GetDocumentationFor("F:DocumentationTest.CommentTests.SeeAlsoElement.SeeAlsoOnField");
            Log("[done]\n");
            Log(string.Format("  {0} - {1}\n", (d != null) ? "found" : "not found", (d != null) ? this.getSafeName(d) : string.Empty));

            Log(" T:DocumentationTest.GenericClass`1 ... ");
            d = this.docs.GetDocumentationFor("T:DocumentationTest.GenericClass`1");
            Log("[done]\n");
            Log(string.Format("  {0} - {1}\n", (d != null) ? "found" : "not found", (d != null) ? this.getSafeName(d) : string.Empty));

            // cref paths for generic classes are pretty horendous :/
            Log(" M:DocumentationTest.GenericClass`1.GenericMethod``1(`0,``0) ... ");
            d = this.docs.GetDocumentationFor("M:DocumentationTest.GenericClass`1.GenericMethod``1(`0,``0)");
            Log("[done]\n");
            Log(string.Format("  {0} - {1}\n", (d != null) ? "found" : "not found", (d != null) ? this.getSafeName(d) : string.Empty));

            Log(" P:DocumentationTest.CommentTests.SummaryElement.SummaryOnProperty ... ");
            d = this.docs.GetDocumentationFor("P:DocumentationTest.CommentTests.SummaryElement.SummaryOnProperty");
            Log("[done]\n");
            Log(string.Format("  {0} - {1}\n", (d != null) ? "found" : "not found", (d != null) ? this.getSafeName(d) : string.Empty));
        }

        private void Search()
        {
            List<ContentEntry> results;
            TableOfContents contents = this.docs.GetTableOfContents();

            Log("\nSearching for text 'genericclass' ... ");
            results = contents.Search("genericclass");
            Log("[done]\n");
            foreach (ContentEntry current in results)
            {
                Log(string.Format(" {0}\n", current.CRefPath));
            }
        }

        private string getSafeName(XmlDocument d)
        {
            XmlNode node = d.SelectNodes("member/name")[0];
            return node.Attributes["safename"].Value;
        }

        private void GetFullTableOfContents()
        {
            Log("\nOutputting all top level entries");

            TableOfContents contents = this.docs.GetTableOfContents();

            foreach (ContentEntry current in contents)
            {
                Log(string.Format(" Top level entry: {0}\n", current.DisplayName));
            }
        }

        private void OutputParents()
        {
            Log("\nOutput parents using GetParents method\n");
            List<ContentEntry> parents;
            TableOfContents contents = this.docs.GetTableOfContents();
            ContentEntry entry = contents.GetDocumentationFor("M:TheBoxSoftware.Reflection.AssemblyDef.#ctor");

            parents = entry.GetParents();

            foreach (ContentEntry current in parents)
            {
                Log(string.Format(" {0}\n", current.DisplayName));
            }
        }

        private void OutputTree()
        {
            Log("\nOutput parents and direct children\n");

            TableOfContents contents = this.docs.GetTableOfContents();
            ContentEntry entry = contents.GetDocumentationFor("T:TheBoxSoftware.Reflection.AssemblyDef");
            int level = 0;
            
            // get the parents
            List<ContentEntry> parents = new List<ContentEntry>();
            ContentEntry currentParent = entry.Parent;
            while (currentParent != null)
            {
                parents.Insert(0, currentParent);
                currentParent = currentParent.Parent;
            }
            level = parents.Count;

            // output the parents
            int currentLevel = 0;
            foreach (ContentEntry current in parents)
            {
                Log(string.Format("{0}{1}\n", new string(' ', currentLevel), current.DisplayName));
                currentLevel++;
            }

            Log(string.Format("{0}{1}\n", new string(' ', currentLevel), entry.DisplayName));
            currentLevel++;

            foreach (ContentEntry current in entry.Children)
            {
                Log(string.Format("{0}{1}\n", new string(' ', currentLevel), current.DisplayName));
            }
        }
    }
}
