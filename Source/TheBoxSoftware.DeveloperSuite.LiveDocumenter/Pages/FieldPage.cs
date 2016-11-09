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
	/// A page that details and describes a field present in a type
	/// </summary>
	public class FieldPage : Page {
		private FieldDef field;
		private XmlCodeCommentFile xmlComments;

		/// <summary>
		/// Initialises a new FieldPage class.
		/// </summary>
		/// <param name="field">The field this page rerpresents.</param>
		/// <param name="xmlComments">The xml comments file for the library.</param>
		public FieldPage(FieldDef field, XmlCodeCommentFile xmlComments) {
			this.field = field;
			this.xmlComments = xmlComments;
		}

		/// <summary>
		/// Generates the pages contents
		/// </summary>
		public override void Generate() {
			if (!this.IsGenerated) {
				CRefPath crefPath = new CRefPath(field);
				List<Block> parsedBlocks = Elements.Parser.Parse(this.field.Type.Assembly, xmlComments, crefPath);

				if (!this.xmlComments.Exists()) {
					this.Blocks.Add(new NoXmlComments(field));
				}

				this.Blocks.Add(new Header1(field.Name));

                // Add the summary if it exists
                if (parsedBlocks != null) {
                    Block summary = parsedBlocks.Find(currentBlock => currentBlock is Summary);
                    if (summary != null) {
                        this.Blocks.Add(summary);
                    }
                }

                this.AddSyntaxBlock(this.field);

				// Add the remarks if it exists
				if (parsedBlocks != null) {
					Block value = parsedBlocks.Find(currentBlock => currentBlock is Value);
					if (value != null) {
						this.Blocks.Add(value);
					}
				}

				if(parsedBlocks != null) {
					Block permissions = parsedBlocks.Find(current => current is PermissionList);
					if(permissions != null) {
						this.Blocks.Add(permissions);
					}
				}

				// Add the remarks if it exists
				if (parsedBlocks != null) {
					Block remarks = parsedBlocks.Find(currentBlock => currentBlock is Remarks);
					if (remarks != null) {
						this.Blocks.Add(remarks);
					}
				}

				// Add the example if it exists
				if (parsedBlocks != null) {
					Block summary = parsedBlocks.Find(currentBlock => currentBlock is Example);
					if (summary != null) {
						this.Blocks.Add(new Header2("Examples"));
						this.Blocks.Add(summary);
					}
				}

				// Add the seealso list if it exists
				this.AddSeeAlso(parsedBlocks);

				this.IsGenerated = true;
			}
		}
	}
}
