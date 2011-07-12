using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Xml;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages {
	using TheBoxSoftware.Reflection;
	using TheBoxSoftware.Reflection.Comments;
	using TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements;

	/// <summary>
	/// A Page to display information about a namespace in the LiveDocumentor
	/// </summary>
	public class NamespacePage : Page {
		private KeyValuePair<string, List<TypeDef>> item;
		private XmlCodeCommentFile commentsXml;

		#region Constructors
		/// <summary>
		/// Initialises a new instance of the NamespacePage class
		/// </summary>
		/// <param name="item">The namespace details as a list of methods</param>
		/// <param name="commentsXml">The code comments file to get comments from</param>
		public NamespacePage(KeyValuePair<string, List<TypeDef>> item, XmlCodeCommentFile commentsXml) {
			this.item = item;
			this.commentsXml = commentsXml;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Generates the contents of the page
		/// </summary>
		public override void Generate() {
			if (!this.IsGenerated) {
				XmlCodeCommentFile xmlFile = commentsXml.GetReusableFile();

				this.Blocks.Add(new Header1(item.Key + " Namespace"));

				// Add class diagram

				// Add the table of classes to the page
				SummaryTable classTable = new SummaryTable();
				var sortedTypes = from type in item.Value
								  orderby type.Name
								  select type;
				foreach (TypeDef currentType in sortedTypes) {
					CRefPath crefPath = new CRefPath(currentType);

					// Find the description for the type
					Block description = this.GetSummaryFor(xmlFile, currentType.Assembly, "/doc/members/member[@name='" + crefPath + "']/summary");
					Hyperlink nameLink = new Hyperlink(new Run(currentType.GetDisplayName(false)));
					nameLink.Tag = new EntryKey(Helper.GetUniqueKey(currentType.Assembly, currentType));
					nameLink.Click += new System.Windows.RoutedEventHandler(LinkHelper.Resolve);
					classTable.AddItem(nameLink, description, Model.ElementIconConstants.GetIconPathFor(currentType));
				}
				this.Blocks.Add(classTable);

				this.IsGenerated = true;
			}
		}
		#endregion
	}
}
