using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TheBoxSoftware.Reflection.Comments {
	/// <summary>
	/// An internal representation of the example XML comment element.
	/// </summary>
	public sealed class ExampleXmlCodeElement : XmlContainerCodeElement {
		/// <summary>
		/// Initialises a new instance of the ExampleXmlCodeElement class.
		/// </summary>
		/// <param name="node">The associated XML node.</param>
		internal ExampleXmlCodeElement(XmlNode node)
			: base(XmlCodeElements.Example) {
			this.Elements = this.Parse(node);
			this.IsBlock = true;
		}
	}
}
