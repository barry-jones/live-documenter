using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements {
	/// <summary>
	/// Represents a section of text that is to be displayed as code
	/// in the document. Currently this relates to the c and code xml
	/// comment types.
	/// </summary>
	public sealed class Code : Paragraph {
		public Code() {
		}
		public Code(string code) {
			this.Inlines.Add(new Run(code));
		}
	}
}
