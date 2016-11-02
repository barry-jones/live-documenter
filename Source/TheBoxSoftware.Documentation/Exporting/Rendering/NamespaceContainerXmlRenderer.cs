
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
            AssociatedEntry = entry;
        }

        /// <summary>
        /// Renders the XML for the namespace to the specified <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public override void Render(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("namespaces");
            writer.WriteAttributeString("id", AssociatedEntry.Key.ToString());
            writer.WriteAttributeString("subId", AssociatedEntry.SubKey);
            this.WriteCref(AssociatedEntry, writer);

            writer.WriteStartElement("name");
            writer.WriteAttributeString("safename", Exporter.CreateSafeName(AssociatedEntry.Name));
            writer.WriteString(AssociatedEntry.Name);
            writer.WriteEndElement();

            foreach (Entry current in AssociatedEntry.Children)
            {
                writer.WriteStartElement("entry");
                writer.WriteAttributeString("name", current.Name);
                writer.WriteAttributeString("key", current.Key.ToString());
                writer.WriteAttributeString("subkey", current.SubKey);
                writer.WriteAttributeString("type", "namespace");
                WriteCref(current, writer);

                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }
    }
}