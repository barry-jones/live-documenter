using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements {
	public class Header1 : Paragraph {
		public Header1() { }
		public Header1(string header) : base(new Run(header)) { }
	}
}
