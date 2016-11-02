
namespace TheBoxSoftware.Documentation.Exporting.Rendering
{
    using System.Collections.Generic;
    using Reflection.Comments;

    /// <summary>
    /// Renders the ListXmlCodeElement as XML.
    /// </summary>
    internal class ListXmlElementRenderer : XmlElementRenderer
    {
        private ListXmlCodeElement _element;

        /// <summary>
        /// Initialises a new instance of the ListXmlElementRenderer class.
        /// </summary>
        /// <param name="associatedEntry"></param>
        /// <param name="element"></param>
        public ListXmlElementRenderer(Entry associatedEntry, ListXmlCodeElement element)
        {
            AssociatedEntry = associatedEntry;
            _element = element;
        }

        public override void Render(System.Xml.XmlWriter writer)
        {
            if (_element.IsTable())
            {
                RenderTable(writer);
            }
            else
            {
                RenderList(writer);
            }
        }

        private void RenderTable(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("table");

            // create the table header
            ListHeaderXmlCodeElement header = (ListHeaderXmlCodeElement)_element.Elements.Find(
                e => e.Element == XmlCodeElements.ListHeader);
            writer.WriteStartElement("header");
            if (header != null)
            {
                XmlContainerCodeElement description = (XmlContainerCodeElement)_element.Elements.Find(
                    e => e.Element == XmlCodeElements.Description);
                XmlContainerCodeElement term = (XmlContainerCodeElement)_element.Elements.Find(
                    e => e.Element == XmlCodeElements.Term);

                writer.WriteStartElement("cell");
                if (term != null)
                {
                    foreach (XmlCodeElement child in term.Elements)
                    { // miss out the listitem and just focus on children
                        this.Serialize(child, writer);
                    }
                }
                else
                {
                    writer.WriteElementString("text", "Term");
                }
                writer.WriteEndElement(); // column

                writer.WriteStartElement("cell");
                if (description != null)
                {
                    foreach (XmlCodeElement child in description.Elements)
                    { // miss out the listitem and just focus on children
                        Serialize(child, writer);
                    }
                }
                else
                {
                    writer.WriteElementString("text", "Description");
                }
                writer.WriteEndElement(); // column
            }
            else
            {
                writer.WriteStartElement("cell");
                writer.WriteElementString("text", "Term");
                writer.WriteEndElement(); // column
                writer.WriteStartElement("cell");
                writer.WriteElementString("text", "Description");
                writer.WriteEndElement(); // column
            }
            writer.WriteEndElement(); // header

            // rows
            List<XmlCodeElement> items = _element.Elements.FindAll(e => e.Element == XmlCodeElements.ListItem);
            foreach (ListItemXmlCodeElement currentItem in items)
            {
                if (currentItem.Elements.Count == 2)
                {
                    writer.WriteStartElement("row");
                    XmlContainerCodeElement description = (XmlContainerCodeElement)currentItem.Elements.Find(
                    e => e.Element == XmlCodeElements.Description);
                    XmlContainerCodeElement term = (XmlContainerCodeElement)currentItem.Elements.Find(
                        e => e.Element == XmlCodeElements.Term);

                    writer.WriteStartElement("cell");
                    if (term != null)
                    {
                        foreach (XmlCodeElement child in term.Elements)
                        { // miss out the listitem and just focus on children
                            Serialize(child, writer);
                        }
                    }
                    writer.WriteEndElement();
                    writer.WriteStartElement("cell");
                    if (description != null)
                    {
                        foreach (XmlCodeElement child in description.Elements)
                        { // miss out the listitem and just focus on children
                            Serialize(child, writer);
                        }
                    }
                    writer.WriteEndElement();
                    writer.WriteEndElement(); // row
                }
            }

            writer.WriteEndElement(); // table
        }

        private void RenderList(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("list");
            // listtype can be bullet or number
            writer.WriteAttributeString("type", _element.ListType.ToString().ToLower());

            List<XmlCodeElement> elements = _element.Elements.FindAll(e => e.Element == XmlCodeElements.ListItem);
            foreach (ListItemXmlCodeElement item in elements)
            {
                writer.WriteStartElement("item");
                foreach (XmlCodeElement child in item.Elements)
                { // miss out the listitem and just focus on children
                    this.Serialize(child, writer);
                }
                writer.WriteEndElement(); // item
            }

            writer.WriteEndElement(); // list
        }
    }
}
