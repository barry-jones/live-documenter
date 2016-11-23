
namespace TheBoxSoftware.Reflection.Comments
{
    using System.Xml;

    public sealed class ParaXmlCodeElement : XmlContainerCodeElement
    {
        internal ParaXmlCodeElement(XmlNode node)
            : base(XmlCodeElements.Para)
        {
            IsBlock = true;
            Elements = Parse(node);
        }
    }
}