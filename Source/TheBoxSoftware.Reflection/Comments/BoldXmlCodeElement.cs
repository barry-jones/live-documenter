
namespace TheBoxSoftware.Reflection.Comments
{
    using System.Xml;

    public sealed class BoldXmlCodeElement : XmlCodeElement 
    {
		internal BoldXmlCodeElement(XmlNode node)
			: base(XmlCodeElements.B)
        {
            Text = RemoveNewLines(node.InnerText);
            IsInline = true;
		}
	}
}
