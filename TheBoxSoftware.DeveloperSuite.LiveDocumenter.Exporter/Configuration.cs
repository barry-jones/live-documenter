using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace TheBoxSoftware.Exporter {

	/// <summary>
	/// Class that describes which exporters to run and how those exporters will 
	/// execute.
	/// </summary>
	[Serializable]
	[XmlRoot("configuration")]
	internal class Configuration {
		[XmlElement("document")]
		public string Document { get; set; }

		[XmlArray("filters")]
		[XmlArrayItem("filter")]
		public List<Reflection.Visibility> Filters { get; set; }

		[XmlArray("outputs")]
		[XmlArrayItem("ldec")]
		public List<Output> Outputs { get; set; }

		public class Output {
			[XmlAttribute("location")]
			public string Location { get; set; }

			[XmlText()]
			public string File { get; set; }
		}

		#region Serialization
		/// <summary>
		/// Deserializes a Configuration from the <paramref name="formFile"/>.
		/// </summary>
		/// <param name="fromFile">The file to read the project from.</param>
		/// <returns>The instantiated project.</returns>
		public static Configuration Deserialize(string fromFile) {
			using (FileStream fs = new FileStream(fromFile, FileMode.Open)) {
				XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
				return (Configuration)serializer.Deserialize(fs);
			}
		}
		#endregion

		/// <summary>
		/// Checks if the configuration is valid and if not outputs the issues to the console and returns
		/// false.
		/// </summary>
		/// <returns></returns>
		public bool IsValid() {
			bool isValid = true;
			
			if (!File.Exists(this.Document)) {
				Console.WriteLine("  The document '{0}' does not exist.", this.Document);
				isValid = false;
			}

			return isValid;
		}
	}
}
