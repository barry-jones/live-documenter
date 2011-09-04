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

				if (!this.representedType.IsInterface && !this.representedType.IsStructure) {
					this.AddInheritanceTree(this.representedType);
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

				this.OutputMembersLists();

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

				// Inform the application the page has been generated
				this.IsGenerated = true;
			}
		}

		/// <summary>
		/// Outputs the members tables.
		/// </summary>
		private void OutputMembersLists() {
			SummaryTable members;
			CRefPath crefPath;			
			XmlCodeCommentFile xmlFile = this.commentsXml.GetReusableFile();
			List<Block> tempContainer = new List<Block>();

			var constructors = from method in this.representedType.GetConstructors()
							   orderby method.Name
							   where !LiveDocumentorFile.Singleton.LiveDocument.IsMemberFiltered(method)
							   select method;
			if (constructors != null && constructors.Count() > 0) {
				tempContainer.Add(new Header2("Constructors"));
				members = new SummaryTable();
				foreach (MethodDef currentMethod in constructors) {
					crefPath = new CRefPath(currentMethod);
					System.Windows.Documents.Hyperlink link = new System.Windows.Documents.Hyperlink();
					link.Inlines.Add(new System.Windows.Documents.Run(currentMethod.GetDisplayName(false)));
					link.Tag = new EntryKey(currentMethod.GetGloballyUniqueId());
					link.Click += new System.Windows.RoutedEventHandler(LinkHelper.Resolve);

					Block description = this.GetSummaryFor(xmlFile,
						currentMethod.Assembly,
						"/doc/members/member[@name='" + crefPath.ToString() + "']/summary");

					members.AddItem(link, description, Model.ElementIconConstants.GetIconPathFor(currentMethod));
				}
				tempContainer.Add(members);
			}

			var fields = from field in this.representedType.GetFields()
						 orderby field.Name
						 where !LiveDocumentorFile.Singleton.LiveDocument.IsMemberFiltered(field)
						 select field;
			if (fields != null && fields.Count() > 0) {
				tempContainer.Add(new Header2("Fields"));
				members = new SummaryTable();
				foreach (FieldDef currentField in fields) {
					crefPath = new CRefPath(currentField);
					System.Windows.Documents.Hyperlink link = new System.Windows.Documents.Hyperlink();
					link.Inlines.Add(new System.Windows.Documents.Run(currentField.Name));
					link.Tag = new EntryKey(currentField.GetGloballyUniqueId());
					link.Click += new System.Windows.RoutedEventHandler(LinkHelper.Resolve);

					Block description = this.GetSummaryFor(xmlFile,
						currentField.Assembly,
						"/doc/members/member[@name='" + crefPath.ToString() + "']/summary");
					Block value = this.GetSummaryFor(xmlFile,
						currentField.Assembly,
						"/doc/members/member[@name='" + crefPath.ToString() + "']/value");
					if (description != null) {
						members.AddItem(link, description, Model.ElementIconConstants.GetIconPathFor(currentField));
					}
					else {
						members.AddItem(link, value, Model.ElementIconConstants.GetIconPathFor(currentField));
					}
				}
				tempContainer.Add(members);
			}

			var properties = from property in this.representedType.GetProperties()
							 orderby property.GetDisplayName(false, true)
							 where !LiveDocumentorFile.Singleton.LiveDocument.IsMemberFiltered(property)
							 select property;
			if (properties != null && properties.Count() > 0) {
				tempContainer.Add(new Header2("Properties"));
				members = new SummaryTable();
				foreach (PropertyDef currentProperty in properties) {
					crefPath = new CRefPath(currentProperty);
					System.Windows.Documents.Hyperlink link = new System.Windows.Documents.Hyperlink();
					link.Inlines.Add(new System.Windows.Documents.Run(currentProperty.GetDisplayName(false, true)));
					link.Tag = new EntryKey(currentProperty.GetGloballyUniqueId());
					link.Click += new System.Windows.RoutedEventHandler(LinkHelper.Resolve);

					Block description = this.GetSummaryFor(xmlFile, currentProperty.Type.Assembly, "/doc/members/member[@name='" + crefPath.ToString() + "']/summary");
					members.AddItem(link, description, Model.ElementIconConstants.GetIconPathFor(currentProperty));
				}
				tempContainer.Add(members);
			}

			var events = from c in this.representedType.GetEvents()
						 orderby c.Name
						 where !LiveDocumentorFile.Singleton.LiveDocument.IsMemberFiltered(c)
						 select c;
			if (events != null && events.Count() > 0) {
				tempContainer.Add(new Header2("Events"));
				members = new SummaryTable();
				foreach (EventDef currentEvent in events) {
					crefPath = new CRefPath(currentEvent);
					System.Windows.Documents.Hyperlink link = new System.Windows.Documents.Hyperlink();
					link.Inlines.Add(new System.Windows.Documents.Run(currentEvent.Name));
					link.Tag = new EntryKey(currentEvent.GetGloballyUniqueId());
					link.Click += new System.Windows.RoutedEventHandler(LinkHelper.Resolve);

					Block description = this.GetSummaryFor(xmlFile, currentEvent.Type.Assembly, "/doc/members/member[@name='" + crefPath.ToString() + "']/summary");
					members.AddItem(link, description, Model.ElementIconConstants.GetIconPathFor(currentEvent));
				}
				tempContainer.Add(members);
			}

			var methods = from method in this.representedType.GetMethods()
						  orderby method.Name
						  where !LiveDocumentorFile.Singleton.LiveDocument.IsMemberFiltered(method)
						  select method;
			if (methods != null && methods.Count() > 0) {
				tempContainer.Add(new Header2("Methods"));
				members = new SummaryTable();				
				foreach (MethodDef currentMethod in methods) {
					crefPath = new CRefPath(currentMethod);
					System.Windows.Documents.Hyperlink link = new System.Windows.Documents.Hyperlink();
					link.Inlines.Add(new System.Windows.Documents.Run(currentMethod.GetDisplayName(false)));
					link.Tag = new EntryKey(currentMethod.GetGloballyUniqueId());
					link.Click += new System.Windows.RoutedEventHandler(LinkHelper.Resolve);

					Block description = this.GetSummaryFor(xmlFile, currentMethod.Assembly, "/doc/members/member[@name='" + crefPath.ToString() + "']/summary");

					members.AddItem(link, description, Model.ElementIconConstants.GetIconPathFor(currentMethod));
				}
				tempContainer.Add(members);
			}

			var operators = from method in this.representedType.GetOperators()
								orderby method.Name
								where !LiveDocumentorFile.Singleton.LiveDocument.IsMemberFiltered(method)
								select method;
			if (operators != null && operators.Count() > 0) {
				tempContainer.Add(new Header2("Operators"));
				members = new SummaryTable();				
				foreach (MethodDef currentMethod in operators) {
					crefPath = new CRefPath(currentMethod);
					System.Windows.Documents.Hyperlink link = new System.Windows.Documents.Hyperlink();
					link.Inlines.Add(new System.Windows.Documents.Run(currentMethod.GetDisplayName(false)));
					link.Tag = new EntryKey(currentMethod.GetGloballyUniqueId());
					link.Click += new System.Windows.RoutedEventHandler(LinkHelper.Resolve);

					Block description = this.GetSummaryFor(xmlFile, currentMethod.Assembly, "/doc/members/member[@name='" + crefPath.ToString() + "']/summary");

					members.AddItem(link, description, Model.ElementIconConstants.GetIconPathFor(currentMethod));
				}
				tempContainer.Add(members);
			}

			if (this.representedType != null && this.representedType.ExtensionMethods.Count > 0) {
				members = new SummaryTable();

				var sortedMethods = from method in this.representedType.ExtensionMethods
									where !method.IsConstructor
									orderby method.Name
									where !LiveDocumentorFile.Singleton.LiveDocument.IsMemberFiltered(method)
									select method;
				foreach (MethodDef currentMethod in sortedMethods) {
					DisplayNameSignitureConvertor displayNameSig = new DisplayNameSignitureConvertor(currentMethod, false, true, true);
					System.Windows.Documents.Hyperlink link = new System.Windows.Documents.Hyperlink();
					link.Inlines.Add(new System.Windows.Documents.Run(displayNameSig.Convert()));
					link.Tag = new EntryKey(currentMethod.GetGloballyUniqueId());
					link.Click += new System.Windows.RoutedEventHandler(LinkHelper.Resolve);

					CRefPath path = new CRefPath(currentMethod);

					Block description = this.GetSummaryFor(xmlFile,
						currentMethod.Assembly,
						"/doc/members/member[@name='" + path.ToString() + "']/summary"
						);

					members.AddItem(link, description, Model.ElementIconConstants.GetIconPathFor(currentMethod));
				}
				tempContainer.Add(new Header2("Extension Methods"));
				tempContainer.Add(members);
			}

			if (tempContainer.Count > 0) {
				this.Blocks.Add(new Paragraph());
				this.Blocks.Add(new Paragraph(new Run(string.Format("The {0} type exposes the following members.", this.representedType.GetDisplayName(false)))));
				this.Blocks.AddRange(tempContainer);
			}
		}
	}
}