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
		private DocumentMap documentMap = null;

		/// <summary>
		/// Initializes a new instance of the <see cref="IndexXmlRenderer"/> class.
		/// </summary>
		/// <param name="documentMap">The document map.</param>
		public IndexXmlRenderer(DocumentMap documentMap) {
			this.documentMap = documentMap;
		}

		public override void Render(XmlWriter writer) {
			writer.WriteStartDocument();
			writer.WriteStartElement("index");

			foreach (Entry current in this.documentMap) {
				writer.WriteStartElement("item");
				writer.WriteAttributeString("name", current.Name);
				writer.WriteAttributeString("safename", Exporter.CreateSafeName(current.Name));
				writer.WriteAttributeString("key", current.Key.ToString());
				writer.WriteAttributeString("subkey", current.SubKey);

				foreach (Entry child in current.Children) {
					this.Render(child, writer);
				}

				writer.WriteEndElement();
			}

			writer.WriteEndElement(); // index
			writer.WriteEndDocument();
		}

		/// <summary>
		/// Renders an individual element in the DocumentMap.
		/// </summary>
		/// <param name="entry">The entry to render.</param>
		/// <param name="writer">The writer to write to.</param>
		private void Render(Entry entry, System.Xml.XmlWriter writer) {
			writer.WriteStartElement("item");
			writer.WriteAttributeString("name", entry.Name);
			writer.WriteAttributeString("safename", Exporter.CreateSafeName(entry.Name));
			writer.WriteAttributeString("key", entry.Key.ToString());
			writer.WriteAttributeString("subkey", entry.SubKey);

			foreach (Entry child in entry.Children) {
				this.Render(child, writer);
			}

			writer.WriteEndElement();
		}
	}
}
