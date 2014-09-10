using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace TheBoxSoftware.Exporter {
	internal class Program {
        /// <summary>
        /// Application entry point.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
		static void Main(string[] args) {
            Program p = new Program();

			bool printHelp = false;
			Configuration configuration = null;
            bool verbose = false;

			// read all the arguments
			if (args == null || args.Length == 0) {
				printHelp = true;
			}
			else {
                string configFile;

                p.ReadArguments(args, out configFile, out verbose, out printHelp);

				if (!printHelp) {
                    if (string.IsNullOrEmpty(configFile))
                    {
                        Console.Write("  [error] No configuration file was provided.\n");
                    }
                    else if (File.Exists(configFile))
                    {
                        try
                        {
                            configuration = Configuration.Deserialize(configFile);
                        }
                        catch (InvalidOperationException e)
                        {
                            Console.Write(string.Format("! there was an error reading the configuration file\n  {0}", e.Message));
                            return; // bail we have no configuration or some of it is missing
                        }
                    }
                    else
                    {
                        Console.Write(string.Format("  [error] the config file '{0}' does not exist", configFile));
                    }
				}
			}

			if (printHelp) {
				p.PrintHelp();
			}
			else if(configuration != null) {
				if (configuration.IsValid()) {
                    p.PrintVersionInformation();

					Exporter exporter = new Exporter(configuration, verbose);
					exporter.Export();
				}
			}
		}

        /// <summary>
        /// Reads the arguments from the command line.
        /// </summary>
        /// <param name="args">The arguments provided by the user.</param>
        /// <param name="configuration">The configuration file to be processed.</param>
        /// <param name="verbose">Indicates if the output should be verbose or not.</param>
        /// <remarks>
        /// <para>The command line takes the following arguments:</para>
        /// <list type="">
        ///     <item>-h show help</item>
        ///     <item>-v verbose output</item>
        ///     <item>[file] configuration file</item>
        /// </list>
        /// </remarks>
        private void ReadArguments(string[] args, out string configuration, out bool verbose, out bool showHelp)
        {
            List<string> arguments = new List<string>(args);

            // pre the output variables
            configuration = string.Empty;
            verbose = false;
            showHelp = false;

            foreach (string modifier in arguments)
            {
                switch (modifier)
                {
                    case "-h":
                    case "help":
                    case "?":
                        showHelp = true;
                        break;
                    case "-v":
                        verbose = true;
                        break;
                }
            }

            // get the details of the configuration file.
            if(arguments.Count > 0)
            {
                string lastItem = arguments[arguments.Count - 1];
                if (!lastItem.StartsWith("-"))
                {
                    configuration = lastItem;
                }
            }
        }

		/// <summary>
		/// Outputs the help information
		/// </summary>
		private void PrintHelp() {
            this.PrintVersionInformation();

            string help =
                "The exporter takes the following arguments\n" +
                "   exporter [modifiers] <filename>\n\n" +
                "   modifiers:\n" +
                "     -h        show help information\n" +
                "     -v        show verbose export details\n" +
                "   <filename>  The path to the configuration xml file.\n";
            Console.Write(help);
		}

        /// <summary>
        /// Prints the exporters current version and details to the console.
        /// </summary>
        private void PrintVersionInformation()
        {
            // get version information
            Version version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;

            Console.Write(string.Format("Live Documenter Exporter Version: {0}\n", version.ToString()));
        }
	}
}
