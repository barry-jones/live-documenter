using System.Xml;

namespace TheBoxSoftware.Reflection.Comments
{
    public sealed class ParaXmlCodeElement : XmlContainerCodeElement
    {
        internal ParaXmlCodeElement(XmlNode node)
            : base(XmlCodeElements.Para)
        {
            this.IsBlock = true;
            this.Elements = this.Parse(node);
        }
    }
}