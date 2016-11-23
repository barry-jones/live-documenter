
namespace TheBoxSoftware.Reflection.Comments
{
    using System.Xml;

    public sealed class ItalicXmlCodeElement : XmlCodeElement
    {
		internal ItalicXmlCodeElement(XmlNode node)
			: base(XmlCodeElements.I) 
        {
            Text = RemoveNewLines(node.InnerText);
            IsInline = true;
		}
	}
}
