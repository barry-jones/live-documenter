using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements {
	using TheBoxSoftware.Reflection.Comments;

	/// <summary>
	/// This is a container class for all list xml code elements. There is
	/// a List class as well but this is to be used internally by this class
	/// when the display of it is in list form.
	/// </summary>
	public class ListXmlElement : Section {
		private ListXmlCodeElement listElement;

		public ListXmlElement(ListXmlCodeElement listElement) {
			this.listElement = listElement;
		}
	}
}
