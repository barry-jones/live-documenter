using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TheBoxSoftware.Reflection.Comments {
	/// <summary>
	/// The details of the comments associated with a single member.
	/// </summary>
	public sealed class XmlCodeComment : XmlContainerCodeElement {
		/// <summary>
		/// Static constructor
		/// </summary>
		static XmlCodeComment() {
			XmlCodeComment.Empty = new XmlCodeComment();
		}

		/// <summary>
		/// Default constructor
		/// </summary>
		private XmlCodeComment() {
		}

		/// <summary>
		/// Initialises a new XmlCodeComment instance.
		/// </summary>
		/// <param name="node">The node to parse the comment details from.</param>
		/// <exception cref="ArgumentNullException">
		/// Thrown when the <paramref name="node"/> is null.
		/// </exception>
		public XmlCodeComment(XmlNode node) {
			if (node == null)
				throw new ArgumentNullException("node");
			this.Elements = XmlContainerCodeElement.ParseChildren(node);
		}

		/// <summary>
		/// The member which this XmlCodeComment is for.
		/// </summary>
		public CRefPath Member { get; set; }

		/// <summary>
		/// Gets a valid but empty XmlCodeComment reference.
		/// </summary>
		public static XmlCodeComment Empty { get; private set; }
	}
}
