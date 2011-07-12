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
	/// A Page that describes an individual Type in the LiveDocumentor
	/// </summary>
	public sealed class TypePage : Page {
		private TypeDef representedType;
		private XmlCodeCommentFile commentsXml;

		#region Constructors
		/// <summary>
		/// Initialises a new TypePage instance
		/// </summary>
		/// <param name="type">The type this page is to document</param>
		/// <param name="commentsXml">The code comments file to read the comments from</param>
		public TypePage(TypeDef type, XmlCodeCommentFile commentsXml) {
			this.representedType = type;
			this.commentsXml = commentsXml;
		}
		#endregion

		/// <summary>
		/// Generates the pages contents
		/// </summary>
		public override void Generate() {
			if (!this.IsGenerated) {
				CRefPath crefPath = new CRefPath(this.representedType);
				List<Block> parsedBlocks = Elements.Parser.Parse(this.representedType.Assembly, commentsXml, crefPath);

				string classType = this.representedType.IsInterface ? " Interface" : " Class";
				this.Blocks.Add(new Header1(this.representedType.GetDisplayName(false) + classType));

                // Add the summary if it exists
                if (parsedBlocks != null) {
                    Block summary = parsedBlocks.Find(currentBlock => currentBlock is Summary);
                    if (summary != null) {
                        this.Blocks.Add(summary);
                    }
                }

                this.AddNamespace(this.representedType);
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

				// Add the remarks if it exists
				if (parsedBlocks != null) {
					Block remarks = parsedBlocks.Find(currentBlock => currentBlock is Remarks);
					if (remarks != null) {
						this.Blocks.Add(remarks);
					}
				}

				// Add the inheritance tree
				this.AddInheritanceTree(this.representedType);

				// Add the seealso list if it exists
				this.AddSeeAlso(parsedBlocks);

				// Inform the application the page has been generated
				this.IsGenerated = true;
			}
		}
	}
}