using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TheBoxSoftware.Reflection.Comments {
	public sealed class ParamRefXmlCodeElement : XmlCodeElement {
		internal ParamRefXmlCodeElement(XmlNode node)
			: base(XmlCodeElements.ParamRef) {
			if (node.Attributes["name"] == null) { throw new AttributeRequiredException("name", XmlCodeElements.ParamRef); }
			this.IsInline = true;			
			this.Text = node.Attributes["name"].Value;
		}
	}
}
