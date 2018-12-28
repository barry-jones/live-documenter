
namespace TheBoxSoftware.Documentation.Exporting
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;

    /// <summary>
    /// Reads a file that contains all of the information needed to perform an export.
    /// </summary>
    /// <remarks>
    /// This file is a zip file which contains the following files:
    /// <list type="">
    ///		<item>export.config [required] describes the main details of the export</item>
    /// </list>
    /// </remarks>
    public class ExportConfigFile
    {
        private XmlDocument _xmlDocument;
        private string _configFile;
        private string _name;
        private Exporters _exporters;
        private string _description;
        private string _version;
        private bool _hasScreenshot;
        private Dictionary<string, string> _properties;
        private bool _isValid;
        private bool _isInitialised = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportConfigFile"/> class.
        /// </summary>
        /// <param name="filename">The file.</param>
        public ExportConfigFile(string filename)
        {
            if (string.IsNullOrEmpty(filename)) throw new ArgumentException("filename");

            _properties = new Dictionary<string, string>();
            _configFile = filename;
        }

        public void Initialise()
        {
            _isInitialised = true;

            using (ICompressedConfigFile tempFile = new ZipCompressedConfigFile(_configFile))
            {
                if (tempFile.HasEntry("export.config"))
                {
                    Stream stream = tempFile.GetEntry("export.config");
                    _xmlDocument = new XmlDocument();
                    _xmlDocument.LoadXml(new StreamReader(stream).ReadToEnd());
                    stream.Close();

                    ReadConfigurationDetails();
                    CheckIsValid(tempFile);
                }
            }
        }

        /// <summary>
        /// Gets the XSLT file from the export configuration file.
        /// </summary>
        /// <returns></returns>
        public virtual Stream GetXslt()
        {
            CheckIfInitialised();

            using (ICompressedConfigFile file = new ZipCompressedConfigFile(_configFile))
            {
                string xslt = _xmlDocument.SelectSingleNode("/export/xslt").InnerText;
                return file.GetEntry(xslt);
            }
        }

        /// <summary>
        /// If the file <see cref="HasScreenshot"/> then this method returns that
        /// screen shot as a Bitmap file.
        /// </summary>
        /// <returns>The Bitmap</returns>
        public Stream GetScreenshot()
        {
            CheckIfInitialised();

            using (ICompressedConfigFile file = new ZipCompressedConfigFile(_configFile))
            {
                string xslt = _xmlDocument.SelectSingleNode("/export/screenshot").InnerText;
                return file.GetEntry(xslt);
            }
        }

        /// <summary>
        /// Saves all of the registered output files in the export configuration file
        /// to the specified location.
        /// </summary>
        /// <param name="location">The location.</param>
        public virtual void SaveOutputFilesTo(string location)
        {
            CheckIfInitialised();

            using (ICompressedConfigFile compressedFile = new ZipCompressedConfigFile(_configFile))
            {
                XmlNodeList files = _xmlDocument.SelectNodes("export/outputfiles/file");
                foreach (XmlNode current in files)
                {
                    string from = current.Attributes["internal"] == null ? string.Empty : current.Attributes["internal"].Value;
                    string to = current.Attributes["output"] == null ? string.Empty : current.Attributes["output"].Value;

                    if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to))
                    {
                        continue;
                    }

                    if (compressedFile.HasEntry(from))
                    {
                        compressedFile.ExtractEntry(from, location);
                    }
                }
            }
        }

        public virtual List<string> GetOutputFileURLs()
        {
            CheckIfInitialised();

            List<string> urls = new List<string>();

            using (ICompressedConfigFile compressedFile = new ZipCompressedConfigFile(_configFile))
            {
                XmlNodeList files = _xmlDocument.SelectNodes("/export/outputfiles/file");
                foreach (XmlNode current in files)
                {
                    string from = current.Attributes["internal"] == null ? string.Empty : current.Attributes["internal"].Value;
                    string to = current.Attributes["output"] == null ? string.Empty : current.Attributes["output"].Value;

                    if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to))
                    {
                        continue;
                    }

                    if (compressedFile.HasEntry(from))
                    {
                        CompressedFileEntry details = compressedFile.GetEntryDetails(from);
                        if (details.IsDirectory)
                        {
                            urls.Add(details.FileName + "*.*");
                        }
                        else
                        {
                            urls.Add(details.FileName);
                        }
                    }
                }
            }

            return urls;
        }

        private void CheckIfInitialised()
        {
            if (!_isInitialised) throw new InvalidOperationException("ExportConfigFile must be initialised first with a call to Initialise().");
        }

        private Exporters UnpackExporter(XmlNode value)
        {
            string content = string.Empty;
            if (value != null) content = value.InnerText;

            switch (content.ToLower())
            {
                case "web":
                    return Exporters.Website;
                case "html1":
                    return Exporters.Html1;
                case "html2":
                    return Exporters.Html2;
                case "helpviewer1":
                    return Exporters.HelpViewer1;
                case "xml":
                    return Exporters.XML;
                default:
                    return Exporters.Website;
            }
        }
        
        /// <summary>
        /// Checks if the file has all of the requisits met and sets the <see cref="IsValid"/> property.
        /// </summary>
        private void CheckIsValid(ICompressedConfigFile container)
        {
            // we need to have a config file
            this.IsValid = _xmlDocument != null;

            // the xml config xml needs to have an xslt specified
            if (this.Exporter != Exporters.XML)
            {
                XmlNode xsltNode = _xmlDocument.SelectSingleNode("/export/xslt");
                this.IsValid = this.IsValid && (xsltNode != null && !string.IsNullOrEmpty(xsltNode.InnerText));

                // does the xslt link point to a file in the zip
                this.IsValid = this.IsValid && container.HasEntry(xsltNode.InnerText);
            }

            this.IsValid = this.IsValid && !string.IsNullOrEmpty(this.Name);
        }

        private void ReadConfigurationDetails()
        {
            XmlNode nameNode = _xmlDocument.SelectSingleNode("/export/name");
            if (nameNode != null)
                _name = nameNode.InnerText;

            XmlNode versionNode = _xmlDocument.SelectSingleNode("/export/version");
            if (versionNode != null)
                _version = versionNode.InnerText;

            _exporters = this.UnpackExporter(_xmlDocument.SelectSingleNode("/export/exporter"));

            XmlNode descriptionNode = _xmlDocument.SelectSingleNode("/export/description");
            if (descriptionNode != null)
            {
                _description = descriptionNode.InnerText;
            }

            XmlNode screenshotNode = _xmlDocument.SelectSingleNode("/export/screenshot");
            if (screenshotNode != null)
            {
                _hasScreenshot = true;
            }

            XmlNodeList properties = _xmlDocument.SelectNodes("/export/properties/property");
            foreach (XmlNode currentProperty in properties)
            {
                _properties.Add(currentProperty.Attributes["name"].Value, currentProperty.Attributes["value"].Value);
            }
        }

        /// <summary>
        /// The full filename and path of the config file
        /// </summary>
        /// <value>The conig file.</value>
        protected string ConfigFile
        {
            get { return _configFile; }
            set { _configFile = value; }
        }

        /// <summary>
        /// Gets the display name of this export configuration.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// The exporter to be used as part of the export for this export configuration.
        /// </summary>
        /// <value>The exporter.</value>
        public Exporters Exporter
        {
            get { return _exporters; }
            set { _exporters = value; }
        }

        /// <summary>
        /// A description of the exporter.
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// The version number of the export config file.
        /// </summary>
        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }

        /// <summary>
        /// Indicates if the file contains a screenshot.
        /// </summary>
        public bool HasScreenshot
        {
            get { return _hasScreenshot; }
            set { _hasScreenshot = value; }
        }

        /// <summary>
        /// The custom properties defined in the config file.
        /// </summary>
        public Dictionary<string, string> Properties
        {
            get { return _properties; }
            set { _properties = value; }
        }

        /// <summary>
        /// Indicates if this is a valid LDEC file.
        /// </summary>
        public bool IsValid
        {
            get { return _isValid; }
            set { _isValid = value; }
        }
    }
}