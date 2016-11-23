
namespace TheBoxSoftware.Reflection.Comments
{
    using System.Xml;

    public sealed class ParamRefXmlCodeElement : XmlCodeElement
    {
        internal ParamRefXmlCodeElement(XmlNode node)
            : base(XmlCodeElements.ParamRef)
        {
            if(node.Attributes["name"] == null)
                throw new AttributeRequiredException("name", XmlCodeElements.ParamRef);

            IsInline = true;
            Text = node.Attributes["name"].Value;
        }
    }
}