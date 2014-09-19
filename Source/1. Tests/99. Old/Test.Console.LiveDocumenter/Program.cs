using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheBoxSoftware.Documentation;
using TheBoxSoftware.Documentation.Exporting;
using System.IO;
using TheBoxSoftware.Reflection;

namespace Test.Console.LiveDocumenter
{
    class Program
    {
        private static Document docs;

        static void Main(string[] args)
        {
            Program.Initialise();

            TestCrefPath testCrefPath = new TestCrefPath();
            testCrefPath.RunTests(docs);
        }

        internal static void Initialise()
        {
            string forDocument = @"C:\Users\Barry\Documents\Development\Projects\Developer Suite\The Box Software Developer Suite.sln";
            List<DocumentedAssembly> files = new List<DocumentedAssembly>();
            Project project = null;
            ExportSettings settings = new ExportSettings();
            settings.Settings = new DocumentSettings();

            if (!File.Exists(forDocument))
                throw new InvalidOperationException(string.Format("The file {0} does not exist.", forDocument));

            // initialise the assemblies, ldproj file will detail all assemblies, we are only working
            // with ldproj, vs projects/solutions and dll files
            if (Path.GetExtension(forDocument) == ".ldproj")
            {
                project = Project.Deserialize(forDocument);
                foreach (string file in project.Files)
                {
                    files.Add(new DocumentedAssembly(file));
                }
                settings.Settings.VisibilityFilters = project.VisibilityFilters;
            }
            else if (Path.GetExtension(forDocument) == ".dll")
            {
                files.Add(new DocumentedAssembly(forDocument));
            }
            else
            {
                try
                {
                    files.AddRange(
                        InputFileReader.Read(
                        forDocument,
                        "Release"
                        ));
                }
                catch (ArgumentException)
                {
                    throw new Exception(
                        string.Format("The provided file [{0}] and extension is not supported", forDocument)
                        );
                }
            }

            settings.Settings.VisibilityFilters = new List<Visibility>() { Visibility.Public }; // we will always default to public

            System.Console.Write("Loading documentation ... ");
            // initialise the document
            EntryCreator entryCreator = new EntryCreator();
            docs = new Document(files, Mappers.NamespaceFirst, false, entryCreator);
            docs.Settings = settings.Settings;
            docs.UpdateDocumentMap();
            System.Console.Write("[done]\n");
        }
    }
}
