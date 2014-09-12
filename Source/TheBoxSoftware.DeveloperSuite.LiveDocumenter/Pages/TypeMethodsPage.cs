using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages {
	using TheBoxSoftware.Reflection;
	using TheBoxSoftware.Reflection.Comments;
	using TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements;
    using TheBoxSoftware.Reflection.Signitures;

	public class TypeMethodsPage : Page {
		private List<MethodDef> typesMethods;
		private XmlCodeCommentFile xmlComments;

		public TypeMethodsPage(List<MethodDef> typesMethods, XmlCodeCommentFile xmlComments) {
			this.typesMethods = typesMethods;
			this.xmlComments = xmlComments;
		}

		public override void Generate() {
			if (!this.IsGenerated) {
				TypeDef definingType = null;
				if(this.typesMethods != null && this.typesMethods.Count > 0) {
					definingType = (TypeDef)this.typesMethods[0].Type;
				}
				XmlCodeCommentFile comments = this.xmlComments.GetReusableFile();

				if (!this.xmlComments.Exists) {
					this.Blocks.Add(new NoXmlComments(definingType));
				}

				this.Blocks.Add(new Header1(definingType.GetDisplayName(false) + " Methods"));

				if (this.typesMethods != null && this.typesMethods.Count > 0) {
					SummaryTable methods = new SummaryTable();

					var sortedMethods = from method in this.typesMethods
										where !method.IsConstructor
										orderby method.Name
										where !LiveDocumentorFile.Singleton.LiveDocument.IsMemberFiltered(method)
										select method;
					foreach(MethodDef currentMethod in sortedMethods) {
						System.Windows.Documents.Hyperlink link = new System.Windows.Documents.Hyperlink();
						link.Inlines.Add(new System.Windows.Documents.Run(currentMethod.GetDisplayName(false)));
						link.Tag = new EntryKey(currentMethod.GetGloballyUniqueId());
						link.Click += new System.Windows.RoutedEventHandler(LinkHelper.Resolve);

						CRefPath path = new CRefPath(currentMethod);

						System.Windows.Documents.Block description = this.GetSummaryFor(comments, 
							currentMethod.Assembly, 
							"/doc/members/member[@name='" + path.ToString() + "']/summary"
							);

						methods.AddItem(link, description, Model.ElementIconConstants.GetIconPathFor(currentMethod));
					}
					this.Blocks.Add(methods);
				}

				if (definingType != null && definingType.ExtensionMethods.Count > 0) {
					SummaryTable methods = new SummaryTable();

					var sortedMethods = from method in definingType.ExtensionMethods
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

						System.Windows.Documents.Block description = this.GetSummaryFor(comments,
							currentMethod.Assembly,
							"/doc/members/member[@name='" + path.ToString() + "']/summary"
							);

						methods.AddItem(link, description, Model.ElementIconConstants.GetIconPathFor(currentMethod));
					}
					this.Blocks.Add(new Header2("Extension Methods"));
					this.Blocks.Add(methods);
				}

				this.IsGenerated = true;
			}
		}
	}
}
