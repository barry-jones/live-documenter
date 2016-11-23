
namespace TheBoxSoftware.Reflection.Comments
{
    using System.Xml;

    public sealed class TermXmlCodeElement : XmlContainerCodeElement
    {
        internal TermXmlCodeElement(XmlNode node)
            : base(XmlCodeElements.Term)
        {
            Elements = Parse(node);
            IsBlock = true;
        }
    }
}