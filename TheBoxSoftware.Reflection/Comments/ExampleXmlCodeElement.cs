using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TheBoxSoftware.Reflection.Comments {
	public sealed class ExampleXmlCodeElement : XmlContainerCodeElement {
		internal ExampleXmlCodeElement(XmlNode node)
			: base(XmlCodeElements.Example) {
			this.Elements = this.Parse(node);
			this.IsBlock = true;
		}
	}
}
