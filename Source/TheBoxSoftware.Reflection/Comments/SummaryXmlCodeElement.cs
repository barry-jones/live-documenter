
namespace TheBoxSoftware.Reflection.Comments
{
    using System.Xml;

    public sealed class SummaryXmlCodeElement : XmlContainerCodeElement
    {
        internal SummaryXmlCodeElement(XmlNode node)
            : base(XmlCodeElements.Summary)
        {
            Elements = Parse(node);
            IsBlock = true;
        }
    }
}
