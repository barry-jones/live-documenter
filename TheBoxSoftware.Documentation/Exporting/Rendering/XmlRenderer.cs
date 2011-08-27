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

			if (renderer != null) {
				renderer.Exporter = exporter;
			}

			return renderer;
		}

		/// <summary>
		/// Renders the list of defined exceptions for the specified <paramref name="member"/>.
		/// </summary>
		/// <param name="member">The member to render the exceptions for.</param>
		/// <param name="writer">The writer to write the exceptions to.</param>
		/// <param name="comment">The XmlCodeComment to read the exceptions from.</param>
		protected virtual void RenderExceptionBlock(ReflectedMember member, System.Xml.XmlWriter writer, XmlCodeComment comment) {
			// output documentation for expected exceptions if they are defined
			if (comment != XmlCodeComment.Empty) {
				List<XmlCodeElement> exceptions = comment.Elements.FindAll(node => node is ExceptionXmlCodeElement);
				if (exceptions != null && exceptions.Count > 0) {
					writer.WriteStartElement("exceptions");
					for (int i = 0; i < exceptions.Count; i++) {
						ExceptionXmlCodeElement current = (ExceptionXmlCodeElement)exceptions[i];
						string exceptionName = string.Empty;
						ReflectedMember found = null;
						if (current.Member.PathType != CRefTypes.Error) {
							TypeDef def = member.Assembly.FindType(current.Member.Namespace, current.Member.TypeName);
							exceptionName = string.Format("{0}.{1}", current.Member.Namespace, current.Member.TypeName);

							if (def != null) {
								found = def;
								switch (current.Member.PathType) {
									// these elements are named and the type of element will
									// not modify how it should be displayed
									case CRefTypes.Field:
									case CRefTypes.Property:
									case CRefTypes.Event:
									case CRefTypes.Namespace:
										break;

									// these could be generic and so will need to modify to
									// a more appropriate display name
									case CRefTypes.Method:
										MethodDef method = current.Member.FindIn(def) as MethodDef;
										if (method != null) {
											found = method;
											exceptionName = method.GetDisplayName(false);
										}
										break;
									case CRefTypes.Type:
										exceptionName = def.GetDisplayName(false);
										break;
								}
							}
						}

						writer.WriteStartElement("exception");
						writer.WriteStartElement("name");
						if (found != null) {
							writer.WriteAttributeString("key", member.GetGloballyUniqueId().ToString());
						}
						writer.WriteString(exceptionName);
						writer.WriteEndElement();
						writer.WriteStartElement("condition");
						for (int j = 0; j < current.Elements.Count; j++) {
							this.Serialize(current.Elements[j], writer, member.Assembly);
						}
						writer.WriteEndElement();
						writer.WriteEndElement();
					}
					writer.WriteEndElement();
				}
			}
		}

		/// <summary>
		/// Renders the generic types in XML.
		/// </summary>
		/// <param name="genericTypes">The generic types to render.</param>
		/// <param name="writer">The writer to write to.</param>
		/// <param name="comment">The XmlCodeComment to read comments from.</param>
		protected virtual void RenderGenericTypeParameters(List<GenericTypeRef> genericTypes, System.Xml.XmlWriter writer, XmlCodeComment comment) {
			writer.WriteStartElement("genericparameters");
			for (int i = 0; i < genericTypes.Count; i++) {
				writer.WriteStartElement("parameter");
				writer.WriteElementString("name", genericTypes[i].Name);
				writer.WriteStartElement("description");
				// find and output the summary
				if (comment != XmlCodeComment.Empty) {
					XmlCodeElement paramEntry = comment.Elements.Find(currentBlock =>
						currentBlock is TypeParamXmlCodeElement
						&& ((TypeParamXmlCodeElement)currentBlock).Name == genericTypes[i].Name);
					if (paramEntry != null) {
						writer.WriteString(paramEntry.Text);
					}
				}
				writer.WriteEndElement(); // description
				writer.WriteEndElement(); // parameter
			}
			writer.WriteEndElement();
		}

		/// <summary>
		/// Renders the syntax block for the specified <paramref name="member"/>.
		/// </summary>
		/// <param name="member">The member to render the syntax for.</param>
		/// <param name="writer">The writer to write the syntax to.</param>
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

		/// <summary>
		/// Renders a seealso xml block when the specified member has seealso references in the comments.
		/// </summary>
		/// <param name="member">The member to render the block for.</param>
		/// <param name="writer">The writer to write the xml to.</param>
		/// <param name="comment">The associated xml comments.</param>
		protected void RenderSeeAlsoBlock(ReflectedMember member, System.Xml.XmlWriter writer, XmlCodeComment comment, AssemblyDef assembly) {
			if (comment != XmlCodeComment.Empty) {
				List<XmlCodeElement> elements = comment.Elements.FindAll(e => e.Element == XmlCodeElements.SeeAlso);
				if (elements != null && elements.Count > 0) {
					writer.WriteStartElement("seealsolist");
					foreach (SeeAlsoXmlCodeElement current in elements) {
						if (current.Member.PathType != CRefTypes.Error) {
							long memberId, typeId;
							this.Exporter.GetUniqueId(current.Member, out memberId, out typeId);
							writer.WriteStartElement("seealso");

							TypeDef def = assembly.FindType(current.Member.Namespace, current.Member.TypeName);
							string displayName = current.Member.TypeName;
							if (def != null) displayName = def.GetDisplayName(false);

							if (def != null) {
								writer.WriteAttributeString("id", def.GetGloballyUniqueId().ToString());
							}
							else if (memberId != 0) {
								writer.WriteAttributeString("id", memberId.ToString());
							}

							writer.WriteString(displayName);
							writer.WriteEndElement(); // seealso
						}
					}
					writer.WriteEndElement(); // seealsolist
				}
			}
		}

		/// <summary>
		/// Serializes an <see cref="XmlCodeElement"/> to XML.
		/// </summary>
		/// <param name="comment">The XML code comment to serialize.</param>
		/// <param name="writer">The XmlWriter to serialize to.</param>
		/// <param name="assembly">The assembly associated with the commented type.</param>
		protected void Serialize(XmlCodeElement comment, System.Xml.XmlWriter writer, AssemblyDef assembly) {
			if (comment != XmlCodeComment.Empty) {
				if (comment.Element == XmlCodeElements.See) {
					SeeXmlCodeElement see = (SeeXmlCodeElement)comment;
					long mId, tId;
					this.Exporter.GetUniqueId(see.Member, out mId, out tId);
					string displayName = see.Text;

					if (see.Member.PathType != CRefTypes.Error) {
						writer.WriteStartElement(comment.Element.ToString().ToLower());
						TypeDef def = assembly.FindType(see.Member.Namespace, see.Member.TypeName);

						switch (see.Member.PathType) {
							// these elements are named and the type of element will
							// not modify how it should be displayed
							case CRefTypes.Field:
							case CRefTypes.Property:
							case CRefTypes.Event:
								if (mId != 0) {
									writer.WriteAttributeString("id", mId.ToString());
								}
								break;

							case CRefTypes.Namespace:
								writer.WriteAttributeString("id", assembly.GetGloballyUniqueId().ToString());
								writer.WriteAttributeString("type", "namespace");
								writer.WriteAttributeString("name", displayName);
								break;

							// these could be generic and so will need to modify to
							// a more appropriate display name
							case CRefTypes.Method:								
								if (def != null) {
									MethodDef method = see.Member.FindIn(def) as MethodDef;

									if (method != null) {
										writer.WriteAttributeString("id", method.GetGloballyUniqueId().ToString());
										displayName = method.GetDisplayName(false);
									}
								}
								break;
							case CRefTypes.Type:
								if (def != null) {
									writer.WriteAttributeString("id", def.GetGloballyUniqueId().ToString());
									displayName = def.GetDisplayName(false);
								}
								break;
						}

						writer.WriteString(displayName);
						writer.WriteEndElement();	// element
					}
				}
				else if (comment is XmlContainerCodeElement) {
					writer.WriteStartElement(comment.Element.ToString().ToLower());
					foreach (XmlCodeElement element in ((XmlContainerCodeElement)comment).Elements) {
						this.Serialize(element, writer, assembly);
					}
					writer.WriteEndElement();
				}
				else {
					writer.WriteStartElement(comment.Element.ToString().ToLower());
					writer.WriteString(comment.Text);
					writer.WriteEndElement();
				}
			}
		}
	}
}
