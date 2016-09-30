using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheBoxSoftware.Reflection;
using TheBoxSoftware.Reflection.Comments;

namespace TheBoxSoftware.Documentation.Exporting.Rendering
{
    class PropertyXmlRenderer : XmlRenderer
    {
        private PropertyDef member;
        private XmlCodeCommentFile xmlComments;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyXmlRenderer"/> class.
        /// </summary>
        /// <param name="entry">The entry to intialise the renderer with.</param>
        public PropertyXmlRenderer(Entry entry)
        {
            this.member = (PropertyDef)entry.Item;
            this.xmlComments = entry.XmlCommentFile;
            this.AssociatedEntry = entry;
        }

        public override void Render(System.Xml.XmlWriter writer)
        {
            CRefPath crefPath = new CRefPath(this.member);
            XmlCodeComment comment = this.xmlComments.ReadComment(crefPath);
            string displayName = this.member.GetDisplayName(false);

            writer.WriteStartElement("member");
            writer.WriteAttributeString("id", this.AssociatedEntry.Key.ToString());
            writer.WriteAttributeString("subId", this.AssociatedEntry.SubKey);
            writer.WriteAttributeString("type", ReflectionHelper.GetType(this.member));
            this.WriteCref(this.AssociatedEntry, writer);
            writer.WriteStartElement("name");
            writer.WriteAttributeString("safename", displayName);
            writer.WriteString(displayName);
            writer.WriteEndElement();

            writer.WriteStartElement("namespace");
            Entry namespaceEntry = this.AssociatedEntry.FindNamespace(this.member.Type.Namespace);
            writer.WriteAttributeString("id", namespaceEntry.Key.ToString());
            writer.WriteAttributeString("name", namespaceEntry.SubKey);
            writer.WriteAttributeString("cref", string.Format("N:{0}", this.member.Type.Namespace));
            writer.WriteString(this.member.Type.Namespace);
            writer.WriteEndElement();
            writer.WriteStartElement("assembly");
            writer.WriteAttributeString("file", System.IO.Path.GetFileName(this.member.Assembly.File.FileName));
            writer.WriteString(this.member.Assembly.Name);
            writer.WriteEndElement();

            // find and output the summary
            if (comment != XmlCodeComment.Empty)
            {
                XmlCodeElement summary = comment.Elements.Find(currentBlock => currentBlock is SummaryXmlCodeElement);
                if (summary != null)
                {
                    this.Serialize(summary, writer);
                }
            }

            this.RenderExceptionBlock(this.member, writer, comment);
            this.RenderPermissionBlock(this.member, writer, comment);
            this.RenderSyntaxBlocks(this.member, writer);

            MethodDef internalMethod = this.member.GetMethod == null ? this.member.SetMethod : this.member.GetMethod;
            if (this.member.IsIndexer && internalMethod.Parameters.Count > 0)
            {
                writer.WriteStartElement("parameters");
                for (int i = 0; i < internalMethod.Parameters.Count; i++)
                {
                    if (internalMethod.Parameters[i].Sequence == 0)
                        continue;

                    TypeRef parameterType = internalMethod.Parameters[i].GetTypeRef();
                    TypeDef foundEntry = this.member.Assembly.FindType(parameterType.Namespace, parameterType.Name);

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
                                this.Serialize(paraElement.Elements[j], writer);
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
                    this.Serialize(remarks, writer);
                }
            }

            // find and output the remarks
            if (comment != XmlCodeComment.Empty)
            {
                XmlCodeElement remarks = comment.Elements.Find(currentBlock => currentBlock is RemarksXmlCodeElement);
                if (remarks != null)
                {
                    this.Serialize(remarks, writer);
                }
            }

            // find and output the examples
            if (comment != XmlCodeComment.Empty)
            {
                XmlCodeElement remarks = comment.Elements.Find(currentBlock => currentBlock is ExampleXmlCodeElement);
                if (remarks != null)
                {
                    this.Serialize(remarks, writer);
                }
            }

            this.RenderSeeAlsoBlock(member, writer, comment);

            writer.WriteEndElement();
        }
    }
}