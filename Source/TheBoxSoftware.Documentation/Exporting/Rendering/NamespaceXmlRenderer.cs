using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheBoxSoftware.Reflection;
using TheBoxSoftware.Reflection.Comments;

namespace TheBoxSoftware.Documentation.Exporting.Rendering
{
	/// <summary>
	/// Renders XML for namespaces in the document map.
	/// </summary>
	internal class NamespaceXmlRenderer : XmlRenderer
    {
		private KeyValuePair<string, List<TypeDef>> member;
		private XmlCodeCommentFile xmlComments;

		/// <summary>
		/// Initializes a new instance of the <see cref="NamespaceXmlRenderer"/> class.
		/// </summary>
		/// <param name="entry">The associated entry.</param>
		public NamespaceXmlRenderer(Entry entry)
        {
			this.member = (KeyValuePair<string, List<TypeDef>>)entry.Item;
			this.xmlComments = entry.XmlCommentFile;
			this.AssociatedEntry = entry;
		}

		/// <summary>
		/// Renders the XML for the namespace to the specified <paramref name="writer"/>.
		/// </summary>
		/// <param name="writer">The writer.</param>
		public override void Render(System.Xml.XmlWriter writer)
        {
			writer.WriteStartElement("namespace");
			writer.WriteAttributeString("id", this.AssociatedEntry.Key.ToString());
			writer.WriteAttributeString("subId", this.AssociatedEntry.SubKey);
            this.WriteCref(this.AssociatedEntry, writer);

			writer.WriteStartElement("name");
			writer.WriteAttributeString("safename", Exporter.CreateSafeName(this.member.Key));
			writer.WriteString(string.Format("{0} Namespace", this.member.Key));
			writer.WriteEndElement();

			foreach (Entry current in this.AssociatedEntry.Children)
            {
				writer.WriteStartElement("parent");
				writer.WriteAttributeString("name", current.Name);
				writer.WriteAttributeString("key", current.Key.ToString());
				writer.WriteAttributeString("type", ReflectionHelper.GetType((TypeDef)current.Item));
				writer.WriteAttributeString("visibility", ReflectionHelper.GetVisibility(current.Item));
                this.WriteCref(current, writer);

				// write the summary text for the current member
				XmlCodeComment comment = this.xmlComments.ReadComment(new CRefPath((TypeDef)current.Item));
				if (comment != null && comment.Elements != null)
                {
					Reflection.Comments.SummaryXmlCodeElement summary = comment.Elements.Find(
						p => p is Reflection.Comments.SummaryXmlCodeElement
						) as Reflection.Comments.SummaryXmlCodeElement;
					if (summary != null) 
                    {
						this.Serialize(summary, writer);
					}
				}

				writer.WriteEndElement();
			}

			writer.WriteEndElement();
		}
	}
}
