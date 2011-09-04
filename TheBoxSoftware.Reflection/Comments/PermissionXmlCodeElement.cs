using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TheBoxSoftware.Reflection.Comments {
	public sealed class PermissionXmlCodeElement : XmlCodeElement {
		internal PermissionXmlCodeElement(XmlNode node)
			: base(XmlCodeElements.Permission) {
			if (node.Attributes["cref"] == null) { throw new AttributeRequiredException("cref", XmlCodeElements.Permission); }
			this.Member = CRefPath.Parse(node.Attributes["cref"].Value);
			this.Text = this.RemoveNewLines(node.InnerText);
			this.IsBlock = true;
		}

		/// <summary>
		/// The member (i.e. permission set) this permission points to.
		/// </summary>
		public CRefPath Member { get; set; }
	}
}
