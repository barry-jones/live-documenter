using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TheBoxSoftware.Reflection.Comments {
	public sealed class ListXmlCodeElement : XmlContainerCodeElement {
		internal ListXmlCodeElement(XmlNode node)
			: base(XmlCodeElements.List) {
			this.Elements = this.Parse(node);
			this.IsBlock = true;

			// We need to record the style of bullet to use for the list
			XmlAttribute attribute = node.Attributes["bullet"];
			if (attribute == null) {
				this.BulletStyle = ListBulletStyles.Bullet;
			}
			else {
				this.BulletStyle = (ListBulletStyles)Enum.Parse(
					typeof(ListBulletStyles), attribute.Value
					);
			}
		}

		/// <summary>
		/// Checks the contents of the list to determine if it should be handled
		/// as a table or a list.
		/// </summary>
		/// <returns>True if the displayer should display a table.</returns>
		public bool IsTable() {
			bool isTable = false;
			int count = this.Elements.Count;
			for (int i = 0; i < count; i++) {
				if (this.Elements[i].Element == XmlCodeElements.ListHeader) {
					isTable = true;
					break;
				}
			}
			return isTable;
		}

		/// <summary>
		/// Gets or sets the style for bullets displayed in this list.
		/// </summary>
		public ListBulletStyles BulletStyle { get; set; }
	}
}
