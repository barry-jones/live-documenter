
namespace TheBoxSoftware.Documentation.Exporting.Rendering
{
    /// <summary>
    /// Renders the <see cref="DocumentMap"/> to XML for exporting clients.
    /// </summary>
    public class DocumentMapXmlRenderer : XmlRenderer
    {
        private DocumentMap _documentMap;
        private bool _includeSafeName;

        /// <summary>
        /// Initialises a new instance of the DocumentMapXmlRenderer.
        /// </summary>
        /// <param name="documentMap">The <paramref name="documentMap"/> to render.</param>
        public DocumentMapXmlRenderer(DocumentMap documentMap) : this(documentMap, true)
        {
        }

        /// <summary>
        /// Initialises a new instance of the DocumentMapXmlRenderer.
        /// </summary>
        /// <param name="documentMap">The <paramref name="documentMap"/> to render.</param>
        /// <param name="includeSafeName">Indicates if the safename attribute should be rendered</param>
        public DocumentMapXmlRenderer(
            DocumentMap documentMap,
            bool includeSafeName)
        {
            _documentMap = documentMap;
            _includeSafeName = includeSafeName;
        }

        /// <summary>
        /// Renders the DocumentMap to the provided <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The XML writer.</param>
        public override void Render(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("toc");

            foreach (Entry current in _documentMap)
            {
                writer.WriteStartElement("item");
                writer.WriteAttributeString("name", current.Name);
                if (_includeSafeName)
                    writer.WriteAttributeString("safename", Exporter.CreateSafeName(current.Name));
                writer.WriteAttributeString("key", current.Key.ToString());
                writer.WriteAttributeString("subkey", current.SubKey);
                WriteCref(current, writer);

                foreach (Entry child in current.Children)
                {
                    Render(child, writer);
                }

                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// Renders an individual element in the DocumentMap.
        /// </summary>
        /// <param name="entry">The entry to render.</param>
        /// <param name="writer">The writer to write to.</param>
        private void Render(Entry entry, System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("item");
            writer.WriteAttributeString("name", entry.Name);
            if (_includeSafeName)
                writer.WriteAttributeString("safename", Exporter.CreateSafeName(entry.Name));
            writer.WriteAttributeString("key", entry.Key.ToString());
            writer.WriteAttributeString("subkey", entry.SubKey);
            WriteCref(entry, writer);

            foreach (Entry child in entry.Children)
            {
                Render(child, writer);
            }

            writer.WriteEndElement();
        }
    }
}