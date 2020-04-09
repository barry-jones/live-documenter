
namespace TheBoxSoftware.Exporter
{
    using System;
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

        public void HandleExport()
        {
            Configuration configuration = null;

            _ui.WriteLine(string.Empty); // always start the output with a new line clearing from the command data

            Parameters parameters = new Parameters();
            try
            {
                parameters.Read(_arguments);
            }
            catch(InvalidParameterException ex)
            {
                _log.LogError($"An invalid value '{ex.Value}' for parameter '{ex.Parameter}' was provided. Please resolve and try again.");
                return;
            }

            string configFile = parameters.FileToExport;

            if (IsHelpShown(parameters))
                return;

            if (IsFileNotProvided(configFile))
                return;

            if (IsConfigurationFile(configFile))
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
                    _log.LogError($"There was an error reading the configuration file\n  {e.Message}");
                    return; // bail we have no configuration or some of it is missing
                }
            }
            else
            {
                configuration = new Configuration();
                configuration.Document = configFile;
                configuration.Filters.AddRange(parameters.Filters);
                configuration.AddOutput(parameters.To, parameters.Format);
            }

            if (configuration != null)
            {
                if (configuration.IsValid(_log))
                {
                    PrintVersionInformation();

                    Exporter exporter = new Exporter(configuration, parameters.Verbose, _log);
                    exporter.Export();
                }
            }

            _ui.WriteLine(string.Empty);
        }

        private static bool IsConfigurationFile(string configFile)
        {
            return System.IO.Path.GetExtension(configFile) == ".xml";
        }

        private bool IsHelpShown(Parameters parameters)
        {
            bool showHelp = !parameters.HasParameters || parameters.ShowHelp;

            if (showHelp)
            {
                PrintVersionInformation();

                string help =
                    "\nThe exporter takes the following arguments\n" +
                    "   exporter <filename> mmodifiers\n\n" +
                    "   [e.g.] exporter theboxsoftware.reflection.dll -to c:\\temp\\web -filters \"public|protected\"\n\n" +
                    "   <filename>  The path to the configuration file, library, project or solution.\n" +
                    "   modifiers:\n" +
                    "     -h        show help information\n" +
                    "     -v        show verbose export details\n" +
                    "     -to       the directory to export to\n" +
                    "     -format   the ldec file format to export content. Defaults to web-msdn.ldec\n" +
                    "     -filters  the visibilty filters (public|protected etc) defaults to public\n\n" +
                    "\n`-to`, `-format` and `-filters` are only used when the file provided is not a\n" +
                    "configuration xml file.\n\n";

                _ui.Write(help);

            }
            return showHelp;
        }

        private bool IsFileNotProvided(string configFile)
        {
            bool isFileProvided = false;

            if (string.IsNullOrEmpty(configFile))
            {
                _log.LogError($"No file was specified to export.\n");
                isFileProvided = true;
            }
            else if (!_filesystem.FileExists(configFile))
            {
                _log.LogError($"The config file '{configFile}' does not exist");
                isFileProvided = true;
            }

            return isFileProvided;
        }
        
        /// <summary>
        /// Prints the exporters current version and details to the console.
        /// </summary>
        private void PrintVersionInformation()
        {
            // get version information
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

            _log.LogInformation($"Live Documenter Exporter Version: {fvi.ProductVersion}\n\n");
        }
    }
}