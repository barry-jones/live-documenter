using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace TheBoxSoftware.Documentation {
	/// <summary>
	/// Represents the details and configuration of a documentation project.
	/// </summary>
	[Serializable]
	[XmlRoot("project")]
	public class Project {
		/// <summary>
		/// Initialises a new instance of the Project class.
		/// </summary>
		public Project() {
			this.Files = new List<string>();
		}

		/// <summary>
		/// Collection of filenames for all the files associated with this project
		/// </summary>
		[XmlArray("files")]
		[XmlArrayItem("file")]
		public List<string> Files { get; set; }

		/// <summary>
		/// Collection of filters that define what is and is not shown in this
		/// project.
		/// </summary>
		[XmlArray("visibilityfilters")]
		[XmlArrayItem("visibility")]
		public List<Reflection.Visibility> VisibilityFilters { get; set; }

		/// <summary>
		/// The currently selected build configuration.
		/// </summary>
		[XmlElement("configuration")]
		public string Configuration { get; set; }

		/// <summary>
		/// The selected syntax language for the document.
		/// </summary>
		[XmlElement("language")]
		public Reflection.Syntax.Languages Language { get; set; }

		/// <summary>
		/// The output location used for this documentation set
		/// </summary>
		[XmlElement("outputlocation")]
		public string OutputLocation { get; set; }

		/// <summary>
		/// Serializes the contents of this project to the <paramref name="toFile"/>.
		/// </summary>
		/// <param name="toFile">The file to replace or create.</param>
		public void Serialize(string toFile) {
			using(FileStream fs = new FileStream(toFile, FileMode.OpenOrCreate)) {
				fs.SetLength(0); // clean up all contents
				XmlSerializer serializer = new XmlSerializer(typeof(Project));
				serializer.Serialize(fs, this);
			}
		}

		/// <summary>
		/// Deserializes a Project from the <paramref name="formFile"/>.
		/// </summary>
		/// <param name="fromFile">The file to read the project from.</param>
		/// <returns>The instantiated project.</returns>
		public static Project Deserialize(string fromFile) {
			using(FileStream fs = new FileStream(fromFile, FileMode.Open)) {
				XmlSerializer serializer = new XmlSerializer(typeof(Project));
				return (Project)serializer.Deserialize(fs);
			}
		}
	}
}
