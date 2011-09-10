using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages {
	using TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements;
	using TheBoxSoftware.Reflection;
	using TheBoxSoftware.Reflection.Comments;

	/// <summary>
	/// Renders Delegates in the documentation in a FlowDocument.
	/// </summary>
	public sealed class DelegatePage : Page {
		private TypeDef representedType;
		private XmlCodeCommentFile commentsXml;

		/// <summary>
		/// Initialises a new instance of the Delegate page.
		/// </summary>
		/// <param name="type">The TypeDef representing the delegate.</param>
		/// <param name="xmlComments">The XmlComments file.</param>
		public DelegatePage(TypeDef type, XmlCodeCommentFile xmlComments) {
			this.representedType = type;
			this.commentsXml = xmlComments;
		}

		public override void Generate() {
			if (!this.IsGenerated) {
				CRefPath crefPath = new CRefPath(this.representedType);
				List<Block> parsedBlocks = Elements.Parser.Parse(this.representedType.Assembly, commentsXml, crefPath);

				if (!this.commentsXml.Exists) {
					this.Blocks.Add(new NoXmlComments(this.representedType));
				}

				this.Blocks.Add(new Header1(this.representedType.GetDisplayName(false) + " Delegate"));

                // Add the summary if it exists
                if (parsedBlocks != null) {
                    Block summary = parsedBlocks.Find(currentBlock => currentBlock is Summary);
                    if (summary != null) {
                        this.Blocks.Add(summary);
                    }
                }

				this.AddSyntaxBlock(this.representedType);

				// Add the type parameters if they exist
				if (parsedBlocks != null) {
					List<Block> typeParams = parsedBlocks.FindAll(currentBlock => currentBlock is TypeParamEntry);
					if (typeParams.Count > 0) {
						TypeParamSection typeParamSection = new TypeParamSection();
						foreach (GenericTypeRef genericType in this.representedType.GenericTypes) {
							string name = genericType.Name;
							string description = string.Empty;
							foreach (TypeParamEntry current in typeParams) {
								if (current.Param == genericType.Name) {
									description = current.Description;
								}
							}
							typeParamSection.AddEntry(new TypeParamEntry(name, description));
						}
						this.Blocks.Add(typeParamSection);
					}
				}
				
				// Show the delegate parameters, this comes from the invoke method
				// Add the parameter information if available
				//List<Param> parameterComments = Parser.ParseElement<Param>(parsedBlocks);
				MethodDef invokeMethod = this.representedType.GetMethods().Find(m => m.Name == "Invoke");
				this.AddParametersForMethod(invokeMethod, parsedBlocks);

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

				this.AddSeeAlso(parsedBlocks);

				this.IsGenerated = true;
			}
		}
	}
}
