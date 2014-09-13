using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Model {
	/// <summary>
	/// The PrivacyFilter is used by the PrivacyFilterCollection to handle the display and selection
	/// of filters in the UI.
	/// </summary>
	/// <seealso cref="PrivacyFilterCollection"/>
	internal sealed class PrivacyFilter {
		/// <summary>
		/// Initialises a new instance of the PrivacyFilter class.
		/// </summary>
		/// <param name="title">The display title of the PrivacyFilter</param>
		/// <param name="filter">The represented filter.</param>
		public PrivacyFilter(string title, TheBoxSoftware.Reflection.Visibility filter) {
			this.Title = title;
			this.Visibility = filter;
		}

		/// <summary>
		/// Gets the display name of this filter.
		/// </summary>
		public string Title { get; private set; }

		/// <summary>
		/// Gets the represented visibility
		/// </summary>
		public TheBoxSoftware.Reflection.Visibility Visibility { get; private set; }

		/// <summary>
		/// Gets a value indicating if this PrivacyFilter has been selected.
		/// </summary>
		public bool IsSelected { get; set; }
	}
}
