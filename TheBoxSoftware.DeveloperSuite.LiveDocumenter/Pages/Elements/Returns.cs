﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements {
	/// <summary>
	/// This refers to a comment about the return value for the current
	/// element in the current document. This relates to the returns xml
	/// code comment element.
	/// </summary>
	public sealed class Returns : Section {
		public Returns(List<Block> children) {
			this.Blocks.Add(new Header3("Returns"));
			this.Blocks.AddRange(children);
		}
	}
}