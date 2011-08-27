using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheBoxSoftware.Reflection.Comments;
using TheBoxSoftware.Reflection;

namespace TheBoxSoftware.Documentation.Exporting.Rendering {
	/// <summary>
	/// Renders the XML for a method in the documentation.
	/// </summary>
	internal class MethodXmlRenderer : XmlRenderer {
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

		/// <summary>
		/// Renders the method XML to the <paramref name="writer."/>
		/// </summary>
		/// <param name="writer">The open valid writer to write to.</param>
		public override void Render(System.Xml.XmlWriter writer) {
			CRefPath crefPath = new CRefPath(this.member);
			XmlCodeComment comment = this.xmlComments.ReadComment(crefPath);
			string displayName = this.member.GetDisplayName(false);

			writer.WriteStartElement("member");
			writer.WriteAttributeString("id", this.AssociatedEntry.Key.ToString());
			writer.WriteAttributeString("subId", this.AssociatedEntry.SubKey);
			writer.WriteAttributeString("type", ReflectionHelper.GetType(this.member));
			writer.WriteStartElement("name");
			writer.WriteAttributeString("safename", Exporter.CreateSafeName(displayName));
			writer.WriteString(this.member.GetDisplayName(false));
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

			if (this.member.IsGeneric) {
				this.RenderGenericTypeParameters(this.member.GetGenericTypes(), writer, comment);
			}

			if (this.member.Parameters.Count > 0) {
				writer.WriteStartElement("parameters");
				for (int i = 0; i < this.member.Parameters.Count; i++) {
					if (this.member.Parameters[i].Sequence == 0)
						continue;

					TypeRef parameterType = this.member.Parameters[i].GetTypeRef();
					Entry foundEntry = this.AssociatedEntry.FindByKey(parameterType.UniqueId, string.Empty);

					writer.WriteStartElement("parameter");
					writer.WriteAttributeString("name", this.member.Parameters[i].Name);
					writer.WriteStartElement("type");
					if (!parameterType.IsExternalReference) {
						writer.WriteAttributeString("key", parameterType.GetGloballyUniqueId().ToString());
					}
					writer.WriteString(parameterType.GetDisplayName(false));
					writer.WriteEndElement(); // type

					if (comment != XmlCodeComment.Empty) {
						XmlCodeElement paramEntry = comment.Elements.Find(currentBlock =>
							currentBlock is ParamXmlCodeElement
							&& ((ParamXmlCodeElement)currentBlock).Name == this.member.Parameters[i].Name);
						if (paramEntry != null) {
							writer.WriteStartElement("description");
							ParamXmlCodeElement paraElement = (ParamXmlCodeElement)paramEntry;
							for (int j = 0; j < paraElement.Elements.Count; j++) {
								this.Serialize(paraElement.Elements[j], writer, this.member.Assembly);
							}
							writer.WriteEndElement();
						}
					}

					writer.WriteEndElement(); // parameter
				}
				writer.WriteEndElement();
			}

			this.RenderExceptionBlock(this.member, writer, comment);

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

			// find and output the examples
			if (comment != XmlCodeComment.Empty) {
				XmlCodeElement remarks = comment.Elements.Find(currentBlock => currentBlock is ExampleXmlCodeElement);
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
