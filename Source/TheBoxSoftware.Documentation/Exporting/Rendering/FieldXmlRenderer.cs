
namespace TheBoxSoftware.Documentation.Exporting.Rendering
{
    using Reflection;
    using Reflection.Comments;

    class FieldXmlRenderer : XmlRenderer
    {
        private FieldDef _member;
        private ICommentSource _xmlComments;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldXmlRenderer"/> class.
        /// </summary>
        /// <param name="entry">The entry to initialise the renderer with.</param>
        public FieldXmlRenderer(Entry entry)
        {
            _member = (FieldDef)entry.Item;
            _xmlComments = entry.XmlCommentFile;
            AssociatedEntry = entry;
        }

        public override void Render(System.Xml.XmlWriter writer)
        {
            CRefPath crefPath = new CRefPath(_member);
            XmlCodeComment comment = _xmlComments.GetComment(crefPath);

            writer.WriteStartElement("member");
            writer.WriteAttributeString("id", AssociatedEntry.Key.ToString());
            writer.WriteAttributeString("subId", AssociatedEntry.SubKey);
            writer.WriteAttributeString("type", ReflectionHelper.GetType(_member));
            writer.WriteAttributeString("cref", crefPath.ToString());
            writer.WriteStartElement("name");
            writer.WriteAttributeString("safename", Exporter.CreateSafeName(_member.Name));
            writer.WriteString(_member.Name);
            writer.WriteEndElement();

            writer.WriteStartElement("namespace");
            Entry namespaceEntry = this.AssociatedEntry.FindNamespace(_member.Type.Namespace);
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

            this.RenderPermissionBlock(_member, writer, comment);
            this.RenderSyntaxBlocks(_member, writer);

            // find and output the value
            if (comment != XmlCodeComment.Empty)
            {
                XmlCodeElement remarks = comment.Elements.Find(currentBlock => currentBlock is ValueXmlCodeElement);
                if (remarks != null)
                {
                    Serialize(remarks, writer);
                }
            }

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

            RenderSeeAlsoBlock(_member, writer, comment);

            writer.WriteEndElement();
        }
    }
}