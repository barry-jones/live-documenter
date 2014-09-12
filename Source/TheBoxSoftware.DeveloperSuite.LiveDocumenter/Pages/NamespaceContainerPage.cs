using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheBoxSoftware.Documentation;
using System.Windows.Documents;
using TheBoxSoftware.Reflection.Comments;
using TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages {
	internal class NamespaceContainerPage : Page {
		private Entry associatedEntry = null;

		public NamespaceContainerPage(Entry associatedEntry) {
			this.associatedEntry = associatedEntry;
		}

		public override void Generate() {
			this.Blocks.Add(new Header1(this.associatedEntry.Name));

			SummaryTable classTable = new SummaryTable("Namespace", string.Empty, false, false);
			foreach (Entry currentNamespace in this.associatedEntry.Children) {
				CRefPath crefPath = CRefPath.Parse(string.Format("N:{0}", currentNamespace.SubKey));

				// Find the description for the type
				// Block description = this.GetSummaryFor(xmlFile, currentType.Assembly, "/doc/members/member[@name='" + crefPath + "']/summary");
				Hyperlink nameLink = new Hyperlink(new Run(currentNamespace.Name));
				nameLink.Tag = new EntryKey(currentNamespace.Key, currentNamespace.SubKey);
				nameLink.Click += new System.Windows.RoutedEventHandler(LinkHelper.Resolve);
				classTable.AddItem(nameLink, string.Empty);
			}
			this.Blocks.Add(classTable);
		}
	}
}
