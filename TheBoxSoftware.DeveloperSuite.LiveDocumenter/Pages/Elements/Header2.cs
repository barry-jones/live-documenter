using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements {
	public class Header2 : Paragraph {
		public Header2(string title) : base(new Run(title)) { }
	}
}
