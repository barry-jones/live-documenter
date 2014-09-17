﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TheBoxSoftware.Reflection.Comments {
	public sealed class RemarksXmlCodeElement : XmlContainerCodeElement {
		internal RemarksXmlCodeElement(XmlNode node)
			: base(XmlCodeElements.Remarks) {
			this.Elements = this.Parse(node);
			this.IsBlock = true;
		}
	}
}