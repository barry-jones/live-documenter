using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheBoxSoftware.Reflection.Comments;
using TheBoxSoftware.Reflection;

namespace TheBoxSoftware.Documentation.Exporting.Rendering
{
    internal class NamespaceContainerXmlRenderer : XmlRenderer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NamespaceContainerXmlRenderer"/> class.
        /// </summary>
        /// <param name="entry">The associated entry.</param>
        public NamespaceContainerXmlRenderer(Entry entry)
        {
            this.AssociatedEntry = entry;
        }

        /// <summary>
        /// Renders the XML for the namespace to the specified <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public override void Render(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("namespaces");
            writer.WriteAttributeString("id", this.AssociatedEntry.Key.ToString());
            writer.WriteAttributeString("subId", this.AssociatedEntry.SubKey);
            this.WriteCref(this.AssociatedEntry, writer);

            writer.WriteStartElement("name");
            writer.WriteAttributeString("safename", Exporter.CreateSafeName(this.AssociatedEntry.Name));
            writer.WriteString(this.AssociatedEntry.Name);
            writer.WriteEndElement();

            foreach (Entry current in this.AssociatedEntry.Children)
            {
                writer.WriteStartElement("entry");
                writer.WriteAttributeString("name", current.Name);
                writer.WriteAttributeString("key", current.Key.ToString());
                writer.WriteAttributeString("subkey", current.SubKey);
                writer.WriteAttributeString("type", "namespace");
                this.WriteCref(current, writer);

                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }
    }
}