using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TheBoxSoftware.Reflection.Comments {
	public sealed class ParaXmlCodeElement : XmlContainerCodeElement {
		internal ParaXmlCodeElement(XmlNode node)
			: base(XmlCodeElements.Para) {
			this.IsBlock = true;
			this.Elements = this.Parse(node);
		}
	}
}
