
namespace TheBoxSoftware.Exporter
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Diagnostics;

    internal class Program
    {
		static void Main(string[] args)
        {
            // https://github.com/dotnet/corefx/issues/31390
            AppContext.SetSwitch("Switch.System.Xml.AllowDefaultResolver", true);

            Program p = new Program();

            bool printHelp = false;
            Configuration configuration = null;
            bool verbose = false;

            Console.WriteLine(string.Empty); // always start the output with a new line clearing from the command data

            // read all the arguments
            if(args == null || args.Length == 0)
            {
                printHelp = true;
            }
            else
            {
                string configFile;

                p.ReadArguments(args, out configFile, out verbose, out printHelp);

                if(!printHelp)
                {
                    if(string.IsNullOrEmpty(configFile))
                    {
                        Logger.Log("No configuration file was provided.\n", LogType.Error);
                    }
                    else if(File.Exists(configFile))
                    {
                        try
                        {
                            configuration = Configuration.Deserialize(configFile);

                            // if no filters are defined, default to Public/Protected
                            if(configuration.Filters == null || configuration.Filters.Count == 0)
                            {
                                configuration.Filters.Add(Reflection.Visibility.Public);
                                configuration.Filters.Add(Reflection.Visibility.Protected);
                            }
                        }
                        catch(InvalidOperationException e)
                        {
                            Logger.Log(string.Format("There was an error reading the configuration file\n  {0}", e.Message), LogType.Error);
                            return; // bail we have no configuration or some of it is missing
                        }
                    }
                    else
                    {
                        Logger.Log(string.Format("The config file '{0}' does not exist", configFile), LogType.Error);
                    }
                }
            }

            if(printHelp)
            {
                p.PrintHelp();
            }
            else if(configuration != null)
            {
                if(configuration.IsValid())
                {
                    Logger.Init(verbose);
                    p.PrintVersionInformation();

                    Exporter exporter = new Exporter(configuration, verbose);
                    exporter.Export();
                }
            }

            Console.WriteLine(); // space at end of outpuut for readability
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
            Parameters parameters = new Parameters();
            parameters.Read(args);

            showHelp = parameters.ShowHelp;
            verbose = parameters.Verbose;
            configuration = parameters.FileToExport;
        }

        /// <summary>
        /// Outputs the help information
        /// </summary>
        private void PrintHelp()
        {
            PrintVersionInformation();

            string help =
                "\nThe exporter takes the following arguments\n" +
                "   exporter <filename> mmodifiers\n\n" +
                "   modifiers:\n" +
                "     -h        show help information\n" +
                "     -v        show verbose export details\n\n" +
                "   <filename>  The path to the configuration xml file.\n";
            Logger.Log(help);
        }

        /// <summary>
        /// Prints the exporters current version and details to the console.
        /// </summary>
        private void PrintVersionInformation()
        {
            // get version information
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

            Logger.Verbose($"Live Documenter Exporter Version: {fvi.ProductVersion}\n\n");
        }
    }
}