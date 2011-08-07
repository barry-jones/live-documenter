using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements {
	/// <summary>
	/// Represents a code formatted text that appears inline in the same
	/// run it is defined in the current document. This refers to the c
	/// code element in the code comments.
	/// </summary>
	public sealed class C : Run {
		public C(string code)
			: base(code) {
				this.Initialise();
		}

		private void Initialise() {
			ResourceDictionary dict = new ResourceDictionary();
			Uri uri = new Uri("../Resources/DefaultDocumentationStyle.xaml", UriKind.Relative);
			dict.Source = uri;
			this.Resources.MergedDictionaries.Add(dict);

			this.Style = (Style)this.FindResource("C");
		}
	}
}
