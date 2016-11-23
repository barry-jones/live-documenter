
namespace TheBoxSoftware.Reflection.Comments
{
    using System.Xml;

    public sealed class ListHeaderXmlCodeElement : XmlContainerCodeElement
    {
        internal ListHeaderXmlCodeElement(XmlNode node)
            : base(XmlCodeElements.ListHeader)
        {
            Elements = Parse(node);
            IsBlock = true;
        }
    }
}