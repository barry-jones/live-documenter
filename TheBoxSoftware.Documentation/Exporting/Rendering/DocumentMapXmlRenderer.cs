using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheBoxSoftware.Reflection.Comments;
using TheBoxSoftware.Reflection;

namespace TheBoxSoftware.Documentation.Exporting.Rendering {
	/// <summary>
	/// Renders the <see cref="DocumentMap"/> to XML for exporting clients.
	/// </summary>
	internal class DocumentMapXmlRenderer : XmlRenderer {
		private DocumentMap documentMap;
		
		/// <summary>
		/// Initialises a new LiveDocument
		/// </summary>
		/// <param name="files">The files to be managed by this LiveDocument.</param>
		public DocumentMapXmlRenderer(DocumentMap documentMap) {
			this.documentMap = documentMap;
		}

		/// <summary>
		/// Renders the DocumentMap to the provided <paramref name="writer"/>.
		/// </summary>
		/// <param name="writer">The XML writer.</param>
		public override void Render(System.Xml.XmlWriter writer) {
			writer.WriteStartElement("toc");

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

			writer.WriteEndElement();
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
			writer.WriteAttributeString("key", entry.Key .ToString());
			writer.WriteAttributeString("subkey", entry.SubKey);

			foreach (Entry child in entry.Children) {
				this.Render(child, writer);
			}

			writer.WriteEndElement();
		}
	}
}
