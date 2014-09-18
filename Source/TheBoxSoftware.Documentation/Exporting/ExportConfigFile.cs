using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using Ionic.Zip;

namespace TheBoxSoftware.Documentation.Exporting {
	/// <summary>
	/// Reads a file that contains all of the information needed to perform an export.
	/// </summary>
	/// <remarks>
	/// This file is a zip file which contains the following files:
	/// <list type="">
	///		<item>export.config [required] describes the main details of the export</item>
	/// </list>
	/// </remarks>
	public class ExportConfigFile {
		private XmlDocument xmlDocument;

		/// <summary>
		/// Initializes a new instance of the <see cref="ExportConfigFile"/> class.
		/// </summary>
		/// <param name="file">The file.</param>
		private ExportConfigFile(string filename) {
			this.Properties = new Dictionary<string, string>();
			this.ConfigFile = filename;
			using (ZipFile file = new ZipFile(filename)) {
				// get the config file
				this.xmlDocument = null;
				Stream ms = new MemoryStream();
				if (file.ContainsEntry("export.config")) {
					file["export.config"].Extract(ms);
					ms.Seek(0, SeekOrigin.Begin);
					xmlDocument = new XmlDocument();
					xmlDocument.LoadXml(new StreamReader(ms).ReadToEnd());
					ms.Close();

                    XmlNode nameNode = xmlDocument.SelectSingleNode("/export/name");
                    if(nameNode != null)
					    this.Name = nameNode.InnerText;
                    XmlNode versionNode = xmlDocument.SelectSingleNode("/export/version");
                    if(versionNode != null)
					    this.Version = versionNode.InnerText;
					this.Exporter = this.UnpackExporter(xmlDocument.SelectSingleNode("/export/exporter"));
					XmlNode descriptionNode = xmlDocument.SelectSingleNode("/export/description");
					if (descriptionNode != null) {
						this.Description = descriptionNode.InnerText;
					}
					XmlNode screenshotNode = xmlDocument.SelectSingleNode("/export/screenshot");
					if (screenshotNode != null) {
						this.HasScreenshot = true;
					}

					XmlNodeList properties = xmlDocument.SelectNodes("/export/properties/property");
					foreach (XmlNode currentProperty in properties) {
						this.Properties.Add(currentProperty.Attributes["name"].Value, currentProperty.Attributes["value"].Value);
					}					
				}

				this.CheckIsValid(file);
			}
		}

		/// <summary>
		/// The full filename and path of the config file
		/// </summary>
		/// <value>The conig file.</value>
		protected string ConfigFile { get; set; }

		/// <summary>
		/// Gets the display name of this export configuration.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; set; }

		/// <summary>
		/// The exporter to be used as part of the export for this export configuration.
		/// </summary>
		/// <value>The exporter.</value>
		public Exporters Exporter { get; set; }

		/// <summary>
		/// A description of the exporter.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// The version number of the export config file.
		/// </summary>
		public string Version { get; set; }
		
		/// <summary>
		/// Indicates if the file contains a screenshot.
		/// </summary>
		public bool HasScreenshot { get; set; }

		/// <summary>
		/// The custom properties defined in the config file.
		/// </summary>
		public Dictionary<string, string> Properties { get; set; }

		/// <summary>
		/// Indicates if this is a valid LDEC file.
		/// </summary>
		public bool IsValid { get; set; }

		/// <summary>
		/// Gets the XSLT file from the export configuration file.
		/// </summary>
		/// <returns></returns>
		public virtual Stream GetXslt() {
			using (ZipFile file = new ZipFile(this.ConfigFile)) {
				string xslt = xmlDocument.SelectSingleNode("/export/xslt").InnerText;
				MemoryStream xsltStream = new MemoryStream();
				file[xslt].Extract(xsltStream);
				xsltStream.Seek(0, SeekOrigin.Begin);
				return xsltStream;
			}
		}

		/// <summary>
		/// If the file <see cref="HasScreenshot"/> then this method returns that
		/// screen shot as a Bitmap file.
		/// </summary>
		/// <returns>The Bitmap</returns>
		public Stream GetScreenshot() {
			using (ZipFile file = new ZipFile(this.ConfigFile)) {
				string filename = xmlDocument.SelectSingleNode("/export/screenshot").InnerText;
				MemoryStream imageStream = new MemoryStream();
				file[filename].Extract(imageStream);
				imageStream.Seek(0, SeekOrigin.Begin);
				return imageStream;
			}
		}

		/// <summary>
		/// Saves all of the registered output files in the export configuration file
		/// to the specified location.
		/// </summary>
		/// <param name="location">The location.</param>
		public virtual void SaveOutputFilesTo(string location) {
			using (ZipFile file = new ZipFile(this.ConfigFile)) {
				XmlNodeList files = this.xmlDocument.SelectNodes("export/outputfiles/file");
				foreach (XmlNode current in files) {
					string from = current.Attributes["internal"] == null ? string.Empty : current.Attributes["internal"].Value;
					string to = current.Attributes["output"] == null ? string.Empty : current.Attributes["output"].Value;

					if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to)) {
						continue;
					}

					if (file[from] == null) {
						continue;
					}

					if (file[from].IsDirectory) {
						file.ExtractSelectedEntries("name = *.*", file[from].FileName, location, ExtractExistingFileAction.OverwriteSilently);
					}
					else {
						file[from].Extract(location);
					}
				}
			}
		}

		public virtual List<string> GetOutputFileURLs() {
			List<string> urls = new List<string>();

			using (ZipFile file = new ZipFile(this.ConfigFile)) {
				XmlNodeList files = this.xmlDocument.SelectNodes("/export/outputfiles/file");
				foreach (XmlNode current in files) {
					string from = current.Attributes["internal"] == null ? string.Empty : current.Attributes["internal"].Value;
					string to = current.Attributes["output"] == null ? string.Empty : current.Attributes["output"].Value;

					if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to)) {
						continue;
					}

					if (file[from] == null) {
						continue;
					}

					if (file[from].IsDirectory) {
						urls.Add(file[from].FileName + "*.*");
					}
					else {
						urls.Add(file[from].FileName);
					}
				}
			}

			return urls;
		}

		private Exporters UnpackExporter(XmlNode value) {
            string content = string.Empty;
            if (value != null) content = value.InnerText;

			switch (content.ToLower()) {
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

		public static ExportConfigFile Create(string filename) {
			return new ExportConfigFile(filename);
		}

		#region Internal Methods
		/// <summary>
		/// Checks if the file has all of the requisits met and sets the <see cref="IsValid"/> property.
		/// </summary>
		private void CheckIsValid(ZipFile container) {
			// we need to have a config file
			this.IsValid = this.xmlDocument != null;

			// the xml config xml needs to have an xslt specified
			if (this.Exporter != Exporters.XML) {
				XmlNode xsltNode = this.xmlDocument.SelectSingleNode("/export/xslt");
				this.IsValid = this.IsValid && (xsltNode != null && !string.IsNullOrEmpty(xsltNode.InnerText));

				// does the xslt link point to a file in the zip
				this.IsValid = this.IsValid && container.ContainsEntry(xsltNode.InnerText);
			}

            this.IsValid = this.IsValid && !string.IsNullOrEmpty(this.Name);
		}
		#endregion
	}
}
