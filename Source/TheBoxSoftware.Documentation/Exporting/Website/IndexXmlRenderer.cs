
namespace TheBoxSoftware.Documentation.Exporting.Website
{
    using System.Xml;

    /// <summary>
    /// A <see cref="Rendering.XmlRenderer"/> that renders the only copy of the index page for the
    /// output documentation.
    /// </summary>
    internal sealed class IndexXmlRenderer : Rendering.XmlRenderer
    {
        private DocumentMap _documentMap = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexXmlRenderer"/> class.
        /// </summary>
        /// <param name="documentMap">The document map.</param>
        public IndexXmlRenderer(DocumentMap documentMap)
        {
            _documentMap = documentMap;
        }

        public override void Render(XmlWriter writer)
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("frontpage");
            writer.WriteAttributeString("id", "0");
            writer.WriteAttributeString("subId", string.Empty);

            writer.WriteStartElement("title");
            writer.WriteString("Documentation produced by Live Documenter");
            writer.WriteEndElement();

            // write all the namespaces
            writer.WriteStartElement("namespaces");
            foreach (Entry current in _documentMap)
            {
                writer.WriteStartElement("namespace");
                writer.WriteAttributeString("key", current.Key.ToString());
                writer.WriteAttributeString("subkey", current.SubKey);

                writer.WriteStartElement("name");
                writer.WriteString(current.Name);
                writer.WriteEndElement();

                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteEndElement();
            writer.WriteEndDocument();
        }
    }
}