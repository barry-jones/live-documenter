
namespace TheBoxSoftware.Reflection.Comments
{
    using System.Xml;

    public sealed class DescriptionXmlCodeElement : XmlContainerCodeElement 
    {
		internal DescriptionXmlCodeElement(XmlNode node)
			: base(XmlCodeElements.Description)
        {
			Elements = Parse(node);
			IsBlock = true;
		}
	}
}
