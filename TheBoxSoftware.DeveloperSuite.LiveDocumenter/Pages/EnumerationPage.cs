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
	/// A page that describes and displays information about an
	/// <see cref="Enum"/> derived type.
	/// </summary>
	public class EnumerationPage : Page {
		private TypeDef representedType;
		private XmlCodeCommentFile commentsXml;

		/// <summary>
		/// Initialises a new instance of the EnumerationPage class.
		/// </summary>
		/// <param name="type">The type to display in the page</param>
		/// <param name="xmlComments">The xml comments document for the assembly</param>
		public EnumerationPage(TypeDef type, XmlCodeCommentFile xmlComments) {
			this.representedType = type;
			this.commentsXml = xmlComments;
		}

		/// <summary>
		/// Generates the contents of the page, utilising details of the
		/// type and its associated xml comments.
		/// </summary>
		public override void Generate() {
			if (!this.IsGenerated) {
				CRefPath crefPath = new CRefPath(this.representedType);
				XmlCodeCommentFile xmlComments = this.commentsXml.GetReusableFile();
				List<Block> parsedBlocks = Elements.Parser.Parse(this.representedType.Assembly, xmlComments, crefPath);

				this.Blocks.Add(new Header1(this.representedType.Name + " Enumeration"));

                // Add the summary if it exists
                if (parsedBlocks != null) {
                    Block summary = parsedBlocks.Find(currentBlock => currentBlock is Summary);
                    if (summary != null) {
                        this.Blocks.Add(summary);
                    }
                }

                this.AddNamespace(this.representedType);
				this.AddSyntaxBlock(this.representedType);

				// Add the remarks if it exists
				if (parsedBlocks != null) {
					Block remarks = parsedBlocks.Find(currentBlock => currentBlock is Remarks);
					if (remarks != null) {
						this.Blocks.Add(remarks);
					}
				}

				// Add the table of classes to the page
				List<FieldDef> fields = this.representedType.GetFields();
				SummaryTable classTable = new SummaryTable("Member Name", "Description", false);
				var sortedFields = from field in fields
								   orderby field.Name
								   select field;
				foreach (FieldDef currentField in sortedFields) {
					if (currentField.IsSystemGenerated) {
						continue;
					}
					Block description = this.GetSummaryFor(xmlComments, currentField.Assembly, "/doc/members/member[@name='" + new CRefPath(currentField).ToString() + "']/summary");
					classTable.AddItem(currentField.Name, description);					
				}
				this.Blocks.Add(new Header2("Members"));
				this.Blocks.Add(classTable);

				// Add the inheritance tree
				this.AddInheritanceTree(this.representedType);

				// Add the seealso list if it exists
				this.AddSeeAlso(parsedBlocks);

				this.IsGenerated = true;
			}
		}
	}
}
