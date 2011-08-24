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
			writer.WriteAttributeString("safename", Exporter.CreateSafeName(this.AssociatedEntry.Name));
			writer.WriteString(this.AssociatedEntry.Name);
			writer.WriteEndElement();

			// we need to write the entries that appear as children to this document map
			// entry. it is going to be easier to use the Entry elements
			writer.WriteStartElement("entries");
			switch (this.AssociatedEntry.SubKey.ToLower()) {
				case "properties":
				case "fields":
				case "constants":
				case "operators":
				case "constructors":
					foreach (Entry current in this.AssociatedEntry.Children) {
						ReflectedMember m = current.Item as ReflectedMember;
						this.WriteEntry(writer, m, ReflectionHelper.GetDisplayName(m));
					}
					break;

				case "methods":
					// write normal methods
					foreach (Entry current in this.AssociatedEntry.Children) {
						ReflectedMember m = current.Item as ReflectedMember;
						this.WriteEntry(writer, m, ReflectionHelper.GetDisplayName(m));
					}
					// write extension methods
					var extensionMethods = from method in ((TypeRef)this.AssociatedEntry.Parent.Item).ExtensionMethods orderby method.Name select method;
					foreach (MethodDef currentMethod in extensionMethods) {
						DisplayNameSignitureConvertor displayNameSig = new DisplayNameSignitureConvertor(currentMethod, false, true, true);
						this.WriteEntry(writer, currentMethod, currentMethod.GetDisplayName(false, true), "extensionmethod");
					}
					break;
			}
			writer.WriteEndElement(); // entries
			writer.WriteEndElement();
			writer.WriteEndDocument();
		}

		private void WriteEntry(System.Xml.XmlWriter writer, ReflectedMember entryMember, string displayName, string type) {
			CRefPath currentPath = CRefPath.Create(entryMember);
			XmlCodeComment currentComment = this.xmlComments.ReadComment(currentPath);

			writer.WriteStartElement("entry");
			writer.WriteAttributeString("id", entryMember.GetGloballyUniqueId().ToString());
			writer.WriteAttributeString("subId", string.Empty);
			writer.WriteAttributeString("type", type);
			writer.WriteAttributeString("visibility", ReflectionHelper.GetVisibility(entryMember));

			writer.WriteStartElement("name");
			writer.WriteString(displayName);
			writer.WriteEndElement();

			// find and output the summary
			if (currentComment != XmlCodeComment.Empty && currentComment.Elements != null) {
				XmlCodeElement summary = currentComment.Elements.Find(currentBlock => currentBlock is SummaryXmlCodeElement);
				if (summary != null) {
					this.Serialize(summary, writer, entryMember.Assembly);
				}
			}
			writer.WriteEndElement();
		}

		private void WriteEntry(System.Xml.XmlWriter writer, ReflectedMember entryMember, string displayName) {
			this.WriteEntry(writer, entryMember, displayName, ReflectionHelper.GetType(entryMember));
		}
	}
}
