using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheBoxSoftware.Reflection;
using TheBoxSoftware.Reflection.Comments;

namespace TheBoxSoftware.Documentation.Exporting.Rendering {
	class NamespaceXmlRenderer : XmlRenderer {
		private List<TypeDef> member;
		private XmlCodeCommentFile xmlComments;

		public NamespaceXmlRenderer(List<TypeDef> container, XmlCodeCommentFile xmlComments, Entry associatedEntry) {
			this.member = container;
			this.xmlComments = xmlComments;
			this.AssociatedEntry = associatedEntry;
		}

		public override void Render(System.Xml.XmlWriter writer) {
			throw new NotImplementedException();
		}
	}
}
