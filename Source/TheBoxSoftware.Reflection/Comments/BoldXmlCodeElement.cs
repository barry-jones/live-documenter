
namespace TheBoxSoftware.Reflection.Comments
{
    using System.Xml;

    public sealed class BoldXmlCodeElement : XmlCodeElement 
    {
		internal BoldXmlCodeElement(XmlNode node)
			: base(XmlCodeElements.B)
        {
			this.Text = this.RemoveNewLines(node.InnerText);
			this.IsInline = true;
		}
	}
}
