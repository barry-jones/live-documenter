using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements {
	/// <summary>
	/// Represents a section of text that is to be displayed as code
	/// in the document. Currently this relates to the c and code xml
	/// comment types.
	/// </summary>
	public sealed class Code : Paragraph {
		public Code() {
			this.Initialise();
		}
		public Code(string code) {
			this.Initialise();
			this.Inlines.Add(new Run(code));
		}

		private void Initialise() {
			ResourceDictionary dict = new ResourceDictionary();
			Uri uri = new Uri("../Resources/DefaultDocumentationStyle.xaml", UriKind.Relative);
			dict.Source = uri;
			this.Resources.MergedDictionaries.Add(dict);

			this.Style = (Style)this.FindResource("Code");
		}
	}
}
