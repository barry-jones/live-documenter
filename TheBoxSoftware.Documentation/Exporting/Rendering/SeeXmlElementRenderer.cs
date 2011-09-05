using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Documentation.Exporting.Rendering
{
	using TheBoxSoftware.Reflection;
	using TheBoxSoftware.Reflection.Comments;

	/// <summary>
	/// Renders the XML for SeeXmlCodeElements.
	/// </summary>
	internal class SeeXmlElementRenderer : XmlElementRenderer {
		private SeeXmlCodeElement element;

		/// <summary>
		/// Initialises a new instance of the SeeXmlElementRenderer class.
		/// </summary>
		/// <param name="associatedEntry"></param>
		/// <param name="element"></param>
		public SeeXmlElementRenderer(Entry associatedEntry, SeeXmlCodeElement element) {
			this.AssociatedEntry = associatedEntry;
			this.element = element;
		}

		public override void Render(System.Xml.XmlWriter writer) {
			Entry entry = this.Exporter.Document.Find(this.element.Member);
			string displayName = string.IsNullOrEmpty(this.element.Member.ElementName)
				? this.element.Member.TypeName
				: this.element.Member.ElementName;

			if (this.element.Member.PathType != CRefTypes.Error) {
				writer.WriteStartElement("see");

				if(entry != null) {
					displayName = entry.Name;					
					writer.WriteAttributeString("id", entry.Key.ToString());

					switch (this.element.Member.PathType) {
						case CRefTypes.Namespace:
							writer.WriteAttributeString("type", "namespace");
							writer.WriteAttributeString("name", displayName);
							break;
						// these could be generic and so will need to modify to
						// a more appropriate display name
						case CRefTypes.Method:
							MethodDef method = entry.Item as MethodDef;
							if (method != null) {
								displayName = method.GetDisplayName(false);
							}
							break;
						case CRefTypes.Type:
							TypeDef def = entry.Item as TypeDef;
							if (def != null) {
								displayName = def.GetDisplayName(false);
							}
							break;
					}
				}

				writer.WriteString(displayName);
				writer.WriteEndElement();	// element
			}
		}
	}
}
