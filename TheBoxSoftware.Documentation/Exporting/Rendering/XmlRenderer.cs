using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheBoxSoftware.Reflection.Syntax;
using TheBoxSoftware.Reflection.Comments;
using TheBoxSoftware.Reflection;

namespace TheBoxSoftware.Documentation.Exporting.Rendering {
	internal abstract class XmlRenderer : IRenderer<System.Xml.XmlWriter> {
		protected Entry AssociatedEntry { get; set; }
		public Exporter Exporter { get; set; }

		public abstract void Render(System.Xml.XmlWriter writer);

		public static XmlRenderer Create(Entry associatedEntry, ReflectedMember member, XmlCodeCommentFile comments, Exporter exporter) {
			XmlRenderer renderer = null;

			// check sub key so we don't output members, fields etc nodes
			if (member is TypeDef && string.IsNullOrEmpty(associatedEntry.SubKey)) {
				renderer = new TypeXmlRenderer((TypeDef)member, comments, associatedEntry);
			}
			else if (member is MethodDef) {
				renderer = new MethodXmlRenderer((MethodDef)member, comments, associatedEntry);
			}
			else if (member is FieldDef) {
				renderer = new FieldXmlRenderer((FieldDef)member, comments, associatedEntry);
			}
			else if (member is PropertyDef) {
				renderer = new PropertyXmlRenderer((PropertyDef)member, comments, associatedEntry);
			}
			else if (member is EventDef) {
				renderer = new EventXmlRenderer((EventDef)member, comments, associatedEntry);
			}

			if (renderer != null) {
				renderer.Exporter = exporter;
			}
			
			return renderer;
		}

		public static XmlRenderer Create(Entry associatedEntry, TypeDef parent, List<ReflectedMember> members, XmlCodeCommentFile comments, Exporter exporter) {
			XmlRenderer renderer = null;

			renderer = new TypeMembersXmlRenderer(parent, (List<ReflectedMember>)members, comments, associatedEntry);

			if (renderer != null) {
				renderer.Exporter = exporter;
			}

			return renderer;
		}

		protected void RenderSyntaxBlocks(ReflectedMember member, System.Xml.XmlWriter writer) {
			IFormatter formatter = SyntaxFactory.Create(member, Languages.CSharp);
			if (formatter != null) {
				writer.WriteStartElement("syntaxblocks");
				writer.WriteStartElement("syntax");
				writer.WriteAttributeString("language", "C#");

				foreach (SyntaxToken token in formatter.Format()) {
					writer.WriteStartElement(token.TokenType.ToString().ToLower());
					writer.WriteString(token.Content);
					writer.WriteEndElement();
				}

				writer.WriteEndElement();
				writer.WriteEndElement();
			}
		}

		protected void RenderSeeAlsoBlock(ReflectedMember member, System.Xml.XmlWriter writer, XmlCodeComment comment) {
			if (comment != XmlCodeComment.Empty) {
				List<XmlCodeElement> elements = comment.Elements.FindAll(e => e.Element == XmlCodeElements.SeeAlso);
				writer.WriteStartElement("seealsolist");
				foreach (SeeAlsoXmlCodeElement current in elements) {
					long memberId, typeId;
					this.Exporter.GetUniqueId(current.Member, out memberId, out typeId);
					writer.WriteStartElement("seealso");
					writer.WriteAttributeString("id", memberId.ToString());
					writer.WriteString(current.Text);
					writer.WriteEndElement();
				}
				writer.WriteEndElement();
			}
		}

		protected void Serialize(XmlCodeElement comment, System.Xml.XmlWriter writer) {
			writer.WriteStartElement(comment.Element.ToString().ToLower());

			if (comment.Element == XmlCodeElements.See) {
				SeeXmlCodeElement see = (SeeXmlCodeElement)comment;
				long mId, tId;
				this.Exporter.GetUniqueId(see.Member, out mId, out tId);
				writer.WriteAttributeString("member", mId.ToString());
				writer.WriteAttributeString("type", tId.ToString());
			}

			if (comment is XmlContainerCodeElement) {
				foreach (XmlCodeElement element in ((XmlContainerCodeElement)comment).Elements) {
					this.Serialize(element, writer);
				}
			}
			else {
				writer.WriteString(comment.Text);
			}
			writer.WriteEndElement();
		}
	}
}
