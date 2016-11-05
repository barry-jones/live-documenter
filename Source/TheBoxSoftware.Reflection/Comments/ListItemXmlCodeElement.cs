
namespace TheBoxSoftware.Reflection.Comments
{
    using System.Xml;

    public sealed class ListItemXmlCodeElement : XmlContainerCodeElement
    {
        internal ListItemXmlCodeElement(XmlNode node)
            : base(XmlCodeElements.ListItem)
        {
            this.Elements = this.Parse(node);
            this.IsBlock = true;
        }
    }
}