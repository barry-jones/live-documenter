using System.Xml;

namespace TheBoxSoftware.Reflection.Comments
{
    public sealed class ReturnsXmlCodeElement : XmlContainerCodeElement
    {
        internal ReturnsXmlCodeElement(XmlNode node)
            : base(XmlCodeElements.Returns)
        {
            this.IsBlock = true;
            this.Elements = this.Parse(node);
        }
    }
}