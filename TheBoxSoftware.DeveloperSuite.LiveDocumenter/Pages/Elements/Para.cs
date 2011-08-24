using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements {
	public sealed class Para : Paragraph {
		public Para(List<Inline> elements) {
			this.Resources.MergedDictionaries.Add(DocumentationResources.BaseResources);
			this.Inlines.AddRange(elements);
		}
	}
}
