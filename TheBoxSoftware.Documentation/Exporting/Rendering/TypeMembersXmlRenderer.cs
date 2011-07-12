using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheBoxSoftware.Reflection;
using TheBoxSoftware.Reflection.Comments;

namespace TheBoxSoftware.Documentation.Exporting.Rendering {
	internal sealed class TypeMembersXmlRenderer : XmlRenderer {
		private TypeDef containingType = null;
		private List<ReflectedMember> members;
		private XmlCodeCommentFile xmlComments;

		/// <summary>
		/// Initializes a new instance of the <see cref="TypeMembersXmlRenderer"/> class.
		/// </summary>
		/// <param name="container">The <see cref="TypeDef"/> that contains these members.</param>
		/// <param name="members">The members.</param>
		/// <param name="xmlComments">The XML comments.</param>
		/// <param name="associatedEntry">The associated entry.</param>
		public TypeMembersXmlRenderer(TypeDef container, List<ReflectedMember> members, XmlCodeCommentFile xmlComments, Entry associatedEntry) {
			this.containingType = container;
			this.members = members;
			this.xmlComments = xmlComments;
			this.AssociatedEntry = associatedEntry;
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

				writer.WriteStartElement("name");
				writer.WriteString(this.GetDisplayName(m));
				writer.WriteEndElement();

				// find and output the summary
				if (comment != XmlCodeComment.Empty) {
					XmlCodeElement summary = comment.Elements.Find(currentBlock => currentBlock is SummaryXmlCodeElement);
					if (summary != null) {
						this.Serialize(summary, writer);
					}
				}

				writer.WriteEndElement();
			}
			writer.WriteEndElement();

			writer.WriteEndElement();
			writer.WriteEndDocument();
		}

		/// <summary>
		/// Obtains a name to display for the reflected member.
		/// </summary>
		/// <param name="entry">The entry.</param>
		/// <returns>A name for the ReflectedMember</returns>
		private string GetDisplayName(ReflectedMember entry) {
			string displayName = string.Empty;

			if (entry is FieldDef) {
				displayName = ((FieldDef)entry).Name;
			}
			else if (entry is PropertyDef) {
				displayName = ((PropertyDef)entry).GetDisplayName(false);
			}
			else if (entry is MethodDef) {
				displayName = ((MethodDef)entry).GetDisplayName(false, true);
			}
			else if (entry is EventDef) {
				displayName = ((EventDef)entry).Name;
			}

			return displayName;
		}
	}
}
