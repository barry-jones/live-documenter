
namespace TheBoxSoftware.Reflection.Comments
{
    using System.Xml;

    /// <summary>
    /// Represents a tokenised version of the inine &lt;c> comment element.
    /// </summary>
    public sealed class CXmlCodeElement : XmlCodeElement 
    {
		internal CXmlCodeElement(XmlNode node)
			: base(XmlCodeElements.C) 
        {
			// 
			this.Text = this.RemoveNewLines(node.InnerText);
			this.IsInline = true;
		}
	}
}
