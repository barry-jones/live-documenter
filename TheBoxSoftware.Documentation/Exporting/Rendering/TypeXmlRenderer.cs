using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheBoxSoftware.Reflection;
using TheBoxSoftware.Reflection.Comments;

namespace TheBoxSoftware.Documentation.Exporting.Rendering {
	class TypeXmlRenderer : XmlRenderer {
		private TypeDef member;
		private XmlCodeCommentFile xmlComments;

		/// <summary>
		/// Initializes a new instance of the <see cref="TypeXmlRenderer"/> class.
		/// </summary>
		/// <param name="entry">The entry in the document map to initialise the renderer with.</param>
		public TypeXmlRenderer(Entry entry) {
			this.member = (TypeDef)entry.Item;
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

			writer.WriteStartElement("assembly");
			writer.WriteAttributeString("file", System.IO.Path.GetFileName(this.member.Assembly.File.FileName));
			writer.WriteString(this.member.Assembly.Name);
			writer.WriteEndElement();

			writer.WriteStartElement("name");
			writer.WriteString(this.member.GetDisplayName(false));
			writer.WriteEndElement();

			writer.WriteStartElement("namespace");
			Entry namespaceEntry = this.AssociatedEntry.FindNamespace(this.member.Namespace);
			writer.WriteAttributeString("id", namespaceEntry.Key.ToString());
			writer.WriteAttributeString("name", namespaceEntry.SubKey);
			writer.WriteString(this.member.Namespace);
			writer.WriteEndElement();

			if (this.member.IsGeneric) {
				List<GenericTypeRef> genericTypes = this.member.GetGenericTypes();
				writer.WriteStartElement("genericparameters");
				for(int i = 0; i < genericTypes.Count; i++) {
					writer.WriteStartElement("parameter");
					writer.WriteAttributeString("name", genericTypes[i].Name);
					// find and output the summary
					if (comment != XmlCodeComment.Empty) {
						XmlCodeElement paramEntry = comment.Elements.Find(currentBlock => 
							currentBlock is TypeParamXmlCodeElement 
							&& ((TypeParamXmlCodeElement)currentBlock).Name == genericTypes[i].Name);
						if (paramEntry != null) {
							this.Serialize(paramEntry, writer, this.member.Assembly);
						}
					}
					writer.WriteEndElement();
				}
				writer.WriteEndElement();
			}

			// find and output the summary
			if (comment != XmlCodeComment.Empty) {
				XmlCodeElement summary = comment.Elements.Find(currentBlock => currentBlock is SummaryXmlCodeElement);
				if (summary != null) {
					this.Serialize(summary, writer, this.member.Assembly);
				}
			}

			this.RenderSyntaxBlocks(this.member, writer);

			// find and output the remarks
			if (comment != XmlCodeComment.Empty) {
				XmlCodeElement remarks = comment.Elements.Find(currentBlock => currentBlock is RemarksXmlCodeElement);
				if (remarks != null) {
					this.Serialize(remarks, writer, this.member.Assembly);
				}
			}

			this.RenderSeeAlsoBlock(member, writer, comment, this.member.Assembly);

			if (this.member.IsEnumeration) {
				writer.WriteStartElement("values");
				List<FieldDef> fields = this.member.Fields;
				for (int i = 0; i < fields.Count; i++) {
					if (fields[i].IsSystemGenerated)
						continue;
					CRefPath currentPath = CRefPath.Create(fields[i]);
					XmlCodeComment currentComment = this.xmlComments.ReadComment(currentPath);

					writer.WriteStartElement("value");
					writer.WriteStartElement("name");
					writer.WriteString(fields[i].Name);
					writer.WriteEndElement();
					writer.WriteStartElement("description");
					if (currentComment != XmlCodeComment.Empty && currentComment.Elements != null) {
						XmlCodeElement summary = currentComment.Elements.Find(currentBlock => currentBlock is SummaryXmlCodeElement);
						if (summary != null) {
							this.Serialize(summary, writer, this.member.Assembly);
						}
					}
					writer.WriteEndElement();
					writer.WriteEndElement();
				}
				writer.WriteEndElement();
			}
			else {
				if (this.AssociatedEntry.Children.Count > 0) {
					// we need to write the entries that appear as children to this document map
					// entry. it is going to be easier to use the Entry elements
					writer.WriteStartElement("entries");
					foreach (Entry parent in this.AssociatedEntry.Children) {
						// each child is a container for properties/constructors etc we are only interested in the children
						foreach (Entry current in parent.Children) {
							ReflectedMember m = current.Item as ReflectedMember;
							CRefPath currentPath = CRefPath.Create(m);
							XmlCodeComment currentComment = this.xmlComments.ReadComment(currentPath);

							writer.WriteStartElement("entry");
							writer.WriteAttributeString("id", current.Key.ToString());
							writer.WriteAttributeString("subId", current.SubKey);
							writer.WriteAttributeString("type", ReflectionHelper.GetType(m));
							writer.WriteAttributeString("visibility", ReflectionHelper.GetVisibility(m));

							writer.WriteStartElement("name");
							writer.WriteString(ReflectionHelper.GetDisplayName(m));
							writer.WriteEndElement();

							// find and output the summary
							if (currentComment != XmlCodeComment.Empty && currentComment.Elements != null) {
								XmlCodeElement summary = currentComment.Elements.Find(currentBlock => currentBlock is SummaryXmlCodeElement);
								if (summary != null) {
									this.Serialize(summary, writer, this.member.Assembly);
								}
							}
							writer.WriteEndElement();
						}

					}
					writer.WriteEndElement();
				}
			}

			this.AddInheritanceTree(this.member, writer);

			writer.WriteEndElement();	// member
		}

