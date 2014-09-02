using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace TheBoxSoftware.Exporter {
	internal class Program {
		static void Main(string[] args) {
            Program p = new Program();

			bool printHelp = false;
			Configuration configuration = null;

			// read all the arguments
			if (args == null || args.Length == 0) {
				printHelp = true;
			}
			else if (args.Length != 1) {
				Console.WriteLine("Invalid numnber of arguments specified, only expecting one.");
			}
			else {
				string argument = args[0].ToLower();
				string[] parts = argument.Split(':', ' ');

				if (argument.StartsWith("help") || argument.StartsWith("?")) {
					printHelp = true;
				}
				else {
                    try
                    {
                        configuration = Configuration.Deserialize(argument);
                    }
                    catch (InvalidOperationException e)
                    {
                        Console.Write(string.Format("! there was an error reading the configuration file\n  {0}", e.Message));
                        return; // bail we have no configuration or some of it is missing
                    }
				}
			}

			if (printHelp) {
				p.PrintHelp();
			}
			else if(configuration != null) {
				if (configuration.IsValid()) {
					Exporter exporter = new Exporter(configuration);
					exporter.Export();
				}
			}
		}

		/// <summary>
		/// Outputs the help information
		/// </summary>
		private void PrintHelp() {
			string help = 
				"The exporter takes the following arguments\n" +
                "   exporter <filename>\n\n" +
                "   <filename>  The path to the configuration xml file.\n" +
				"   help        Prints the help text.\n";
            Console.Write(help);
		}
	}
}
