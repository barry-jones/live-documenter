
namespace TheBoxSoftware.Reflection.Comments
{
    using System.Xml;

    public sealed class DescriptionXmlCodeElement : XmlContainerCodeElement 
    {
		internal DescriptionXmlCodeElement(XmlNode node)
			: base(XmlCodeElements.Description)
        {
			this.Elements = this.Parse(node);
			this.IsBlock = true;
		}
	}
}
