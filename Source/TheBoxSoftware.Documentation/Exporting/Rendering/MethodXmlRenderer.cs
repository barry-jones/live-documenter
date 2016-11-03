
namespace TheBoxSoftware.Documentation.Exporting.Rendering
{
    using Reflection.Comments;
    using Reflection;

    /// <summary>
    /// Renders the XML for a method in the documentation.
    /// </summary>
    internal class MethodXmlRenderer : XmlRenderer
    {
        private MethodDef _member;
        private XmlCodeCommentFile _xmlComments;

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodXmlRenderer"/> class.
        /// </summary>
        /// <param name="entry">The entry to initialise the renderer with.</param>
        public MethodXmlRenderer(Entry entry)
        {
            _member = entry.Item as MethodDef;
            _xmlComments = entry.XmlCommentFile;
            AssociatedEntry = entry;
        }

        /// <summary>
        /// Renders the method XML to the <paramref name="writer."/>
        /// </summary>
        /// <param name="writer">The open valid writer to write to.</param>
        public override void Render(System.Xml.XmlWriter writer)
        {
            CRefPath crefPath = new CRefPath(_member);
            XmlCodeComment comment = _xmlComments.ReadComment(crefPath);
            string displayName = _member.GetDisplayName(false);

            writer.WriteStartElement("member");
            writer.WriteAttributeString("id", AssociatedEntry.Key.ToString());
            writer.WriteAttributeString("subId", AssociatedEntry.SubKey);
            writer.WriteAttributeString("type", ReflectionHelper.GetType(_member));
            writer.WriteAttributeString("cref", crefPath.ToString());
            writer.WriteStartElement("name");
            writer.WriteAttributeString("safename", Exporter.CreateSafeName(displayName));
            writer.WriteString(_member.GetDisplayName(false));
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

            if (_member.IsGeneric)
            {
                RenderGenericTypeParameters(_member.GetGenericTypes(), writer, comment);
            }

            if (_member.Parameters.Count > 0)
            {
                writer.WriteStartElement("parameters");
                for (int i = 0; i < _member.Parameters.Count; i++)
                {
                    if (_member.Parameters[i].Sequence == 0)
                        continue;

                    TypeRef parameterType = _member.Parameters[i].GetTypeRef();
                    TypeDef foundEntry = _member.Assembly.FindType(parameterType.Namespace, parameterType.Name);

                    writer.WriteStartElement("parameter");
                    writer.WriteAttributeString("name", _member.Parameters[i].Name);
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
                            && ((ParamXmlCodeElement)currentBlock).Name == _member.Parameters[i].Name);
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

            RenderExceptionBlock(_member, writer, comment);
            RenderPermissionBlock(_member, writer, comment);

            // find and output the summary
            if (comment != XmlCodeComment.Empty)
            {
                XmlCodeElement summary = comment.Elements.Find(currentBlock => currentBlock is SummaryXmlCodeElement);
                if (summary != null)
                {
                    Serialize(summary, writer);
                }
            }

            RenderSyntaxBlocks(_member, writer);

            // find and output the summary
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

            // find and output the see also
            if (comment != XmlCodeComment.Empty)
            {
                XmlCodeElement remarks = comment.Elements.Find(currentBlock => currentBlock is SeeAlsoXmlCodeElement);
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