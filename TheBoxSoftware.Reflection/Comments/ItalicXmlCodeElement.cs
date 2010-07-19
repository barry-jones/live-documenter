using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TheBoxSoftware.Reflection.Comments {
	public sealed class ItalicXmlCodeElement : XmlCodeElement {
		internal ItalicXmlCodeElement(XmlNode node)
			: base(XmlCodeElements.I) {
			this.Text = this.RemoveNewLines(node.InnerText);
			this.IsInline = true;
		}
	}
}
