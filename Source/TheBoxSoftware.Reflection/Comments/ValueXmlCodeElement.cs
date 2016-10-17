using System.Xml;

namespace TheBoxSoftware.Reflection.Comments
{
    public sealed class ValueXmlCodeElement : XmlContainerCodeElement
    {
        internal ValueXmlCodeElement(XmlNode node)
            : base(XmlCodeElements.Value)
        {
            this.Elements = this.Parse(node);
            this.IsBlock = true;
        }
    }
}