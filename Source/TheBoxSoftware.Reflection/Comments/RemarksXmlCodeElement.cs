
namespace TheBoxSoftware.Reflection.Comments
{
    using System.Xml;

    public sealed class RemarksXmlCodeElement : XmlContainerCodeElement
    {
        internal RemarksXmlCodeElement(XmlNode node)
            : base(XmlCodeElements.Remarks)
        {
            this.Elements = this.Parse(node);
            this.IsBlock = true;
        }
    }
}