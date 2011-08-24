using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements {
	/// <summary>
	/// 
	/// </summary>
	public sealed class Header3 : Paragraph {
		public Header3() {
			this.Initialise();
		}

		public Header3(string title) : base(new Run(title)) {
			this.Initialise();
		}

		private void Initialise() {
			this.Resources.MergedDictionaries.Add(DocumentationResources.BaseResources);
		}
	}
}
