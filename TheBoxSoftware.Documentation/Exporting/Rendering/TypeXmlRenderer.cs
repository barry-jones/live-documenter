using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheBoxSoftware.Reflection;
using TheBoxSoftware.Reflection.Comments;
using TheBoxSoftware.Reflection.Signitures;

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

			string displayName = this.member.GetDisplayName(false);
			writer.WriteStartElement("name");
			writer.WriteAttributeString("safename", Exporter.CreateSafeName(displayName));
			writer.WriteString(displayName);
			writer.WriteEndElement();

			writer.WriteStartElement("namespace");
			Entry namespaceEntry = this.AssociatedEntry.FindNamespace(this.member.Namespace);
			writer.WriteAttributeString("id", namespaceEntry.Key.ToString());
			writer.WriteAttributeString("name", namespaceEntry.SubKey);
			writer.WriteString(this.member.Namespace);
			writer.WriteEndElement();

			if (this.member.IsGeneric) {
				this.RenderGenericTypeParameters(this.member.GetGenericTypes(), writer, comment);
			}

			// find and output the summary
			if (comment != XmlCodeComment.Empty) {
				XmlCodeElement summary = comment.Elements.Find(currentBlock => currentBlock is SummaryXmlCodeElement);
				if (summary != null) {
					this.Serialize(summary, writer);
				}
			}

			this.RenderSyntaxBlocks(this.member, writer);
			this.RenderPermissionBlock(this.member, writer, comment);

			// find and output the remarks
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
							this.Serialize(summary, writer);
						}
					}
					writer.WriteEndElement();
					writer.WriteEndElement();
				}
				writer.WriteEndElement();
			}
			else {
				if(this.member.HasMembers) {
					this.OutputMembers(writer);
				}
			}

			if (!this.member.IsDelegate && !this.member.IsEnumeration && !this.member.IsInterface && !this.member.IsStructure) {
				this.AddInheritanceTree(this.member, writer);
			}

			writer.WriteEndElement();	// member
		}

		private void OutputMembers(System.Xml.XmlWriter writer) {
			if (this.AssociatedEntry.Children.Count == 0) return;

			writer.WriteStartElement("entries");
			List<Entry> children = this.AssociatedEntry.Children;

			Entry constructors = children.Find(entry => entry.Name == "Constructors");
			if (constructors != null) {
				var s = from child in constructors.Children orderby child.Name select child;
				foreach (Entry current in s) {
					MethodDef currentMember = (MethodDef)current.Item;
					this.WriteEntry(writer, currentMember, currentMember.GetDisplayName(false, true));
				}
			}

			Entry fields = children.Find(entry => entry.Name == "Fields");
			if (fields != null) {
				var s = from child in fields.Children orderby child.Name select child;
				foreach (Entry current in s) {
					FieldDef currentMember = (FieldDef)current.Item;
					this.WriteEntry(writer, currentMember, currentMember.Name);
				}
			}

			Entry properties = children.Find(entry => entry.Name == "Properties");
			if (properties != null) {
				var s = from child in properties.Children orderby child.Name select child;
				foreach (Entry current in s) {
					PropertyDef currentMember = (PropertyDef)current.Item;
					this.WriteEntry(writer, currentMember, currentMember.GetDisplayName(false, true));
				}
			}

			Entry events = children.Find(entry => entry.Name == "Events");
			if (events != null) {
				var s = from child in events.Children orderby child.Name select child;
				foreach (Entry current in s) {
					EventDef currentMember = (EventDef)current.Item;
					this.WriteEntry(writer, currentMember, currentMember.Name);
				}
			}

			Entry methods = children.Find(entry => entry.Name == "Methods");
			if (methods != null) {
				var s = from child in methods.Children orderby child.Name select child;
				foreach (Entry current in s) {
					MethodDef currentMember = (MethodDef)current.Item;
					this.WriteEntry(writer, currentMember, currentMember.GetDisplayName(false, true));
				}
			}

			Entry operators = children.Find(entry => entry.Name == "Operators");
			if (operators != null) {
				var s = from child in operators.Children orderby child.Name select child;
				foreach (Entry current in s) {
					MethodDef currentMember = (MethodDef)current.Item;
					this.WriteEntry(writer, currentMember, currentMember.GetDisplayName(false));
				}
			}

			var extensionMethods = from method in this.member.ExtensionMethods orderby method.Name select method;
			foreach (MethodDef currentMethod in extensionMethods) {
				//if (!this.Exporter.Document.IsMemberFiltered(currentMethod)) {
					DisplayNameSignitureConvertor displayNameSig = new DisplayNameSignitureConvertor(currentMethod, false, true, true);
					this.WriteEntry(writer, currentMethod, currentMethod.GetDisplayName(false, true), "extensionmethod");
				//}
			}

			writer.WriteEndElement();
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
					this.Serialize(summary, writer);
				}
			}
			writer.WriteEndElement();
		}

		private void WriteEntry(System.Xml.XmlWriter writer, ReflectedMember entryMember, string displayName) {
			this.WriteEntry(writer, entryMember, displayName, ReflectionHelper.GetType(entryMember));
		}

		/// <summary>
		/// Adds the inheritance tree for the specified <paramref name="type"/>.
		/// </summary>
		/// <param name="type">The type to parse and display the tree for.</param>
		/// <param name="writer">The writer to write the XML to.</param>
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
					writer.WriteAttributeString("id", current.GetGloballyUniqueId().ToString());
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
					Entry found = this.Document.Find(CRefPath.Create(current));
					if(found != null) {
						writer.WriteStartElement("type");
						if (current is TypeDef) {	// only provide ids for internal classes not filtered
							writer.WriteAttributeString("id", current.GetGloballyUniqueId().ToString());
						}
						writer.WriteAttributeString("name", current.GetDisplayName(true));
						writer.WriteEndElement();
					}
				}
			}
		}
	}
}
