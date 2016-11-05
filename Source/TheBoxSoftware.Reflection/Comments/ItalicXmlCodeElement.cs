
namespace TheBoxSoftware.Reflection.Comments
{
    using System.Xml;

    public sealed class ItalicXmlCodeElement : XmlCodeElement
    {
		internal ItalicXmlCodeElement(XmlNode node)
			: base(XmlCodeElements.I) 
        {
			this.Text = this.RemoveNewLines(node.InnerText);
			this.IsInline = true;
		}
	}
}
