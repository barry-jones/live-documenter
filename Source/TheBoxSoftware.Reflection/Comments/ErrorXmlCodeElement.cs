
namespace TheBoxSoftware.Reflection.Comments
{
    using System;

    public class ErrorXmlCodeElement : XmlCodeElement {
		public ErrorXmlCodeElement(Exception exception) 
			: base(XmlCodeElements.Text) {
			this.Text = string.Format("[ERROR:{0}]", exception.Message);
			this.IsInline = true;
		}
	}
}
