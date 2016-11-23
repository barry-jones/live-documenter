
namespace TheBoxSoftware.Reflection.Comments
{
    using System.Xml;

    public sealed class ValueXmlCodeElement : XmlContainerCodeElement
    {
        internal ValueXmlCodeElement(XmlNode node)
            : base(XmlCodeElements.Value)
        {
            Elements = Parse(node);
            IsBlock = true;
        }
    }
}