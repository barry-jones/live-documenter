using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TheBoxSoftware.Reflection.Comments {
	public sealed class DescriptionXmlCodeElement : XmlContainerCodeElement {
		internal DescriptionXmlCodeElement(XmlNode node)
			: base(XmlCodeElements.Description) {
			this.Elements = this.Parse(node);
			this.IsBlock = true;
		}
	}
}
