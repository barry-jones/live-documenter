using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Documents;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages {
	using TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements;
	using TheBoxSoftware.Reflection;
	using TheBoxSoftware.Reflection.Comments;

	/// <summary>
	/// A page to describe and display the properties for a single TypeDef.
	/// </summary>
	public class TypePropertiesPage : Page {
		private List<PropertyDef> properties;
		private XmlCodeCommentFile xmlComments;

		/// <summary>
		/// Initialises a new instance of the TypePropertiesPage class.
		/// </summary>
		/// <param name="properties">The properties to display.</param>
		/// <param name="xmlComments">The assemblies xml comments file</param>
		public TypePropertiesPage(List<PropertyDef> properties, XmlCodeCommentFile xmlComments) {
			this.properties = properties;
			this.xmlComments = xmlComments;
		}

		/// <summary>
		/// Generates the pages contents.
		/// </summary>
		public override void Generate() {
			if (!this.IsGenerated) {
				TypeDef definingType = null;
				if (this.properties != null && this.properties.Count > 0) {
					definingType = (TypeDef)this.properties[0].Type;
				}
				XmlCodeCommentFile comments = this.xmlComments.GetReusableFile();

				this.Blocks.Add(new Header1(definingType.GetDisplayName(false) + " Properties"));

				if (this.properties != null && this.properties.Count > 0) {
					SummaryTable methods = new SummaryTable();

					var sortedProperties = from property in this.properties
										   orderby property.GetDisplayName(false, true)
										   select property;
					foreach (PropertyDef currentProperty in sortedProperties) {
						CRefPath path = new CRefPath(currentProperty);
						System.Windows.Documents.Hyperlink link = new System.Windows.Documents.Hyperlink();
						link.Inlines.Add(new System.Windows.Documents.Run(currentProperty.GetDisplayName(false, true)));
						link.Tag = new EntryKey(Helper.GetUniqueKey(currentProperty.Type.Assembly, currentProperty));
						link.Click += new System.Windows.RoutedEventHandler(LinkHelper.Resolve);

						Block description = this.GetSummaryFor(comments,
							currentProperty.Type.Assembly,
							"/doc/members/member[@name='" + path.ToString() + "']/summary");

						methods.AddItem(link, description, Model.ElementIconConstants.GetIconPathFor(currentProperty));
					}
					this.Blocks.Add(methods);
				}

				this.IsGenerated = true;
			}
		}
	}
}
