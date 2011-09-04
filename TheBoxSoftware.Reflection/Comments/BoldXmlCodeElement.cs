using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TheBoxSoftware.Reflection.Comments {
	public sealed class BoldXmlCodeElement : XmlCodeElement {
		internal BoldXmlCodeElement(XmlNode node)
			: base(XmlCodeElements.B) {
			this.Text = this.RemoveNewLines(node.InnerText);
			this.IsInline = true;
		}
	}
}
