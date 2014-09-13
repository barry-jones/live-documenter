using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection {
	using TheBoxSoftware.Reflection.Core;
	using TheBoxSoftware.Reflection.Core.COFF;

	public class CustomAttribute {
		private MemberRef attributeType;

		public CustomAttribute(MemberRef attributeType) {
			this.attributeType = attributeType;
		}

		public string Name {
			get { return this.attributeType.Type.Name; }
		}
	}
}
