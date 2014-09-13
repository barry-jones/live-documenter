using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TheBoxSoftware.Reflection.Comments {
    /// <summary>
    /// This XML element allows people to associate other useful elements to the
    /// current documentation. I.e. for further reading.
    /// </summary>
	public sealed class SeeAlsoXmlCodeElement : XmlCodeElement {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="node">The node describing the seealso xml element.</param>
		internal SeeAlsoXmlCodeElement(XmlNode node)
			: base(XmlCodeElements.SeeAlso) {
			if (node.Attributes["cref"] == null) { throw new AttributeRequiredException("cref", XmlCodeElements.SeeAlso); }
			this.Member = CRefPath.Parse(node.Attributes["cref"].Value);
			switch (this.Member.PathType) {
				case CRefTypes.Type:
					this.Text = this.Member.TypeName;
					break;
				case CRefTypes.Namespace:
					this.Text = this.Member.Namespace;
					break;
				default:
					if (!string.IsNullOrEmpty(this.Member.ElementName)) {
						this.Text = this.Member.ElementName;
					}
					else if(!string.IsNullOrEmpty(node.Attributes["cref"].Value)) {
						this.Text = node.Attributes["cref"].Value.Substring(2);
					}					
					break;
			}
			this.IsInline = true;
		}

		/// <summary>
		/// Obtains the member this see also element refers to.
		/// </summary>
		public CRefPath Member { get; set; }
	}
}
