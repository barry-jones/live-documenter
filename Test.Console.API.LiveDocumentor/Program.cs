using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using TheBoxSoftware.API.LiveDocumentor;

namespace Test.Console.API.LiveDocumentor {
    class Program {
        static void Main(string[] args) {
            Documentation d = new Documentation(@"C:\Users\Barry\Documents\Current Projects\Live Documenter\The Box Software Developer Suite.sln");
            System.Console.WriteLine("Loading documentation...");
            d.Load();
            System.Console.WriteLine("Loaded.");

            // write out the table of contents
            System.Console.WriteLine();
            System.Console.WriteLine("XML ToC");
            System.Console.WriteLine("-------");
            XmlDocument xmlToc = d.GetTableOfContents();

            xmlToc.Save(System.Console.Out);

            // pause so the user can see it
            System.Console.ReadLine();
        }
    }
}
