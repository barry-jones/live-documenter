using System.Xml;

namespace TheBoxSoftware.Reflection.Comments
{
    public sealed class SummaryXmlCodeElement : XmlContainerCodeElement
    {
        internal SummaryXmlCodeElement(XmlNode node)
            : base(XmlCodeElements.Summary)
        {
            this.Elements = this.Parse(node);
            this.IsBlock = true;
        }
    }
}
