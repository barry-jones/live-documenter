
namespace Runner
{
    using System;
    using System.Collections.Generic;
    using TheBoxSoftware.Reflection;
    using TheBoxSoftware.Reflection.Core;
    using TheBoxSoftware.Documentation;
    using TheBoxSoftware.Documentation.Exporting;

    internal class Program
    {
        private const string TestFile = @"DocumentationTest.dll";
        private const string ConfigFile = @"";

        static void Main(string[] args)
        {
            Program p = new Program();
            p.LoadAssemblyDefOnly();
            return;

            Console.WriteLine(string.Empty);
            Console.WriteLine("Please select from one of the options:");
            Console.WriteLine("\t1: Load PeCoffFile");
            Console.WriteLine("\t2: Load AssemblyDef");
            Console.WriteLine("\t3: Export");

            string item = string.Empty;
            int selection = 0;

            do
            {
                item = Console.ReadLine();
                if(!int.TryParse(item, out selection))
                {
                    Console.WriteLine("Selection unknown, please try again.");
                }
            }
            while(selection == 0);

            Program program = new Program();
            switch(selection)
            {
                case 1: program.LoadPeCoffFileOnly(); break;
                case 2: program.LoadAssemblyDefOnly(); break;
                case 3: program.LoadAssemblyDefAndExport(); break;
            }
        }

        private void LoadPeCoffFileOnly()
        {
            PeCoffFile file = new PeCoffFile(TestFile);
            file.Initialise();
        }

        private void LoadAssemblyDefOnly()
        {
            AssemblyDef assembly = AssemblyDef.Create(TestFile);
        }

        private void LoadAssemblyDefAndExport()
        {
            List<DocumentedAssembly> assemblies = new List<DocumentedAssembly>();
            DocumentedAssembly documentedAssembly = new DocumentedAssembly(TestFile);
            assemblies.Add(documentedAssembly);

            Document document = new Document(assemblies);

            ExportSettings settings = new ExportSettings();
            settings.PublishDirectory = string.Empty; // current directory
            ExportConfigFile config = ExportConfigFile.Create(ConfigFile);

            WebsiteExporter exporter = new WebsiteExporter(document, settings, config);
        }
    }
}
