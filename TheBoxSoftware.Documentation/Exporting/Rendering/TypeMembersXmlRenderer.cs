using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheBoxSoftware.Reflection;
using TheBoxSoftware.Reflection.Comments;

namespace TheBoxSoftware.Documentation.Exporting.Rendering {
	internal sealed class TypeMembersXmlRenderer : XmlRenderer {
		private TypeDef containingType = null;
		private XmlCodeCommentFile xmlComments;

		/// <summary>
		/// Initializes a new instance of the <see cref="TypeMembersXmlRenderer"/> class.
		/// </summary>
		/// <param name="associatedEntry">The associated entry.</param>
		public TypeMembersXmlRenderer(Entry entry) {
			this.containingType = (TypeDef)entry.Parent.Item;
			this.xmlComments = entry.XmlCommentFile;
			this.AssociatedEntry = entry;
		}

		public override void Render(System.Xml.XmlWriter writer) {
			writer.WriteStartDocument();

			writer.WriteStartElement("members");
			writer.WriteAttributeString("id", this.AssociatedEntry.Key.ToString());
			writer.WriteAttributeString("subId", this.AssociatedEntry.SubKey);

			writer.WriteStartElement("name");
			writer.WriteString(string.Format("{0} {1}", this.containingType.GetDisplayName(false), this.AssociatedEntry.Name));
			writer.WriteEndElement();

			// we need to write the entries that appear as children to this document map
			// entry. it is going to be easier to use the Entry elements
			writer.WriteStartElement("entries");
			foreach (Entry current in this.AssociatedEntry.Children) {
				ReflectedMember m = current.Item as ReflectedMember;
				CRefPath crefPath = CRefPath.Create(m);
				XmlCodeComment comment = this.xmlComments.ReadComment(crefPath);

				writer.WriteStartElement("entry");
				writer.WriteAttributeString("id", current.Key.ToString());
				writer.WriteAttributeString("subId", current.SubKey);
				writer.WriteAttributeString("type", ReflectionHelper.GetType(m));
				writer.WriteAttributeString("visibility", ReflectionHelper.GetVisibility(m));

				writer.WriteStartElement("name");
				writer.WriteString(ReflectionHelper.GetDisplayName(m));
				writer.WriteEndElement();

				// find and output the summary
				if (comment != XmlCodeComment.Empty) {
					XmlCodeElement summary = comment.Elements.Find(currentBlock => currentBlock is SummaryXmlCodeElement);
					if (summary != null) {
						this.Serialize(summary, writer, this.containingType.Assembly);
					}
				}

				writer.WriteEndElement();
			}
			writer.WriteEndElement();

			writer.WriteEndElement();
			writer.WriteEndDocument();
		}
	}
}
