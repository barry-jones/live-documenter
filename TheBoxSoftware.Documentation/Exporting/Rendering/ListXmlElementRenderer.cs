using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Documentation.Exporting.Rendering
{
	using TheBoxSoftware.Reflection;
	using TheBoxSoftware.Reflection.Comments;

	/// <summary>
	/// Renders the ListXmlCodeElement as XML.
	/// </summary>
	internal class ListXmlElementRenderer : XmlElementRenderer {
		private ListXmlCodeElement element;

		/// <summary>
		/// Initialises a new instance of the ListXmlElementRenderer class.
		/// </summary>
		/// <param name="associatedEntry"></param>
		/// <param name="element"></param>
		public ListXmlElementRenderer(Entry associatedEntry, ListXmlCodeElement element) {
			this.AssociatedEntry = associatedEntry;
			this.element = element;
		}

		public override void Render(System.Xml.XmlWriter writer) {
			if (element.IsTable()) {
				this.RenderTable(writer);
			}
			else {
				this.RenderList(writer);
			}
		}

		private void RenderTable(System.Xml.XmlWriter writer) {
			writer.WriteStartElement("table");



			writer.WriteEndElement(); // table
		}

		private void RenderList(System.Xml.XmlWriter writer) {
			writer.WriteStartElement("list");
			// listtype can be bullet or number
			writer.WriteAttributeString("type", this.element.ListType.ToString().ToLower());

			List<XmlCodeElement> elements = this.element.Elements.FindAll(e => e.Element == XmlCodeElements.ListItem);
			foreach (ListItemXmlCodeElement item in elements) {
				writer.WriteStartElement("item");
				foreach (XmlCodeElement child in item.Elements) { // miss out the listitem and just focus on children
					this.Serialize(child, writer);
				}
				writer.WriteEndElement(); // item
			}

			writer.WriteEndElement(); // list
		}
	}
}
