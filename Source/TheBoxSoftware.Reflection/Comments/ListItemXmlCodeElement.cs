
namespace TheBoxSoftware.Reflection.Comments
{
    using System.Xml;

    public sealed class ListItemXmlCodeElement : XmlContainerCodeElement
    {
        internal ListItemXmlCodeElement(XmlNode node)
            : base(XmlCodeElements.ListItem)
        {
            Elements = Parse(node);
            IsBlock = true;
        }
    }
}