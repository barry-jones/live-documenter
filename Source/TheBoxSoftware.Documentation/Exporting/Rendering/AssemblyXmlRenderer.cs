
namespace TheBoxSoftware.Documentation.Exporting.Rendering
{
    using System;
    using Reflection;
    using Reflection.Comments;

    /// <summary>
    /// Renders an <see cref="AssemblyDef"/> via a <see cref="DocumentMap"/> in XML.
    /// </summary>
    internal class AssemblyXmlRenderer : XmlRenderer
    {
        private AssemblyDef _member;
        private ICommentSource _xmlComments;

        /// <summary>
        /// Initializes a new instance of the <see cref="NamespaceXmlRenderer"/> class.
        /// </summary>
        /// <param name="entry">The associated entry.</param>
        /// <exception cref="InvalidOperationException">Thrown when an Entry with an invalid Item is provided.</exception>
        public AssemblyXmlRenderer(Entry entry)
        {
            _member = entry.Item as AssemblyDef;
            _xmlComments = entry.XmlCommentFile;
            AssociatedEntry = entry;

            if (_member == null)
            {
                throw new InvalidOperationException(
                    $"Entry in DocumentMap is being exported as AssemblyDef when type is '{entry.Item.GetType()}'"
                    );
            }
        }

        /// <summary>
        /// Renders the XML for the namespace to the specified <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public override void Render(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("assembly");
            writer.WriteAttributeString("id", AssociatedEntry.Key.ToString());

            writer.WriteStartElement("name");
            writer.WriteString($"{_member.Name} Assembly");
            writer.WriteEndElement();

            foreach (Entry current in AssociatedEntry.Children)
            {
                writer.WriteStartElement("parent");
                writer.WriteAttributeString("name", current.Name);
                writer.WriteAttributeString("key", current.Key.ToString());
                writer.WriteAttributeString("type", "namespace");
                if (IncludeCRefPath)
                    writer.WriteAttributeString("cref", string.Format("N:{0}", current.Name));
                writer.WriteEndElement(); // parent
            }

            writer.WriteEndElement(); // assembly
        }
    }
}