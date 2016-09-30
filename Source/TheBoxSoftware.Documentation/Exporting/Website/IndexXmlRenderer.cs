using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TheBoxSoftware.Documentation.Exporting.Website
{
    /// <summary>
    /// A <see cref="XmlRenderer"/> that renders the only copy of the index page for the
    /// output documentation.
    /// </summary>
    internal sealed class IndexXmlRenderer : Rendering.XmlRenderer
    {
        private DocumentMap documentMap = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexXmlRenderer"/> class.
        /// </summary>
        /// <param name="documentMap">The document map.</param>
        public IndexXmlRenderer(DocumentMap documentMap)
        {
            this.documentMap = documentMap;
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
            foreach (Entry current in this.documentMap)
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