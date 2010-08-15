using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheBoxSoftware.Reflection;
using TheBoxSoftware.Reflection.Comments;

namespace TheBoxSoftware.Documentation.Exporting.Rendering {
	/// <summary>
	/// Renders XML for namespaces in the document map.
	/// </summary>
	internal class NamespaceXmlRenderer : XmlRenderer {
		private KeyValuePair<string, List<TypeDef>> member;
		private XmlCodeCommentFile xmlComments;

		/// <summary>
		/// Initializes a new instance of the <see cref="NamespaceXmlRenderer"/> class.
		/// </summary>
		/// <param name="entry">The associated entry.</param>
		public NamespaceXmlRenderer(Entry entry) {
			this.member = (KeyValuePair<string, List<TypeDef>>)entry.Item;
			this.xmlComments = entry.XmlCommentFile;
			this.AssociatedEntry = entry;
		}

		/// <summary>
		/// Renders the XML for the namespace to the specified <paramref name="writer"/>.
		/// </summary>
		/// <param name="writer">The writer.</param>
		public override void Render(System.Xml.XmlWriter writer) {
			writer.WriteStartElement("namespace");
			writer.WriteAttributeString("id", this.AssociatedEntry.Key.ToString());
			writer.WriteAttributeString("subId", this.AssociatedEntry.SubKey);

			writer.WriteStartElement("name");
			writer.WriteString(string.Format("{0} Namespace", this.member.Key));
			writer.WriteEndElement();

			foreach (TypeDef current in this.member.Value) {
				writer.WriteStartElement("parent");
				writer.WriteAttributeString("name", current.GetDisplayName(false));
				writer.WriteAttributeString("key", current.UniqueId.ToString());
				writer.WriteAttributeString("type", this.GetTypeAsString(current));

				// write the summary text for the current member
				this.Serialize(this.xmlComments.ReadComment(new CRefPath(current)), writer);


				writer.WriteEndElement();
			}

			writer.WriteEndElement();
		}

		/// <summary>
		/// Returns a string that describes the current type.
		/// </summary>
		/// <param name="current">The current.</param>
		/// <returns>A string that details the type of TypeDef.</returns>
		/// <remarks>
		/// A <see cref="TypeDef"/> can be many things. This method checks the properties
		/// of the type and returns a string which describes the type. struct, class, interface
		/// etc.
		/// </remarks>
		private string GetTypeAsString(TypeDef current) {
			string actualType = "class";
			if (current.IsDelegate) {
				actualType = "delegate";
			}
			else if (current.IsEnumeration) {
				actualType = "enum";
			}
			else if (current.IsInterface) {
				actualType = "interface";
			}
			else if (current.IsStructure) {
				actualType = "struct";
			}
			return actualType;
		}
	}
}
