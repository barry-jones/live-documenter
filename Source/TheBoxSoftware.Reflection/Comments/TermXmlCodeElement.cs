
namespace TheBoxSoftware.Reflection.Comments
{
    using System.Xml;

    public sealed class TermXmlCodeElement : XmlContainerCodeElement
    {
        internal TermXmlCodeElement(XmlNode node)
            : base(XmlCodeElements.Term)
        {
            this.Elements = this.Parse(node);
            this.IsBlock = true;
        }
    }
}