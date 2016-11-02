
namespace TheBoxSoftware.Documentation.Exporting.Rendering
{
    using Reflection;
    using Reflection.Comments;

    /// <summary>
    /// Renders the XML for SeeXmlCodeElements.
    /// </summary>
    internal class SeeXmlElementRenderer : XmlElementRenderer
    {
        private SeeXmlCodeElement _element;

        /// <summary>
        /// Initialises a new instance of the SeeXmlElementRenderer class.
        /// </summary>
        /// <param name="associatedEntry"></param>
        /// <param name="element"></param>
        public SeeXmlElementRenderer(Entry associatedEntry, SeeXmlCodeElement element)
        {
            AssociatedEntry = associatedEntry;
            _element = element;
        }

        public override void Render(System.Xml.XmlWriter writer)
        {
            Entry entry = Document.Find(_element.Member);
            string displayName = string.IsNullOrEmpty(_element.Member.ElementName)
                ? _element.Member.TypeName
                : _element.Member.ElementName;

            if (_element.Member.PathType != CRefTypes.Error)
            {
                writer.WriteStartElement("see");

                if (entry != null)
                {
                    displayName = entry.Name;
                    writer.WriteAttributeString("id", entry.Key.ToString());
                    writer.WriteAttributeString("cref", _element.Member.ToString());

                    switch (_element.Member.PathType)
                    {
                        case CRefTypes.Namespace:
                            writer.WriteAttributeString("type", "namespace");
                            writer.WriteAttributeString("name", displayName);
                            break;
                        // these could be generic and so will need to modify to
                        // a more appropriate display name
                        case CRefTypes.Method:
                            MethodDef method = entry.Item as MethodDef;
                            if (method != null)
                            {
                                displayName = method.GetDisplayName(false);
                            }
                            break;
                        case CRefTypes.Type:
                            TypeDef def = entry.Item as TypeDef;
                            if (def != null)
                            {
                                displayName = def.GetDisplayName(false);
                            }
                            break;
                    }
                }

                writer.WriteString(displayName);
                writer.WriteEndElement();   // element
            }
        }
    }
}