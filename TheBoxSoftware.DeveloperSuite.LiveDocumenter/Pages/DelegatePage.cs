using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages {
	using TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements;
	using TheBoxSoftware.Reflection;
	using TheBoxSoftware.Reflection.Comments;

	public sealed class DelegatePage : Page {
		private TypeDef representedType;
		private XmlCodeCommentFile commentsXml;

		public DelegatePage(TypeDef type, XmlCodeCommentFile xmlComments) {
			this.representedType = type;
			this.commentsXml = xmlComments;
		}

		public override void Generate() {
			if (!this.IsGenerated) {
				CRefPath crefPath = new CRefPath(this.representedType);
				List<Block> parsedBlocks = Elements.Parser.Parse(this.representedType.Assembly, commentsXml, crefPath);

				this.Blocks.Add(new Header1(this.representedType.GetDisplayName(false) + " Delegate"));

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
				
				// Show the delegate parameters, this comes from the invoke method
				// Add the parameter information if available
				List<Param> parameterComments = Parser.ParseElement<Param>(parsedBlocks);
				MethodDef invokeMethod = this.representedType.GetMethods().Find(m => m.Name == "Invoke");
				if (invokeMethod != null) {
					if (invokeMethod.Parameters != null && invokeMethod.Parameters.Count > 0) {
						ParameterList parameters = null;
						foreach (ParamDef methodParam in invokeMethod.Parameters) {
							if (methodParam.Sequence != 0) {
								// Find the parameter comments
								Param paramComment = null;
								foreach (Param current in parameterComments) {
									if (current.Name == methodParam.Name) {
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
											typeKey = new EntryKey(Helper.GetUniqueKey(typeRef.Assembly, typeRef));
										}
									}
									else {
										typeKey = new CrefEntryKey(typeRef.Assembly, new CRefPath(typeRef).ToString());
									}
									typeName = typeRef.GetDisplayName(false);
								}
								List<Block> description = new List<Block>();
								if (paramComment != null) {
									description = paramComment.Description;
								}
								parameters.Add(methodParam.Name, typeName, invokeMethod.Assembly, typeKey, description);
							}
						}
						if (parameters != null) {
							this.Blocks.Add(parameters);
						}
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

				this.IsGenerated = true;
			}
		}
	}
}
