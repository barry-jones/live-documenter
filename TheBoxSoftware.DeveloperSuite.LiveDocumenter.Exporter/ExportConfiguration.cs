using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace TheBoxSoftware.Exporter {

	/// <summary>
	/// Class that describes which exporters to run and how those exporters will 
	/// execute.
	/// </summary>
	[Serializable]
	[XmlRoot("configuration")]
	public class Configuration {
		[XmlArrayItem("export")]
		public List<Export> Exporters { get; set; }

		public class Export {
			[XmlElement("ldec")]
			public string Exporter { get; set; }

			[XmlArray("filters")]
			[XmlArrayItem("filter")]
			public List<Reflection.Visibility> Filters { get; set; }
		}
	}
}
