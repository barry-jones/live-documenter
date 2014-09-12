using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Model {
	/// <summary>
	/// Manages a collection of PrivacyFilters for UI selection elements.
	/// </summary>
	/// <seealso cref="PrivacyFilter"/>
	internal class PrivacyFilterCollection : ObservableCollection<PrivacyFilter> {
		/// <summary>
		/// Returns a displayable string representation of this the collection.
		/// </summary>
		/// <returns>A string.</returns>
		public override string ToString() {
			List<string> selectedNames = new List<string>();

			foreach (PrivacyFilter current in this) {
				if (current.IsSelected) {
					selectedNames.Add(current.Visibility.ToString());
				}
			}

			return selectedNames.Count > 0
				? selectedNames.Count == this.Count ? "Document all members" : string.Format("Document Public, {0} members", string.Join(", ", selectedNames.ToArray()))
				: "Document Public members";
		}

		/// <summary>
		/// Sets the <see cref="PrivacyFilter.IsSelected"/> property of all of the provided
		/// <paramref name="filters"/>.
		/// </summary>
		/// <param name="filters">The filters to select</param>
		public void SetFilters(List<Reflection.Visibility> filters) {
			// set the currently selected filters
			foreach (Reflection.Visibility filter in filters) {
				PrivacyFilter p = this.ToList().Find(c => c.Visibility == filter);
				if (p != null) {
					p.IsSelected = true;
				}
			}
		}

		/// <summary>
		/// Returns a List&lt&gt; of the selected Visibility filters.
		/// </summary>
		/// <returns>A list of selected filters.</returns>
		public List<Reflection.Visibility> GetFilters() {
			List<Reflection.Visibility> filters = new List<Reflection.Visibility>();
			foreach (PrivacyFilter filter in this) {
				if (filter.IsSelected) filters.Add(filter.Visibility);
			}
			return filters;
		}
	}
}
