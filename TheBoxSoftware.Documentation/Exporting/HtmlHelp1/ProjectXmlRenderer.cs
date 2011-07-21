using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TheBoxSoftware.Documentation.Exporting.HtmlHelp1 {
	/// <summary>
	/// A <see cref="Rendering.XmlRenderer"/> class that writes the XML for the
	/// HTML Help 1 project file.
	/// </summary>
	internal sealed class ProjectXmlRenderer : Rendering.XmlRenderer {
		private DocumentMap documentMap = null;

		/// <summary>
		/// Initializes a new instance of the <see cref="IndexXmlRenderer"/> class.
		/// </summary>
		/// <param name="documentMap">The document map.</param>
		public ProjectXmlRenderer(DocumentMap documentMap) {
			this.documentMap = documentMap;
		}

		public override void Render(XmlWriter writer) {
			writer.WriteStartDocument();
			writer.WriteStartElement("project");

			writer.WriteElementString("contentsfile", "toc.hhc");
			writer.WriteElementString("indexfile", "index.hhk");
			writer.WriteElementString("title", "Test");
			Entry firstEntry = this.documentMap.First();
			writer.WriteElementString("defaulttopic", string.Format("{0}-{1}.htm", firstEntry.Key, firstEntry.SubKey));

			writer.WriteEndElement(); // project
			writer.WriteEndDocument();
		}
	}
}
