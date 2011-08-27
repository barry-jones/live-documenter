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
	/// A page that constructs and displays the comments and details for
	/// a <see cref="MethodDef"/> instance.
	/// </summary>
	public class MethodPage : Page {
		private MethodDef method;
		private XmlCodeCommentFile commentsXml;

		/// <summary>
		/// Initialises a new MethodPage class
		/// </summary>
		/// <param name="method">The method this page is to document</param>
		/// <param name="commentsXml">The comments document</param>
		public MethodPage(MethodDef method, XmlCodeCommentFile commentsXml)
			: base() {
			this.method = method;
			this.commentsXml = commentsXml;
		}

		/// <summary>
		/// Generates the page contents
		/// </summary>
		public override void Generate() {
			if (!this.IsGenerated) {
				CRefPath crefPath = new CRefPath(method);
				List<Block> parsedBlocks = Elements.Parser.Parse(this.method.Assembly, commentsXml, crefPath);

				this.Blocks.Add(new Elements.Header1(method.GetDisplayName(false)));

                // Add the summary if it exists
                if (parsedBlocks != null) {
                    Block summary = parsedBlocks.Find(currentBlock => currentBlock is Summary);
                    if (summary != null) {
                        this.Blocks.Add(summary);
                    }
                }

				//Diagramming.SequenceDiagram diagram = new Diagramming.SequenceDiagram(
				//    new Model.Diagram.SequenceDiagram.SequenceDiagram(method)
				//    );
				//diagram.Height = 300;
				//BlockUIContainer diagramContainer = new BlockUIContainer(diagram);
				//this.Blocks.Add(diagramContainer);

				this.AddSyntaxBlock(this.method);

				// Add the type parameters if they exist
				if (parsedBlocks != null) {
					List<Block> typeParams = parsedBlocks.FindAll(currentBlock => currentBlock is TypeParamEntry);
					if (typeParams.Count > 0) {
						TypeParamSection typeParamSection = new TypeParamSection();
						foreach (GenericTypeRef genericType in this.method.GenericTypes) {
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

				// Add the parameter information if available
				List<Param> parameterComments = Parser.ParseElement<Param>(parsedBlocks);
				if (this.method.Parameters != null && this.method.Parameters.Count > 0) {
					ParameterList parameters = null;
					foreach (ParamDef methodParam in this.method.Parameters) {
						if (methodParam.Sequence != 0) {
							// Find the parameter comments
							Param paramComment = null;
							foreach (Param current in parameterComments) {
								if(current.Name == methodParam.Name) {
									paramComment = current;
									break;
								}
							}
							
							TypeRef typeRef = methodParam.GetTypeRef();
							EntryKey typeKey = null;
							string typeName = string.Empty;
							if (parameters == null) { parameters = new ParameterList(); }
							if (typeRef != null) {
								if (!typeRef.IsExternalReference) {
									if (typeRef is GenericTypeRef) {
										typeKey = null;
									}
									else {
										typeKey = new EntryKey(typeRef.GetGloballyUniqueId());
									}
								}
								else {
									typeKey = new CrefEntryKey(typeRef.Assembly, new CRefPath(typeRef).ToString());
								}
								typeName = typeRef.GetDisplayName(false);
							}
							List<Block> paramDescription = new List<Block>();
							if (paramComment != null && paramComment.Description != null) {
								paramDescription = paramComment.Description;
							}
							parameters.Add(methodParam.Name, typeName, method.Assembly, typeKey, paramDescription);
						}
					}
					if (parameters != null) {
						this.Blocks.Add(parameters);
					}
				}
				
				// add the returns comment if it is found
				if (parsedBlocks != null) {
					Block returns = parsedBlocks.Find(currentBlock => currentBlock is Returns);
					if (returns != null) {
						this.Blocks.Add(returns);
					}
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