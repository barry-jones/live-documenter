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
			this.VisibilityFilters = new List<Reflection.Visibility>();
			this.RemovedAssemblies = new List<string>();
		}

		/// <summary>
		/// Collection of filenames for all the files associated with this project
		/// </summary>
		[XmlArray("files")]
		[XmlArrayItem("file")]
		public List<string> Files { get; set; }

		/// <summary>
		/// The list of assemblies that the user has marked as not requiring documentation.
		/// </summary>
		public List<string> RemovedAssemblies { get; set; }

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
		/// Obtains all of the DocumentedAssembly references for assemblies that are valid
		/// for the current configuration.
		/// </summary>
		/// <returns></returns>
		/// <remarks>
		/// <para>The current configuration is made from the list of <see cref="Files"/>, the
		/// <see cref="Configuration"/> and the <see cref="RemovedAssemblies"/>.</para>
		/// </remarks>
		public List<DocumentedAssembly> GetAssemblies() {
			List<DocumentedAssembly> assemblies = new List<DocumentedAssembly>();

			foreach (string file in this.Files) {
				List<DocumentedAssembly> readFiles = InputFileReader.Read(file, this.Configuration);
				for (int i = 0; i < readFiles.Count; i++) {
					if (!this.RemovedAssemblies.Any(current => current == string.Format("{0}\\{1}", System.IO.Path.GetFileName(file), readFiles[i].Name))) {
						assemblies.Add(readFiles[i]);
					}
				}
			}

			assemblies.Sort((a, b) => a.Name.CompareTo(b.Name));
			for (int i = 0; i < assemblies.Count; i++) {
				assemblies[i].UniqueId = i;
			}

			return assemblies;
		}

		public void AddFiles(string[] files) {
			for (int i = 0; i < files.Length; i++) {
				if (!this.Files.Any(current => current == files[i])) {
					this.Files.Add(files[i]);
				}
			}
		}

		/// <summary>
		/// Tests the <see cref="Files"/> and returns a list of files that could not
		/// be located.
		/// </summary>
		/// <returns>An array of missing files as complete file paths.</returns>
		public string[] GetMissingFiles() {
			List<string> missingFiles = new List<string>();
			for(int i = 0; i < this.Files.Count; i++) {
				if(!System.IO.File.Exists(this.Files[i])) {
					missingFiles.Add(this.Files[i]);
				}
			}
			return missingFiles.ToArray();
		}

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
		/// Deserializes a Project from the <paramref name="fromFile"/>.
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
