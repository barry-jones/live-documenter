using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TheBoxSoftware.Reflection.Comments {
	public sealed class TermXmlCodeElement : XmlContainerCodeElement {
		internal TermXmlCodeElement(XmlNode node)
			: base(XmlCodeElements.Term) {
			this.Elements = this.Parse(node);
			this.IsBlock = true;
		}
	}
}
