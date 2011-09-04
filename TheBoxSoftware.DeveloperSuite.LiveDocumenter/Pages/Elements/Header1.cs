using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements {
	public class Header1 : Paragraph {
		/// <summary>
		/// Initialises a new instance of the Header1 class.
		/// </summary>
		public Header1() {
			this.Initialise();
		}

		/// <summary>
		/// Initialises a new instance of the Header1 class.
		/// </summary>
		/// <param name="header">The header text.</param>
		public Header1(string header) : base(new Run(header)) {
			this.Initialise();
		}

		private void Initialise() {
			this.Resources.MergedDictionaries.Add(DocumentationResources.BaseResources);
		}
	}
}
