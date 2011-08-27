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

			// find and output the examples
			if (comment != XmlCodeComment.Empty) {
				XmlCodeElement remarks = comment.Elements.Find(currentBlock => currentBlock is ExampleXmlCodeElement);
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
			writer.WriteStartElement("entries");
			var constructors = from method in this.member.GetConstructors() orderby method.Name select method;
			foreach (MethodDef currentMethod in constructors) {
				this.WriteEntry(writer, currentMethod, currentMethod.GetDisplayName(false, true));
			}

			var fields = from field in this.member.GetFields() orderby field.Name select field;
			foreach (FieldDef currentField in fields) {
				this.WriteEntry(writer, currentField, currentField.Name);
			}

			var properties = from property in this.member.GetProperties() orderby property.Name select property;
			foreach (PropertyDef currentProperty in properties) {
				this.WriteEntry(writer, currentProperty, currentProperty.GetDisplayName(false, true));
			}

			var events = from ev in this.member.GetEvents() orderby ev.Name select ev;
			foreach (EventDef currentEvent in events) {
				this.WriteEntry(writer, currentEvent, currentEvent.Name);
			}

			var methods = from method in this.member.GetMethods() orderby method.Name select method;
			foreach (MethodDef currentMethod in methods) {
				this.WriteEntry(writer, currentMethod, currentMethod.GetDisplayName(false, true));
			}

			var operators = from op in this.member.GetOperators() orderby op.Name select op;
			foreach (MethodDef currentMethod in operators) {
				this.WriteEntry(writer, currentMethod, currentMethod.GetDisplayName(false));
			}

			var extensionMethods = from method in this.member.ExtensionMethods orderby method.Name select method;
			foreach (MethodDef currentMethod in extensionMethods) {
				DisplayNameSignitureConvertor displayNameSig = new DisplayNameSignitureConvertor(currentMethod, false, true, true);
				this.WriteEntry(writer, currentMethod, currentMethod.GetDisplayName(false, true), "extensionmethod");
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
					this.Serialize(summary, writer, this.member.Assembly);
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
					writer.WriteStartElement("type");
					if (current is TypeDef) {	// only provide ids for internal classes
						writer.WriteAttributeString("id", current.GetGloballyUniqueId().ToString());
					}
					writer.WriteAttributeString("name", current.GetDisplayName(true));
					writer.WriteEndElement();
				}
			}
		}
	}
}
