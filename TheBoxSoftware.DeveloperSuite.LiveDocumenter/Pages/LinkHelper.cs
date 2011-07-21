using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages {
	using TheBoxSoftware.Documentation;

	/// <summary>
	/// Helps links in the flow document resolve out to an item in the
	/// document map.
	/// </summary>
	internal static class LinkHelper {
		/// <summary>
		/// Resolves a hyperlink to a treenode in the document map
		/// </summary>
		/// <param name="sender">The source of the event</param>
		/// <param name="e">The event arguments</param>
		public static void Resolve(object sender, System.Windows.RoutedEventArgs e) {
			if (e.Source is System.Windows.Documents.Hyperlink) {
				System.Windows.Documents.Hyperlink sourceLink = e.Source as System.Windows.Documents.Hyperlink;
				LiveDocument document = LiveDocumentorFile.Singleton.LiveDocument;
				DocumentMap items = document.DocumentMap;
				Entry entry = null;
				sourceLink.Cursor = Cursors.Wait;

				foreach (Entry item in items) {
					EntryKey key = null;
					if (sourceLink.Tag is CrefEntryKey) {
						key = Helper.ResolveTypeAndGetUniqueIdFromCref((CrefEntryKey)sourceLink.Tag);
					}
					else if (sourceLink.Tag is EntryKey) {
						key = (EntryKey)sourceLink.Tag;
					}
					if (key != null) {
						entry = item.FindByKey(key.Key, key.SubKey);
						if (entry != null) {
							break;
						}
					}
				}
				if (entry != null && entry.Parent != null) {
					entry.IsSelected = true;
					entry.Parent.IsExpanded = true;
				}

				sourceLink.Cursor = null;
			}
		}
	}
}
