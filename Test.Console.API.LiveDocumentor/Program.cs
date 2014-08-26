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

            // pause so the user can see it
            System.Console.ReadLine();
        }

        internal void Initialise()
        {
            this.docs = new Documentation(@"C:\Users\Barry\Documents\Current Projects\Live Documenter\The Box Software Developer Suite.sln");
            System.Console.Write("Loading documentation ... ");
            this.docs.Load();
            System.Console.Write("[done]\n");
        }

        private void SearchByCref() {
            XmlDocument d;

            System.Console.Write("Searching for field F:DocumentationTest.CommentTests.SeeAlsoElement.SeeAlsoOnField ... ");
            d = this.docs.GetDocumentationFor("F:DocumentationTest.CommentTests.SeeAlsoElement.SeeAlsoOnField");
            System.Console.Write("[done]\n");
            System.Console.WriteLine(string.Format("\t {0} - {1}\n", (d != null) ? "found" : "not found", (d != null) ? this.getSafeName(d) : string.Empty));

            System.Console.Write("Searching for type T:DocumentationTest.GenericClass`1 ... ");
            d = this.docs.GetDocumentationFor("T:DocumentationTest.GenericClass`1");
            System.Console.Write("[done]\n");
            System.Console.WriteLine(string.Format("\t {0} - {1}\n", (d != null) ? "found" : "not found", (d != null) ? this.getSafeName(d) : string.Empty));

            // cref paths for generic classes are pretty horendous :/
            System.Console.Write("Searching for method M:DocumentationTest.GenericClass`1.GenericMethod``1(`0,``0) ... ");
            d = this.docs.GetDocumentationFor("M:DocumentationTest.GenericClass`1.GenericMethod``1(`0,``0)");
            System.Console.Write("[done]\n");
            System.Console.WriteLine(string.Format("\t {0} - {1}\n", (d != null) ? "found" : "not found", (d != null) ? this.getSafeName(d) : string.Empty));

            System.Console.Write("Searching for property P:DocumentationTest.CommentTests.SummaryElement.SummaryOnProperty ... ");
            d = this.docs.GetDocumentationFor("P:DocumentationTest.CommentTests.SummaryElement.SummaryOnProperty");
            System.Console.Write("[done]\n");
            System.Console.WriteLine(string.Format("\t {0} - {1}\n", (d != null) ? "found" : "not found", (d != null) ? this.getSafeName(d) : string.Empty));
        }

        private void Search()
        {
            XmlDocument d;

            System.Console.Write("Searching for text 'genericclass' ... ");
            d = this.docs.Search("genericclass");
            System.Console.Write("[done]\n");
            System.Console.WriteLine(string.Format("\t {0} - {1}\n", (d != null) ? "found" : "not found", (d != null) ? this.getSafeName(d) : string.Empty));
        }

        private string getSafeName(XmlDocument d)
        {
            XmlNode node = d.SelectNodes("member/name")[0];
            return node.Attributes["safename"].Value;
        }
    }
}
