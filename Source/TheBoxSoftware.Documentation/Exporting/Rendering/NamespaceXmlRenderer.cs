
namespace TheBoxSoftware.Documentation.Exporting.Rendering
{
    using System.Collections.Generic;
    using Reflection;
    using Reflection.Comments;

    /// <summary>
    /// Renders XML for namespaces in the document map.
    /// </summary>
    internal class NamespaceXmlRenderer : XmlRenderer
    {
        private KeyValuePair<string, List<TypeDef>> _member;
        private ICommentSource _xmlComments;

        /// <summary>
        /// Initializes a new instance of the <see cref="NamespaceXmlRenderer"/> class.
        /// </summary>
        /// <param name="entry">The associated entry.</param>
        public NamespaceXmlRenderer(Entry entry)
        {
            _member = (KeyValuePair<string, List<TypeDef>>)entry.Item;
            _xmlComments = entry.XmlCommentFile;
            AssociatedEntry = entry;
        }

        /// <summary>
        /// Renders the XML for the namespace to the specified <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public override void Render(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("namespace");
            writer.WriteAttributeString("id", AssociatedEntry.Key.ToString());
            writer.WriteAttributeString("subId", AssociatedEntry.SubKey);
            WriteCref(AssociatedEntry, writer);

            writer.WriteStartElement("name");
            writer.WriteAttributeString("safename", Exporter.CreateSafeName(_member.Key));
            writer.WriteString($"{_member.Key} Namespace");
            writer.WriteEndElement();

            foreach (Entry current in AssociatedEntry.Children)
            {
                writer.WriteStartElement("parent");
                writer.WriteAttributeString("name", current.Name);
                writer.WriteAttributeString("key", current.Key.ToString());
                writer.WriteAttributeString("type", ReflectionHelper.GetType((TypeDef)current.Item));
                writer.WriteAttributeString("visibility", ReflectionHelper.GetVisibility(current.Item));
                WriteCref(current, writer);

                // write the summary text for the current member
                XmlCodeComment comment = _xmlComments.GetComment(new CRefPath((TypeDef)current.Item));
                if (comment != null && comment.Elements != null)
                {
                    SummaryXmlCodeElement summary = comment.Elements.Find(
                        p => p is SummaryXmlCodeElement
                        ) as SummaryXmlCodeElement;
                    if (summary != null)
                    {
                        Serialize(summary, writer);
                    }
                }

                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }
    }
}