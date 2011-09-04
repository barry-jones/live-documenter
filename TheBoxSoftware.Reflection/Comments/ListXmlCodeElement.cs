using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TheBoxSoftware.Reflection.Comments {
	/// <summary>
	/// An internal representation of the list XML element.
	/// </summary>
	public sealed class ListXmlCodeElement : XmlContainerCodeElement {
		/// <summary>
		/// Initialises a new instance of the ListXmlCodeElement class.
		/// </summary>
		/// <param name="node">The node the list is based on.</param>
		internal ListXmlCodeElement(XmlNode node)
			: base(XmlCodeElements.List) {
			this.Elements = this.Parse(node);
			this.IsBlock = true;
			this.ListType = ListTypes.Bullet; // default
			
			// the node should have a type attribute, if not default to bullet list
			XmlAttribute typeAttribute = node.Attributes["type"];
			if (typeAttribute == null) {
				if (this.IsTable()) {
					this.ListType = ListTypes.Table;
				}
			}
			else {
				switch (typeAttribute.Value.ToLower()) {
					case "table":
						this.ListType = ListTypes.Table;
						break;
					case "number":
						this.ListType = ListTypes.Number;
						break;
				}
			}
		}

		/// <summary>
		/// Checks the contents of the list to determine if it should be handled
		/// as a table or a list.
		/// </summary>
		/// <returns>True if the displayer should display a table.</returns>
		public bool IsTable() {
			return this.ListType == ListTypes.Table;
		}

		/// <summary>
		/// Gets or sets the style for bullets displayed in this list.
		/// </summary>
		public ListTypes ListType { get; set; }
	}
}
