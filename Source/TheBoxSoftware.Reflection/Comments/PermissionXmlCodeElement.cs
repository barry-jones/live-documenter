using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TheBoxSoftware.Reflection.Comments {
	/// <summary>
	/// Represents the permission XML code element.
	/// </summary>
	public sealed class PermissionXmlCodeElement : XmlContainerCodeElement {
		/// <summary>
		/// Initialises a new instance of the PermissionXmlCodeElement class.
		/// </summary>
		/// <param name="node">The XML node representing the permission details.</param>
		/// <exception cref="AttributeRequiredException">The cref attribute was missing.</exception>
		internal PermissionXmlCodeElement(XmlNode node)
			: base(XmlCodeElements.Permission) {
			if (node.Attributes["cref"] == null) { throw new AttributeRequiredException("cref", XmlCodeElements.Permission); }
			this.Member = CRefPath.Parse(node.Attributes["cref"].Value);
			this.Elements = this.Parse(node);
			this.IsBlock = true;
		}

		/// <summary>
		/// The member (i.e. permission set) this permission points to.
		/// </summary>
		public CRefPath Member { get; set; }
	}
}
