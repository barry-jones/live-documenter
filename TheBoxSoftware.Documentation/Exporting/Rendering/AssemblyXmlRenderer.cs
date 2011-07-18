using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Documentation.Exporting.Rendering {
	using TheBoxSoftware.Reflection;
	using TheBoxSoftware.Reflection.Comments;

	internal class AssemblyXmlRenderer : XmlRenderer {
		private AssemblyDef member;
		private XmlCodeCommentFile xmlComments;

		/// <summary>
		/// Initializes a new instance of the <see cref="NamespaceXmlRenderer"/> class.
		/// </summary>
		/// <param name="entry">The associated entry.</param>
		public AssemblyXmlRenderer(Entry entry) {
			this.member = (AssemblyDef)entry.Item;
			this.xmlComments = entry.XmlCommentFile;
			this.AssociatedEntry = entry;
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

				// at the moment there is no xml comments for namespaces
				//XmlCodeComment comment = this.xmlComments.ReadComment(new CRefPath((TypeDef)current.Item));
				//if (comment != null && comment.Elements != null) {
				//    Reflection.Comments.SummaryXmlCodeElement summary = comment.Elements.First(p => p is Reflection.Comments.SummaryXmlCodeElement) as Reflection.Comments.SummaryXmlCodeElement;
				//    if (summary != null) {
				//        this.Serialize(summary, writer, this.member);
				//    }
				//}

				writer.WriteEndElement();
			}

			writer.WriteEndElement();
		}
	}
}
