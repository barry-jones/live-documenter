using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheBoxSoftware.Reflection;
using TheBoxSoftware.Reflection.Comments;

namespace TheBoxSoftware.Documentation.Exporting.Rendering {
	class FieldXmlRenderer : XmlRenderer {
		private FieldDef member;
		private XmlCodeCommentFile xmlComments;

		/// <summary>
		/// Initializes a new instance of the <see cref="FieldXmlRenderer"/> class.
		/// </summary>
		/// <param name="entry">The entry to initialise the renderer with.</param>
		public FieldXmlRenderer(Entry entry) {
			this.member = (FieldDef)entry.Item;
			this.xmlComments = entry.XmlCommentFile;
			this.AssociatedEntry = entry;
		}

		public override void Render(System.Xml.XmlWriter writer) {
			CRefPath crefPath = new CRefPath(this.member);
			XmlCodeComment comment = this.xmlComments.ReadComment(crefPath);

			writer.WriteStartElement("member");
			writer.WriteAttributeString("id", this.AssociatedEntry.Key.ToString());
			writer.WriteAttributeString("subId", this.AssociatedEntry.SubKey);
			writer.WriteAttributeString("type", ReflectionHelper.GetType(this.member));
			writer.WriteStartElement("name");
			writer.WriteAttributeString("safename", Exporter.CreateSafeName(this.member.Name));
			writer.WriteString(this.member.Name);
			writer.WriteEndElement();

			writer.WriteStartElement("namespace");
			Entry namespaceEntry = this.AssociatedEntry.FindNamespace(this.member.Type.Namespace);
			writer.WriteAttributeString("id", namespaceEntry.Key.ToString());
			writer.WriteAttributeString("name", namespaceEntry.SubKey);
			writer.WriteString(this.member.Type.Namespace);
			writer.WriteEndElement();
			writer.WriteStartElement("assembly");
			writer.WriteAttributeString("file", System.IO.Path.GetFileName(this.member.Assembly.File.FileName));
			writer.WriteString(this.member.Assembly.Name);
			writer.WriteEndElement();

			// find and output the summary
			if (comment != XmlCodeComment.Empty) {
				XmlCodeElement summary = comment.Elements.Find(currentBlock => currentBlock is SummaryXmlCodeElement);
				if (summary != null) {
					this.Serialize(summary, writer);
				}
			}

			this.RenderPermissionBlock(this.member, writer, comment);
			this.RenderSyntaxBlocks(this.member, writer);

			// find and output the value
			if (comment != XmlCodeComment.Empty) {
				XmlCodeElement remarks = comment.Elements.Find(currentBlock => currentBlock is ValueXmlCodeElement);
				if (remarks != null) {
					this.Serialize(remarks, writer);
				}
			}

			// find and output the summary
			if (comment != XmlCodeComment.Empty) {
				XmlCodeElement remarks = comment.Elements.Find(currentBlock => currentBlock is RemarksXmlCodeElement);
				if (remarks != null) {
					this.Serialize(remarks, writer);
				}
			}

			// find and output the examples
			if (comment != XmlCodeComment.Empty) {
				XmlCodeElement remarks = comment.Elements.Find(currentBlock => currentBlock is ExampleXmlCodeElement);
				if (remarks != null) {
					this.Serialize(remarks, writer);
				}
			}

			this.RenderSeeAlsoBlock(member, writer, comment);

			writer.WriteEndElement();
		}
	}
}
