
namespace TheBoxSoftware.Documentation.Exporting.Rendering
{
    using Reflection;
    using Reflection.Comments;

    class PropertyXmlRenderer : XmlRenderer
    {
        private PropertyDef _member;
        private XmlCodeCommentFile _xmlComments;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyXmlRenderer"/> class.
        /// </summary>
        /// <param name="entry">The entry to intialise the renderer with.</param>
        public PropertyXmlRenderer(Entry entry)
        {
            _member = entry.Item as PropertyDef;
            _xmlComments = entry.XmlCommentFile;
            AssociatedEntry = entry;
        }

        public override void Render(System.Xml.XmlWriter writer)
        {
            CRefPath crefPath = new CRefPath(_member);
            XmlCodeComment comment = _xmlComments.ReadComment(crefPath);
            string displayName = _member.GetDisplayName(false);

            writer.WriteStartElement("member");
            writer.WriteAttributeString("id", AssociatedEntry.Key.ToString());
            writer.WriteAttributeString("subId", AssociatedEntry.SubKey);
            writer.WriteAttributeString("type", ReflectionHelper.GetType(_member));
            WriteCref(AssociatedEntry, writer);
            writer.WriteStartElement("name");
            writer.WriteAttributeString("safename", displayName);
            writer.WriteString(displayName);
            writer.WriteEndElement();

            writer.WriteStartElement("namespace");
            Entry namespaceEntry = AssociatedEntry.FindNamespace(_member.Type.Namespace);
            writer.WriteAttributeString("id", namespaceEntry.Key.ToString());
            writer.WriteAttributeString("name", namespaceEntry.SubKey);
            writer.WriteAttributeString("cref", $"N:{_member.Type.Namespace}");
            writer.WriteString(_member.Type.Namespace);
            writer.WriteEndElement();
            writer.WriteStartElement("assembly");
            writer.WriteAttributeString("file", System.IO.Path.GetFileName(_member.Assembly.FileName));
            writer.WriteString(_member.Assembly.Name);
            writer.WriteEndElement();

            // find and output the summary
            if (comment != XmlCodeComment.Empty)
            {
                XmlCodeElement summary = comment.Elements.Find(currentBlock => currentBlock is SummaryXmlCodeElement);
                if (summary != null)
                {
                    Serialize(summary, writer);
                }
            }

            RenderExceptionBlock(_member, writer, comment);
            RenderPermissionBlock(_member, writer, comment);
            RenderSyntaxBlocks(_member, writer);

            MethodDef internalMethod = _member.GetMethod == null ? _member.SetMethod : _member.GetMethod;
            if (_member.IsIndexer && internalMethod.Parameters.Count > 0)
            {
                writer.WriteStartElement("parameters");
                for (int i = 0; i < internalMethod.Parameters.Count; i++)
                {
                    if (internalMethod.Parameters[i].Sequence == 0)
                        continue;

                    TypeRef parameterType = internalMethod.Parameters[i].GetTypeRef();
                    TypeDef foundEntry = _member.Assembly.FindType(parameterType.Namespace, parameterType.Name);

                    writer.WriteStartElement("parameter");
                    writer.WriteAttributeString("name", internalMethod.Parameters[i].Name);
                    writer.WriteStartElement("type");
                    if (foundEntry != null)
                    {
                        writer.WriteAttributeString("key", foundEntry.GetGloballyUniqueId().ToString());
                        writer.WriteAttributeString("cref", CRefPath.Create(foundEntry).ToString());
                    }
                    writer.WriteString(parameterType.GetDisplayName(false));
                    writer.WriteEndElement(); // type

                    if (comment != XmlCodeComment.Empty)
                    {
                        XmlCodeElement paramEntry = comment.Elements.Find(currentBlock =>
                            currentBlock is ParamXmlCodeElement
                            && ((ParamXmlCodeElement)currentBlock).Name == internalMethod.Parameters[i].Name);
                        if (paramEntry != null)
                        {
                            writer.WriteStartElement("description");
                            ParamXmlCodeElement paraElement = (ParamXmlCodeElement)paramEntry;
                            for (int j = 0; j < paraElement.Elements.Count; j++)
                            {
                                Serialize(paraElement.Elements[j], writer);
                            }
                            writer.WriteEndElement();
                        }
                    }

                    writer.WriteEndElement(); // parameter
                }
                writer.WriteEndElement();
            }

            // find and output the value
            if (comment != XmlCodeComment.Empty)
            {
                XmlCodeElement remarks = comment.Elements.Find(currentBlock => currentBlock is ValueXmlCodeElement);
                if (remarks != null)
                {
                    Serialize(remarks, writer);
                }
            }

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

            writer.WriteEndElement();
        }
    }
}