
namespace TheBoxSoftware.Documentation.Exporting.Rendering
{
    using System.Collections.Generic;
    using System.Linq;
    using Reflection;
    using Reflection.Comments;
    using Reflection.Signitures;

    internal sealed class TypeXmlRenderer : XmlRenderer
    {
        private TypeDef _member;
        private XmlCodeCommentFile _xmlComments;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeXmlRenderer"/> class.
        /// </summary>
        /// <param name="entry">The entry in the document map to initialise the renderer with.</param>
        public TypeXmlRenderer(Entry entry)
        {
            _member = (TypeDef)entry.Item;
            _xmlComments = entry.XmlCommentFile;
            AssociatedEntry = entry;
        }

        public override void Render(System.Xml.XmlWriter writer)
        {
            CRefPath crefPath = new CRefPath(_member);
            XmlCodeComment comment = _xmlComments.GetComment(crefPath);

            writer.WriteStartElement("member");
            writer.WriteAttributeString("id", this.AssociatedEntry.Key.ToString());
            writer.WriteAttributeString("subId", this.AssociatedEntry.SubKey);
            writer.WriteAttributeString("type", ReflectionHelper.GetType(_member));
            WriteCref(AssociatedEntry, writer);

            writer.WriteStartElement("assembly");
            writer.WriteAttributeString("file", System.IO.Path.GetFileName(_member.Assembly.FileName));
            writer.WriteString(_member.Assembly.Name);
            writer.WriteEndElement();

            string displayName = _member.GetDisplayName(false);
            writer.WriteStartElement("name");
            writer.WriteAttributeString("safename", Exporter.CreateSafeName(displayName));
            writer.WriteString(displayName);
            writer.WriteEndElement();

            writer.WriteStartElement("namespace");
            Entry namespaceEntry = this.AssociatedEntry.FindNamespace(_member.Namespace);
            writer.WriteAttributeString("id", namespaceEntry.Key.ToString());
            writer.WriteAttributeString("name", namespaceEntry.SubKey);
            writer.WriteAttributeString("cref", $"N:{_member.Namespace}");
            writer.WriteString(_member.Namespace);
            writer.WriteEndElement();

            if (_member.IsGeneric)
            {
                RenderGenericTypeParameters(_member.GetGenericTypes(), writer, comment);
            }

            // find and output the summary
            if (comment != XmlCodeComment.Empty)
            {
                XmlCodeElement summary = comment.Elements.Find(currentBlock => currentBlock is SummaryXmlCodeElement);
                if (summary != null)
                {
                    this.Serialize(summary, writer);
                }
            }

            RenderSyntaxBlocks(_member, writer);
            RenderPermissionBlock(_member, writer, comment);

            // find and output the remarks
            if (comment != XmlCodeComment.Empty)
            {
                XmlCodeElement remarks = comment.Elements.Find(currentBlock => currentBlock is RemarksXmlCodeElement);
                if (remarks != null)
                {
                    Serialize(remarks, writer);
                }
            }

            // find and output the examples
            if (comment != XmlCodeComment.Empty)
            {
                XmlCodeElement remarks = comment.Elements.Find(currentBlock => currentBlock is ExampleXmlCodeElement);
                if (remarks != null)
                {
                    Serialize(remarks, writer);
                }
            }

            RenderSeeAlsoBlock(_member, writer, comment);

            if (_member.IsEnumeration)
            {
                writer.WriteStartElement("values");
                List<FieldDef> fields = _member.Fields;
                for (int i = 0; i < fields.Count; i++)
                {
                    if (fields[i].IsSystemGenerated)
                        continue;
                    CRefPath currentPath = CRefPath.Create(fields[i]);
                    XmlCodeComment currentComment = _xmlComments.GetComment(currentPath);

                    writer.WriteStartElement("value");
                    writer.WriteStartElement("name");
                    writer.WriteString(fields[i].Name);
                    writer.WriteEndElement();
                    writer.WriteStartElement("description");
                    if (currentComment != XmlCodeComment.Empty && currentComment.Elements != null)
                    {
                        XmlCodeElement summary = currentComment.Elements.Find(currentBlock => currentBlock is SummaryXmlCodeElement);
                        if (summary != null)
                        {
                            Serialize(summary, writer);
                        }
                    }
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            else
            {
                if (_member.HasMembers)
                {
                    OutputMembers(writer);
                }
            }

            if (!_member.IsDelegate && !_member.IsEnumeration && !_member.IsInterface && !_member.IsStructure)
            {
                AddInheritanceTree(_member, writer);
            }

            writer.WriteEndElement();   // member
        }

        private void OutputMembers(System.Xml.XmlWriter writer)
        {
            if (this.AssociatedEntry.Children.Count == 0) return;

            writer.WriteStartElement("entries");
            List<Entry> children = AssociatedEntry.Children;

            Entry constructors = children.Find(entry => entry.Name == "Constructors");
            if (constructors != null)
            {
                var s = from child in constructors.Children orderby child.Name select child;
                foreach (Entry current in s)
                {
                    MethodDef currentMember = (MethodDef)current.Item;
                    WriteEntry(writer, currentMember, currentMember.GetDisplayName(false, true));
                }
            }

            Entry fields = children.Find(entry => entry.Name == "Fields");
            if (fields != null)
            {
                var s = from child in fields.Children orderby child.Name select child;
                foreach (Entry current in s)
                {
                    FieldDef currentMember = (FieldDef)current.Item;
                    WriteEntry(writer, currentMember, currentMember.Name);
                }
            }

            Entry properties = children.Find(entry => entry.Name == "Properties");
            if (properties != null)
            {
                var s = from child in properties.Children orderby child.Name select child;
                foreach (Entry current in s)
                {
                    PropertyDef currentMember = current.Item as PropertyDef;
                    WriteEntry(writer, currentMember, new DisplayNameSignitureConvertor(currentMember, false, true).Convert());
                }
            }

            Entry events = children.Find(entry => entry.Name == "Events");
            if (events != null)
            {
                var s = from child in events.Children orderby child.Name select child;
                foreach (Entry current in s)
                {
                    EventDef currentMember = (EventDef)current.Item;
                    WriteEntry(writer, currentMember, currentMember.Name);
                }
            }

            Entry methods = children.Find(entry => entry.Name == "Methods");
            if (methods != null)
            {
                var s = from child in methods.Children orderby child.Name select child;
                foreach (Entry current in s)
                {
                    MethodDef currentMember = (MethodDef)current.Item;
                    WriteEntry(writer, currentMember, currentMember.GetDisplayName(false, true));
                }
            }

            Entry operators = children.Find(entry => entry.Name == "Operators");
            if (operators != null)
            {
                var s = from child in operators.Children orderby child.Name select child;
                foreach (Entry current in s)
                {
                    MethodDef currentMember = (MethodDef)current.Item;
                    WriteEntry(writer, currentMember, currentMember.GetDisplayName(false));
                }
            }

            // I dont think we have any extension methods anymore - perhaps we can just delete this code?
            var extensionMethods = from method in this._member.ExtensionMethods orderby method.Name select method;
            foreach (MethodDef currentMethod in extensionMethods)
            {
                DisplayNameSignitureConvertor displayNameSig = new DisplayNameSignitureConvertor(currentMethod, false, true, true);
                WriteEntry(writer, currentMethod, currentMethod.GetDisplayName(false, true), "extensionmethod");
            }

            writer.WriteEndElement();
        }

        private void WriteEntry(System.Xml.XmlWriter writer, ReflectedMember entryMember, string displayName, string type)
        {
            CRefPath currentPath = CRefPath.Create(entryMember);
            XmlCodeComment currentComment = this._xmlComments.GetComment(currentPath);

            writer.WriteStartElement("entry");
            writer.WriteAttributeString("id", entryMember.GetGloballyUniqueId().ToString());
            writer.WriteAttributeString("subId", string.Empty);
            writer.WriteAttributeString("type", type);
            writer.WriteAttributeString("visibility", ReflectionHelper.GetVisibility(entryMember));
            writer.WriteAttributeString("cref", currentPath.ToString());

            writer.WriteStartElement("name");
            writer.WriteString(displayName);
            writer.WriteEndElement();

            // find and output the summary
            if (currentComment != XmlCodeComment.Empty && currentComment.Elements != null)
            {
                XmlCodeElement summary = currentComment.Elements.Find(currentBlock => currentBlock is SummaryXmlCodeElement);
                if (summary != null)
                {
                    Serialize(summary, writer);
                }
            }
            writer.WriteEndElement();
        }

        private void WriteEntry(System.Xml.XmlWriter writer, ReflectedMember entryMember, string displayName)
        {
            WriteEntry(writer, entryMember, displayName, ReflectionHelper.GetType(entryMember));
        }

        /// <summary>
        /// Adds the inheritance tree for the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to parse and display the tree for.</param>
        /// <param name="writer">The writer to write the XML to.</param>
        private void AddInheritanceTree(TypeDef type, System.Xml.XmlWriter writer)
        {
            List<TypeRef> reverseInheritanceTree = new List<TypeRef>();
            TypeRef parent = type.InheritsFrom;
            while (parent != null)
            {
                reverseInheritanceTree.Add(parent);

                // only add types that are referenced in the current library
                // TODO: for some types like system, we could link to MS website
                if (parent is TypeDef)
                {
                    parent = ((TypeDef)parent).InheritsFrom;
                }
                else
                {
                    parent = null;
                }
            }

            if (reverseInheritanceTree.Count > 0)
            {
                reverseInheritanceTree.Reverse();
                writer.WriteStartElement("inheritance");

                WriteType(0, reverseInheritanceTree, writer);

                writer.WriteEndElement(); // inheritance
            }
        }

        private void WriteType(int index, List<TypeRef> tree, System.Xml.XmlWriter writer)
        {
            if (index < tree.Count)
            {
                writer.WriteStartElement("type");
                TypeRef current = tree[index];
                if (current is TypeDef)
                {   // only provide ids for internal classes
                    writer.WriteAttributeString("id", current.GetGloballyUniqueId().ToString());
                    writer.WriteAttributeString("cref", CRefPath.Create(current).ToString());
                }
                writer.WriteAttributeString("name", current.GetDisplayName(true));

                WriteType(++index, tree, writer);
                writer.WriteEndElement();
            }
            else if (index == tree.Count)
            {
                writer.WriteStartElement("type");
                writer.WriteAttributeString("current", "true");
                writer.WriteAttributeString("name", _member.GetDisplayName(true));
                WriteType(++index, tree, writer);
                writer.WriteEndElement();
            }
            else if (index > tree.Count)
            {
                foreach (TypeRef current in this._member.GetExtendingTypes())
                {
                    Entry found = this.Document.Find(CRefPath.Create(current));
                    if (found != null)
                    {
                        writer.WriteStartElement("type");
                        if (current is TypeDef)
                        {   // only provide ids for internal classes not filtered
                            writer.WriteAttributeString("id", current.GetGloballyUniqueId().ToString());
                            writer.WriteAttributeString("cref", CRefPath.Create(current).ToString());
                        }
                        writer.WriteAttributeString("name", current.GetDisplayName(true));
                        writer.WriteEndElement();
                    }
                }
            }
        }
    }
}