using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TheBoxSoftware.Reflection.Comments {
	public sealed class TypeParamRefXmlCodeElement : XmlCodeElement {
		internal TypeParamRefXmlCodeElement(XmlNode node)
			: base(XmlCodeElements.TypeParamRef) {
			if (node.Attributes["name"] == null) { throw new AttributeRequiredException("name", XmlCodeElements.TypeParamRef); }
			this.Name = node.Attributes["name"].Value;
			this.Text = this.Name;
			this.IsInline = true;
		}

		/// <summary>
		/// The name this parameter reference refers to.
		/// </summary>
		public string Name { get; set; }
	}
}
