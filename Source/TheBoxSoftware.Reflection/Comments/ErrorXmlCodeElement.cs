using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Comments {
	public class ErrorXmlCodeElement : XmlCodeElement {
		public ErrorXmlCodeElement(Exception exception) 
			: base(XmlCodeElements.Text) {
			this.Text = string.Format("[ERROR:{0}]", exception.Message);
			this.IsInline = true;
		}
	}
}
