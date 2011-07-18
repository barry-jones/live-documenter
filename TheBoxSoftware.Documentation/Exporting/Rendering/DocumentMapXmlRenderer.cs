using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheBoxSoftware.Reflection.Comments;
using TheBoxSoftware.Reflection;

namespace TheBoxSoftware.Documentation.Exporting.Rendering {
	class DocumentMapXmlRenderer : XmlRenderer {
		private DocumentMap documentMap;
		
		/// <summary>
		/// Initialises a new LiveDocument
		/// </summary>
		/// <param name="files">The files to be managed by this LiveDocument.</param>
		public DocumentMapXmlRenderer(DocumentMap documentMap) {
			this.documentMap = documentMap;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="writer"></param>
		public override void Render(System.Xml.XmlWriter writer) {
			writer.WriteStartElement("toc");

			foreach (Entry current in this.documentMap) {
				writer.WriteStartElement("parent");
				writer.WriteAttributeString("name", current.Name);
				writer.WriteAttributeString("key", current.Key.ToString());
				writer.WriteAttributeString("subkey", current.SubKey);

				foreach (Entry child in current.Children) {
					this.Render(child, writer);
				}

				writer.WriteEndElement();
			}

			writer.WriteEndElement();
		}

		private void Render(Entry entry, System.Xml.XmlWriter writer) {
			writer.WriteStartElement("item");
			writer.WriteAttributeString("name", entry.Name);
			writer.WriteAttributeString("key", entry.Key .ToString());
			writer.WriteAttributeString("subkey", entry.SubKey);

			foreach (Entry child in entry.Children) {
				this.Render(child, writer);
			}

			writer.WriteEndElement();
		}
	}
}
