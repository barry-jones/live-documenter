using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TheBoxSoftware.Reflection.Comments {
	public sealed class ListItemXmlCodeElement : XmlContainerCodeElement {
		internal ListItemXmlCodeElement(XmlNode node)
			: base(XmlCodeElements.ListItem) {
			this.Elements = this.Parse(node);
			this.IsBlock = true;
		}
	}
}
