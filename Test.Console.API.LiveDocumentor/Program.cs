using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using TheBoxSoftware.API.LiveDocumentor;

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
        }

        internal void Initialise()
        {
            this.docs = new Documentation(@"C:\Users\Barry\Documents\Current Projects\Live Documenter\The Box Software Developer Suite.sln");
            System.Console.Write("Loading documentation ... ");
            this.docs.Load();
            System.Console.Write("[done]\n");
        }

        private void SearchByCref() {
            System.Console.WriteLine();
            System.Console.WriteLine("Test searching by crefpath");

            XmlDocument d;

            System.Console.Write(" F:DocumentationTest.CommentTests.SeeAlsoElement.SeeAlsoOnField ... ");
            d = this.docs.GetDocumentationFor("F:DocumentationTest.CommentTests.SeeAlsoElement.SeeAlsoOnField");
            System.Console.Write("[done]\n");
            System.Console.WriteLine(string.Format("  {0} - {1}", (d != null) ? "found" : "not found", (d != null) ? this.getSafeName(d) : string.Empty));

            System.Console.Write(" T:DocumentationTest.GenericClass`1 ... ");
            d = this.docs.GetDocumentationFor("T:DocumentationTest.GenericClass`1");
            System.Console.Write("[done]\n");
            System.Console.WriteLine(string.Format("  {0} - {1}", (d != null) ? "found" : "not found", (d != null) ? this.getSafeName(d) : string.Empty));

            // cref paths for generic classes are pretty horendous :/
            System.Console.Write(" M:DocumentationTest.GenericClass`1.GenericMethod``1(`0,``0) ... ");
            d = this.docs.GetDocumentationFor("M:DocumentationTest.GenericClass`1.GenericMethod``1(`0,``0)");
            System.Console.Write("[done]\n");
            System.Console.WriteLine(string.Format("  {0} - {1}", (d != null) ? "found" : "not found", (d != null) ? this.getSafeName(d) : string.Empty));

            System.Console.Write(" P:DocumentationTest.CommentTests.SummaryElement.SummaryOnProperty ... ");
            d = this.docs.GetDocumentationFor("P:DocumentationTest.CommentTests.SummaryElement.SummaryOnProperty");
            System.Console.Write("[done]\n");
            System.Console.WriteLine(string.Format("  {0} - {1}", (d != null) ? "found" : "not found", (d != null) ? this.getSafeName(d) : string.Empty));
        }

        private void Search()
        {
            System.Console.WriteLine();
            XmlDocument d;

            System.Console.Write(" Searching for text 'genericclass' ... ");
            d = this.docs.Search("genericclass");
            System.Console.Write("[done]\n");
            System.Console.WriteLine(string.Format("  {0} - {1}", (d != null) ? "found" : "not found", (d != null) ? this.getSafeName(d) : string.Empty));
        }

        private string getSafeName(XmlDocument d)
        {
            XmlNode node = d.SelectNodes("member/name")[0];
            return node.Attributes["safename"].Value;
        }

        private void GetFullTableOfContents()
        {
            System.Console.WriteLine();
            System.Console.WriteLine("Outputting all top level entries");

            TableOfContents contents = this.docs.GetTableOfContents();

            foreach (ContentEntry current in contents)
            {
                System.Console.WriteLine(string.Format(" Top level entry: {0}", current.DisplayName));
            }

            foreach (ContentEntry current in contents)
            {
                System.Console.WriteLine(string.Format(" Top level entry: {0}", current.CRefPath));
            }
        }

        private void OutputTree()
        {
            System.Console.WriteLine();
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
                System.Console.WriteLine(string.Format("{0}{1}", new string(' ', currentLevel), current.DisplayName));
                currentLevel++;
            }

            System.Console.WriteLine(string.Format("{0}{1}", new string(' ', currentLevel), entry.DisplayName));
            currentLevel++;

            foreach (ContentEntry current in entry.Children)
            {
                System.Console.WriteLine(string.Format("{0}{1}", new string(' ', currentLevel), current.DisplayName));
            }
        }
    }
}
