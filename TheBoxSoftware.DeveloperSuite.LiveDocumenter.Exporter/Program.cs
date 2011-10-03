using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace TheBoxSoftware.Exporter {
	internal class Program {
		static void Main(string[] args) {
			if (DateTime.Now > new DateTime(2011, 10, 31)) {
				System.Windows.MessageBox.Show("This beta version of has expired.", "Software Expired");
			}

			bool printHelp = false;
			Configuration configuration = null;

			// read all the arguments
			if (args == null || args.Length == 0) {
				printHelp = true;
			}
			else if (args.Length != 1) {
				Console.WriteLine("Invalid numnber of arguments specified, only expecting one.");
				printHelp = true;
			}
			else {
				string argument = args[0].ToLower();
				string[] parts = argument.Split(':', ' ');

				if (argument.StartsWith("help") || argument.StartsWith("?")) {
					printHelp = true;
				}
				else {
					configuration = Configuration.Deserialize(argument);
				}
			}

			if (printHelp) {
				Program.PrintHelp();
			}
			else {
				if (configuration.IsValid()) {
					Exporter exporter = new Exporter(configuration);
					exporter.Export();
				}
			}
		}

		/// <summary>
		/// Outputs the help information
		/// </summary>
		private static void PrintHelp() {
			string[] lines = new string[] {
			//	"-------------------------------------------------------------------------------",
				"The exporter takes the following arguments",
				"",
				"    exporter \"configuration.xml\"",
				"",
				"  <filename>: The path to the configuration xml file.",
				"  help:       Prints the help text.",
			};
			foreach (string line in lines) {
				Console.WriteLine(line);
			}
		}
	}
}
