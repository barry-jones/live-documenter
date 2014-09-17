using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Documentation.Exporting.Rendering {
	using TheBoxSoftware.Reflection;
	using TheBoxSoftware.Reflection.Comments;

	/// <summary>
	/// Renders an <see cref="AssemblyDef"/> via a <see cref="DocumentMap"/> in XML.
	/// </summary>
	internal class AssemblyXmlRenderer : XmlRenderer {
		private AssemblyDef member;
		private XmlCodeCommentFile xmlComments;

		/// <summary>
		/// Initializes a new instance of the <see cref="NamespaceXmlRenderer"/> class.
		/// </summary>
		/// <param name="entry">The associated entry.</param>
		/// <exception cref="InvalidOperationException">Thrown when an Entry with an invalid Item is provided.</exception>
		public AssemblyXmlRenderer(Entry entry)
        {
			this.member = entry.Item as AssemblyDef;
			this.xmlComments = entry.XmlCommentFile;
			this.AssociatedEntry = entry;

			if (member == null) {
				throw new InvalidOperationException(
					string.Format("Entry in DocumentMap is being exported as AssemblyDef when type is '{0}'", entry.Item.GetType())
					);
			}
		}

		/// <summary>
		/// Renders the XML for the namespace to the specified <paramref name="writer"/>.
		/// </summary>
		/// <param name="writer">The writer.</param>
		public override void Render(System.Xml.XmlWriter writer) {
			writer.WriteStartElement("assembly");
			writer.WriteAttributeString("id", this.AssociatedEntry.Key.ToString());

			writer.WriteStartElement("name");
			writer.WriteString(string.Format("{0} Assembly", this.member.Name));
			writer.WriteEndElement();

			foreach (Entry current in this.AssociatedEntry.Children) {
				writer.WriteStartElement("parent");
				writer.WriteAttributeString("name", current.Name);
				writer.WriteAttributeString("key", current.Key.ToString());
				writer.WriteAttributeString("type", "namespace");
                if(this.IncludeCRefPath)
                    writer.WriteAttributeString("cref", string.Format("N:{0}", current.Name));
				writer.WriteEndElement(); // parent
			}

			writer.WriteEndElement(); // assembly
		}
	}
}
