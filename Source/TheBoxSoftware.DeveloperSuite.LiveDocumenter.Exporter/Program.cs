
namespace TheBoxSoftware.Exporter
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Diagnostics;

    internal class Program
    {
        private readonly IFileSystem _filesystem;
        private readonly string[] _arguments;
        private readonly IUserInterface _ui;
        private readonly ILog _log;

		static void Main(string[] args)
        {
            // https://github.com/dotnet/corefx/issues/31390
            AppContext.SetSwitch("Switch.System.Xml.AllowDefaultResolver", true);

            ConsoleUserInterface ui = new ConsoleUserInterface();
            Program p = new Program(args, new FileSystem(), ui, new Logger(ui));

            p.HandleExport();
        }

        public Program(string[] arguments, IFileSystem fileSystem, IUserInterface ui, ILog logger)
        {
            _filesystem = fileSystem;
            _arguments = arguments;
            _ui = ui;
            _log = logger;
        }

        private void HandleExport()
        {
            Configuration configuration = null;

            _ui.WriteLine(string.Empty); // always start the output with a new line clearing from the command data

            Parameters parameters = new Parameters();
            parameters.Read(_arguments);

            if (!parameters.HasParameters || parameters.ShowHelp)
            {
                PrintHelp();
            }
            else
            {
                string configFile = parameters.FileToExport;

                if (string.IsNullOrEmpty(configFile))
                {
                    _log.Log("No configuration file was provided.\n", LogType.Error);
                }
                else if (_filesystem.FileExists(configFile))
                {
                    try
                    {
                        configuration = Configuration.Deserialize(configFile);

                        // if no filters are defined, default to Public/Protected
                        if (configuration.Filters == null || configuration.Filters.Count == 0)
                        {
                            configuration.Filters.Add(Reflection.Visibility.Public);
                            configuration.Filters.Add(Reflection.Visibility.Protected);
                        }
                    }
                    catch (InvalidOperationException e)
                    {
                        _log.Log($"There was an error reading the configuration file\n  {e.Message}", LogType.Error);
                        return; // bail we have no configuration or some of it is missing
                    }
                }
                else
                {
                    _log.Log($"The config file '{configFile}' does not exist", LogType.Error);
                }
            }

            if (configuration != null)
            {
                if (configuration.IsValid(_log))
                {
                    _log.Init(parameters.Verbose);
                    PrintVersionInformation();

                    Exporter exporter = new Exporter(configuration, parameters.Verbose, _log);
                    exporter.Export();
                }
            }

            _ui.WriteLine(string.Empty);
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

        private void PrintHelp()
        {
            PrintVersionInformation();

            string help =
                "\nThe exporter takes the following arguments\n" +
                "   exporter <filename> mmodifiers\n\n" +
                "   <filename>  The path to the configuration xml file.\n" +
                "   modifiers:\n" +
                "     -h        show help information\n" +
                "     -v        show verbose export details\n\n";
            _log.Log(help);
        }

        /// <summary>
        /// Prints the exporters current version and details to the console.
        /// </summary>
        private void PrintVersionInformation()
        {
            // get version information
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

            _log.Verbose($"Live Documenter Exporter Version: {fvi.ProductVersion}\n\n");
        }
    }
}