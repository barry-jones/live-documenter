using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheBoxSoftware.Reflection.Comments;
using TheBoxSoftware.Reflection;

namespace TheBoxSoftware.Documentation.Exporting.Rendering {
	class MethodXmlRenderer : XmlRenderer {
		private MethodDef member;
		private XmlCodeCommentFile xmlComments;

		/// <summary>
		/// Initializes a new instance of the <see cref="MethodXmlRenderer"/> class.
		/// </summary>
		/// <param name="entry">The entry to initialise the renderer with.</param>
		public MethodXmlRenderer(Entry entry) {
			this.member = (MethodDef)entry.Item;
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
			writer.WriteString(this.member.GetDisplayName(false));
			writer.WriteEndElement();

			writer.WriteStartElement("namespace");
			writer.WriteAttributeString("id", this.Exporter.GetUniqueKey(this.member.Assembly).ToString());
			writer.WriteAttributeString("name", this.member.Type.Namespace);
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
					this.Serialize(summary, writer, this.member.Assembly);
				}
			}

			this.RenderSyntaxBlocks(this.member, writer);

			// find and output the summary
			if (comment != XmlCodeComment.Empty) {
				XmlCodeElement remarks = comment.Elements.Find(currentBlock => currentBlock is RemarksXmlCodeElement);
				if (remarks != null) {
					this.Serialize(remarks, writer, this.member.Assembly);
				}
			}

			// find and output the see also
			if (comment != XmlCodeComment.Empty) {
				XmlCodeElement remarks = comment.Elements.Find(currentBlock => currentBlock is SeeAlsoXmlCodeElement);
				if (remarks != null) {
					this.Serialize(remarks, writer, this.member.Assembly);
				}
			}

			this.RenderSeeAlsoBlock(member, writer, comment, this.member.Assembly);

			writer.WriteEndElement();
		}
	}
}
