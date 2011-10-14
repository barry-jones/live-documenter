using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages {
	using TheBoxSoftware.Reflection;
	using TheBoxSoftware.Reflection.Comments;
	using TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements;

	/// <summary>
	/// Displays the details about a specified <see cref="EventDef"/>.
	/// </summary>
	public class EventPage : Page {
		private EventDef eventDef;
		private XmlCodeCommentFile xmlComments;

		/// <summary>
		/// Initialises a new instance of the EventPage class.
		/// </summary>
		/// <param name="eventDef">The event to be documented.</param>
		/// <param name="xmlComments">The xml comments associated with the defining library.</param>
		public EventPage(EventDef eventDef, XmlCodeCommentFile xmlComments) {
			this.eventDef = eventDef;
			this.xmlComments = xmlComments;
		}

		/// <summary>
		/// Generates the page
		/// </summary>
		public override void Generate() {
			if (!this.IsGenerated) {
				CRefPath crefPath = new CRefPath(eventDef);
				List<Block> parsedBlocks = Elements.Parser.Parse(this.eventDef.Type.Assembly, xmlComments, crefPath);

				if (!this.xmlComments.Exists) {
					this.Blocks.Add(new NoXmlComments(eventDef));
				}

				this.Blocks.Add(new Elements.Header1(eventDef.Name));

                // Add the summary if it exists
                if (parsedBlocks != null) {
                    Block summary = parsedBlocks.Find(currentBlock => currentBlock is Summary);
                    if (summary != null) {
                        this.Blocks.Add(summary);
                    }
                }

                this.AddSyntaxBlock(this.eventDef);		

				// Add the exception table if it exists
				if (parsedBlocks != null) {
					Block exceptions = parsedBlocks.Find(currentBlock => currentBlock is ExceptionList);
					if (exceptions != null) {
						this.Blocks.Add(exceptions);
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
