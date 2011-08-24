using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements {
	/// <summary>
	/// Represents a link to a type or member that is referenced in the
	/// current compilation environment. This relates to the see xml code
	/// comment.
	/// </summary>
	public sealed class See : Hyperlink {
		public See(CrefEntryKey key, string name)
			: base() {
			this.Resources.MergedDictionaries.Add(DocumentationResources.BaseResources);
			this.Inlines.Add(new Run(name));
			this.Tag = key;
			this.Click += new System.Windows.RoutedEventHandler(LinkHelper.Resolve);
		}
	}
}
