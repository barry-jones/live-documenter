using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheBoxSoftware.Reflection;
using TheBoxSoftware.Reflection.Comments;

namespace TheBoxSoftware.Documentation.Exporting.Rendering
{
    /// <summary>
    /// Renders the ListXmlCodeElement as XML.
    /// </summary>
    internal class ListXmlElementRenderer : XmlElementRenderer
    {
        private ListXmlCodeElement element;

        /// <summary>
        /// Initialises a new instance of the ListXmlElementRenderer class.
        /// </summary>
        /// <param name="associatedEntry"></param>
        /// <param name="element"></param>
        public ListXmlElementRenderer(Entry associatedEntry, ListXmlCodeElement element)
        {
            this.AssociatedEntry = associatedEntry;
            this.element = element;
        }

        public override void Render(System.Xml.XmlWriter writer)
        {
            if (element.IsTable())
            {
                this.RenderTable(writer);
            }
            else
            {
                this.RenderList(writer);
            }
        }

        private void RenderTable(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("table");

            // create the table header
            ListHeaderXmlCodeElement header = (ListHeaderXmlCodeElement)this.element.Elements.Find(
                e => e.Element == XmlCodeElements.ListHeader);
            writer.WriteStartElement("header");
            if (header != null)
            {
                XmlContainerCodeElement description = (XmlContainerCodeElement)this.element.Elements.Find(
                    e => e.Element == XmlCodeElements.Description);
                XmlContainerCodeElement term = (XmlContainerCodeElement)this.element.Elements.Find(
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
                        this.Serialize(child, writer);
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
            List<XmlCodeElement> items = this.element.Elements.FindAll(e => e.Element == XmlCodeElements.ListItem);
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
                            this.Serialize(child, writer);
                        }
                    }
                    writer.WriteEndElement();
                    writer.WriteStartElement("cell");
                    if (description != null)
                    {
                        foreach (XmlCodeElement child in description.Elements)
                        { // miss out the listitem and just focus on children
                            this.Serialize(child, writer);
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
            writer.WriteAttributeString("type", this.element.ListType.ToString().ToLower());

            List<XmlCodeElement> elements = this.element.Elements.FindAll(e => e.Element == XmlCodeElements.ListItem);
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
