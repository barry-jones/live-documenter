
namespace TheBoxSoftware.Exporter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TheBoxSoftware.Reflection;

    /// <summary>
    /// Reads and processes the command line arguments for the Exporter application.
    /// </summary>
    internal class Parameters
    {
        private const string DefaultFormat = "web-msdn.ldec";
        private readonly string[] PARAMETERS = { "-v", "-h", "-format", "-f", "-filters", "-to" };

        private bool _showVerbose = false;
        private bool _showHelp = false;
        private string _toLocation = string.Empty;
        private List<Visibility> _filters = new List<Visibility>();
        private string _format = string.Empty;
        private string _export = string.Empty;
        private bool _hasParameters = false;

        public Parameters()
        {
        }

        public void Read(string[] parameters)
        {
            _hasParameters = CheckForParameters(parameters);

            if (_hasParameters)
            {
                ReadFileToExport(parameters);
                ReadVerbosity(parameters);
                ReadFormats(parameters);
                ReadTo(parameters);
                ReadFilters(parameters);
                ReadHelp(parameters);
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

            if (ValueIsAParameter(readValue))
            {
                readValue = string.Empty;
            }

            return readValue;
        }

        private bool ValueIsAParameter(string readValue)
        {
            return PARAMETERS.Contains(readValue);
        }

        private bool CheckForParameters(string[] parameters)
        {
            return parameters != null && parameters.Length > 0;
        }

        private void ReadFileToExport(string[] parameters)
        {
            _export = parameters[0];
        }

        private void ReadVerbosity(string[] parameters)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                if ("-v" == parameters[i])
                {
                    _showVerbose = true;
                }
            }
        }

        private void ReadHelp(string[] parameters)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                if ("-h" == parameters[i])
                {
                    _showHelp = true;
                }
            }
        }

        private void ReadFormats(string[] parameters)
        {
            int index = Array.IndexOf(parameters, "-format");
            if (index != -1)
            {
                _format = ReadValue(index, parameters);
            }
            if(string.IsNullOrEmpty(_format))
            {
                _format = DefaultFormat;
            }
        }

        private void ReadTo(string[] parameters)
        {
            int index = Array.IndexOf(parameters, "-to");
            if (index != -1)
            {
                _toLocation = ReadValue(index, parameters);
            }
        }

        private void ReadFilters(string[] parameters)
        {
            int index = Array.IndexOf(parameters, "-filters");
            string value = string.Empty;
            if (index != -1)
            {
                value = ReadValue(index, parameters);
            }

            if(string.IsNullOrEmpty(value))
            {
                AddDefaultVisibilityFilters();
            }
            else
            {
                ConvertFilters(value);
            }
        }

        private void ConvertFilters(string value)
        {
            foreach (string current in value.Split('|'))
            {
                Visibility parsed;
                bool hasParsed = Enum.TryParse<Visibility>(current, true, out parsed);
                if (hasParsed)
                    _filters.Add(parsed);
                else
                    throw new InvalidParameterException("formats", current);
            }
        }

        private void AddDefaultVisibilityFilters()
        {
            _filters.Add(Visibility.Public);
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

        public List<Visibility> Filters
        {
            get { return _filters; }
        }

        public bool ShowHelp
        {
            get { return _showHelp; }
        }

        public bool HasParameters
        {
            get { return _hasParameters; }
        }
    }
}
