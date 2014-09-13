using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TheBoxSoftware.Reflection.Comments {
	/// <summary>
	/// 
	/// </summary>
	public sealed class CXmlCodeElement : XmlCodeElement {
		internal CXmlCodeElement(XmlNode node)
			: base(XmlCodeElements.C) {
			// 
			this.Text = this.RemoveNewLines(node.InnerText);
			this.IsInline = true;
		}
	}
}
