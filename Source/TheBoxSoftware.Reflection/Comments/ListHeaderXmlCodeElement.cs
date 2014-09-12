using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TheBoxSoftware.Reflection.Comments {
	public sealed class ListHeaderXmlCodeElement : XmlContainerCodeElement {
		internal ListHeaderXmlCodeElement(XmlNode node)
			: base(XmlCodeElements.ListHeader) {
			this.Elements = this.Parse(node);
			this.IsBlock = true;
		}
	}
}
