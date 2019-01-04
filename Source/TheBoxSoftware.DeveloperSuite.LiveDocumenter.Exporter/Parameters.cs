
<<<<<<< HEAD
namespace TheBoxSoftware.Exporter
=======
namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter
>>>>>>> updated test projects to net core 2.2 to enable testign of the exporter. Added parameters class to read parameters from the command line and associated tests.
{
    using System;
    using System.Linq;

    internal class Parameters
    {
        private readonly string[] PARAMETERS = { "-v", "-h", "-format", "-f", "-filters", "-to" };

        private bool _showVerbose = false;
        private bool _showHelp = false;
        private string _toLocation = string.Empty;
        private string _filters = string.Empty;
        private string _format = string.Empty;
        private string _export = string.Empty;
        private bool _hasParameters = false;

        public Parameters()
        {
        }

        public void Read(string[] parameters)
        {
            _hasParameters = hasParameters(parameters);

            if (_hasParameters)
            {
                readFileToExport(parameters);
                readVerbosity(parameters);
                readFormats(parameters);
                readTo(parameters);
                readFilters(parameters);
                readHelp(parameters);
            }
        }

        public bool IsValid()
        {
            bool isValid = false;

            isValid = _showHelp || string.IsNullOrEmpty(_export);

            return isValid;
        }

        private string ReadValue(int currentIndex, string[] parameters)
        {
            string readValue = string.Empty;
            int indexOfValue = (currentIndex + 1);

            if (indexOfValue < parameters.Length)
            {
                readValue = parameters[indexOfValue];
            }

            if (valueIsAParameter(readValue))
            {
                readValue = string.Empty;
            }

            return readValue;
        }

        private bool valueIsAParameter(string readValue)
        {
            return PARAMETERS.Contains(readValue);
        }

        private bool hasParameters(string[] parameters)
        {
<<<<<<< HEAD
            return parameters != null && parameters.Length > 0;
=======
            return parameters != null;
>>>>>>> updated test projects to net core 2.2 to enable testign of the exporter. Added parameters class to read parameters from the command line and associated tests.
        }

        private void readFileToExport(string[] parameters)
        {
            _export = parameters[0];
        }

        private void readVerbosity(string[] parameters)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                if ("-v" == parameters[i])
                {
                    _showVerbose = true;
                }
            }
        }

        private void readHelp(string[] parameters)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                if ("-h" == parameters[i])
                {
                    _showHelp = true;
                }
            }
        }

        private void readFormats(string[] parameters)
        {
            int index = Array.IndexOf(parameters, "-format");
            if (index != -1)
            {
                _format = ReadValue(index, parameters);
            }
        }

        private void readTo(string[] parameters)
        {
            int index = Array.IndexOf(parameters, "-to");
            if (index != -1)
            {
                _toLocation = ReadValue(index, parameters);
            }
        }

        private void readFilters(string[] parameters)
        {
            int index = Array.IndexOf(parameters, "-filters");
            if (index != -1)
            {
                _filters = ReadValue(index, parameters);
            }
        }

        public string FileToExport
        {
            get { return _export; }
        }

        public bool Verbose
        {
            get { return _showVerbose; }
        }

        public string Format
        {
            get { return _format; }
        }

        public string To
        {
            get { return _toLocation; }
        }

        public string Filters
        {
            get { return _filters; }
        }

        public bool ShowHelp
        {
            get { return _showHelp; }
        }
<<<<<<< HEAD

        public bool HasParameters
        {
            get { return _hasParameters; }
        }
=======
>>>>>>> updated test projects to net core 2.2 to enable testign of the exporter. Added parameters class to read parameters from the command line and associated tests.
    }
}