		/// <summary>
		/// Adds the inheritance tree for the specified <paramref name="type"/>.
		/// </summary>
		/// <param name="type">The type to parse and display the tree for.</param>
		private void AddInheritanceTree(TypeDef type, System.Xml.XmlWriter writer) {
			List<TypeRef> reverseInheritanceTree = new List<TypeRef>();
			TypeRef parent = type.InheritsFrom;
			while (parent != null) {
				reverseInheritanceTree.Add(parent);

				// only add types that are referenced in the current library
				// TODO: for some types like system, we could link to MS website
				if (parent is TypeDef) {
					parent = ((TypeDef)parent).InheritsFrom;
				}
				else {
					parent = null;
				}
			}

			if (reverseInheritanceTree.Count > 0) {
				reverseInheritanceTree.Reverse();
				writer.WriteStartElement("inheritance");

				this.WriteType(0, reverseInheritanceTree, writer);

				writer.WriteEndElement(); // inheritance
			}
		}

		private void WriteType(int index, List<TypeRef> tree, System.Xml.XmlWriter writer) {
			if (index < tree.Count) {
				writer.WriteStartElement("type");
				TypeRef current = tree[index];
				if (current is TypeDef) {	// only provide ids for internal classes
					writer.WriteAttributeString("id", ReflectionHelper.GetUniqueKey(current.Assembly, current).ToString());
				}
				writer.WriteAttributeString("name", current.GetDisplayName(true));

				this.WriteType(++index, tree, writer);
				writer.WriteEndElement();
			}
			else if(index == tree.Count) {
				writer.WriteStartElement("type");
				writer.WriteAttributeString("current", "true");
				writer.WriteAttributeString("name", this.member.GetDisplayName(true));
				this.WriteType(++index, tree, writer);
				writer.WriteEndElement();
			}
			else if (index > tree.Count) {
				foreach (TypeRef current in this.member.GetExtendingTypes()) {
					writer.WriteStartElement("type");
					if (current is TypeDef) {	// only provide ids for internal classes
						writer.WriteAttributeString("id", ReflectionHelper.GetUniqueKey(current.Assembly, current).ToString());
					}
					writer.WriteAttributeString("name", current.GetDisplayName(true));
					writer.WriteEndElement();
				}
			}
		}
	}
}
