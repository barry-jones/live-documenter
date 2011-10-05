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

	public class PropertyPage : Page {
		private PropertyDef property;
		private XmlCodeCommentFile xmlComments;

		public PropertyPage(PropertyDef property, XmlCodeCommentFile xmlComments) {
			this.property = property;
			this.xmlComments = xmlComments;
		}

		public override void Generate() {
			if (!this.IsGenerated) {
				CRefPath crefPath = new CRefPath(property);
				List<Block> parsedBlocks = Elements.Parser.Parse(this.property.Type.Assembly, xmlComments, crefPath);

				if (!this.xmlComments.Exists) {
					this.Blocks.Add(new NoXmlComments(property));
				}

				this.Blocks.Add(new Elements.Header1(property.GetDisplayName(false, true)));

                // Add the summary if it exists
                if (parsedBlocks != null) {
                    Block summary = parsedBlocks.Find(currentBlock => currentBlock is Summary);
                    if (summary != null) {
                        this.Blocks.Add(summary);
                    }
                }

                this.AddSyntaxBlock(this.property);

				// add parameters for indexers
				if(this.property.IsIndexer) {
					this.AddParametersForMethod(this.property.GetMethod != null ? this.property.GetMethod : this.property.SetMethod, parsedBlocks);
				}

				// Add the exception table if it exists
				if (parsedBlocks != null) {
					Block exceptions = parsedBlocks.Find(currentBlock => currentBlock is ExceptionList);
					if (exceptions != null) {
						this.Blocks.Add(exceptions);
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
