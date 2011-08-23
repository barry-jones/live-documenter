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
	/// A page that displays all of the fields that are available in a type
	/// </summary>
	public sealed class TypeFieldsPage : Page {
		private List<FieldDef> fields;
		private XmlCodeCommentFile xmlComments;

		/// <summary>
		/// Initialises a new instance of the TypeFieldsPage class
		/// </summary>
		/// <param name="fields">The fields to manage</param>
		/// <param name="xmlComments">The xml comments</param>
		public TypeFieldsPage(List<FieldDef> fields, XmlCodeCommentFile xmlComments) {
			this.fields = fields;
			this.xmlComments = xmlComments;
		}

		/// <summary>
		/// Generates the pages contents
		/// </summary>
		public override void Generate() {
			if (!this.IsGenerated) {
				if (fields.Count > 0) {
					this.Blocks.Add(new Header1(fields[0].Type.GetDisplayName(false) + " Fields"));
				}
				XmlCodeCommentFile comments = this.xmlComments.GetReusableFile();

				if (this.fields != null && this.fields.Count > 0) {
					SummaryTable displayedFields = new SummaryTable();

					var sortedFields = from field in this.fields
									   orderby field.Name
									   select field;
					foreach (FieldDef currentField in sortedFields) {
						CRefPath crefPath = new CRefPath(currentField);
						System.Windows.Documents.Hyperlink link = new System.Windows.Documents.Hyperlink();
						link.Inlines.Add(new System.Windows.Documents.Run(currentField.Name));
						link.Tag = new EntryKey(currentField.GetGloballyUniqueId());
						link.Click += new System.Windows.RoutedEventHandler(LinkHelper.Resolve);

						// First we check if there is a summary for the field, then if not we check for a
						// definition of value and use that if it is defined.
						Block summary = null;
						XmlCodeComment comment = comments.ReadComment(
							"/doc/members/member[@name='" + crefPath.ToString() + "']/summary"
							);
						List<Block> parsedBlocks = Elements.Parser.Parse(currentField.Assembly, comment);
						if (parsedBlocks != null && parsedBlocks.Count > 0) {
							summary = parsedBlocks[0];
						}
						else {
							XmlCodeComment value = comments.ReadComment(
								"/doc/members/member[@name='" + crefPath.ToString() + "']/value"
								);
							parsedBlocks = Elements.Parser.Parse(currentField.Assembly, value);
							if (parsedBlocks != null && parsedBlocks.Count > 0) {
								summary = parsedBlocks[0];
							}
						}

						displayedFields.AddItem(link, summary, Model.ElementIconConstants.GetIconPathFor(currentField));
					}
					this.Blocks.Add(displayedFields);
				}

				this.IsGenerated = true;
			}
		}
	}
}
