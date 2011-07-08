using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheBoxSoftware.Reflection.Syntax;
using TheBoxSoftware.Reflection.Comments;
using TheBoxSoftware.Reflection;

namespace TheBoxSoftware.Documentation.Exporting.Rendering {
	/// <summary>
	/// Renders the <see cref="Entry"/> to XML
	/// </summary>
	internal abstract class XmlRenderer : IRenderer<System.Xml.XmlWriter> {
		protected Entry AssociatedEntry { get; set; }
		public Exporter Exporter { get; set; }

		public abstract void Render(System.Xml.XmlWriter writer);

		/// <summary>
		/// Factory method for creating new instances of <see cref="XmlRenderer"/>. Instantiates
		/// the correct renderer forthe specied document map <see cref="Entry"/>.
		/// </summary>
		/// <param name="entry">The entry in the document map to render.</param>
		/// <param name="exporter">The exporter.</param>
		/// <returns>A valid renderer or null.</returns>
		public static XmlRenderer Create(Entry entry, Exporter exporter) {
			XmlRenderer renderer = null;

			if (entry.Item is ReflectedMember) {
				if (entry.Item is TypeDef && string.IsNullOrEmpty(entry.SubKey)) {
					renderer = new TypeXmlRenderer(entry);
				}
				else if (entry.Item is MethodDef) {
					renderer = new MethodXmlRenderer(entry);
				}
				else if (entry.Item is FieldDef) {
					renderer = new FieldXmlRenderer(entry);
				}
				else if (entry.Item is PropertyDef) {
					renderer = new PropertyXmlRenderer(entry);
				}
				else if (entry.Item is EventDef) {
					renderer = new EventXmlRenderer(entry);
				}
			}
			else if (entry.Item is KeyValuePair<string, List<TypeDef>>) { // namespace
				renderer = new NamespaceXmlRenderer(entry);
			}
			else if (entry.Item is List<PropertyDef> || entry.Item is List<MethodDef> || entry.Item is List<FieldDef> || entry.Item is List<EventDef>) {
				renderer = new TypeMembersXmlRenderer(entry);
			}
			
			// TODO: still need to ouput a members page xml page

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
			if (comment != XmlCodeComment.Empty) {
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
}
