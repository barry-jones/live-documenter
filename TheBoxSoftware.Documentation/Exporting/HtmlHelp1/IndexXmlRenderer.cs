using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TheBoxSoftware.Documentation.Exporting.HtmlHelp1 {
	/// <summary>
	/// A <see cref="XmlRenderer"/> which renders the XML for the html index file.
	/// </summary>
	internal sealed class IndexXmlRenderer : Rendering.XmlRenderer {
		private List<Entry> documentMap = null;

		/// <summary>
		/// Initializes a new instance of the <see cref="IndexXmlRenderer"/> class.
		/// </summary>
		/// <param name="documentMap">The document map.</param>
		public IndexXmlRenderer(List<Entry> documentMap) {
			this.documentMap = documentMap;
		}

		public override void Render(XmlWriter writer) {
			writer.WriteStartDocument();
			writer.WriteStartElement("index");

			writer.WriteEndElement(); // project
			writer.WriteEndDocument();
		}
	}
}
