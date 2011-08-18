using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages {
	using TheBoxSoftware.Reflection;
	using TheBoxSoftware.Reflection.Comments;
	using TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements;

	/// <summary>
	/// Page the handles the display and formatting of a TypeDefs
	/// operators.
	/// </summary>
	public sealed class TypeOperatorsPage : Page {
		private List<MethodDef> typesMethods;
		private XmlCodeCommentFile xmlComments;

		public TypeOperatorsPage(List<MethodDef> typesMethods, XmlCodeCommentFile xmlComments) {
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

				this.Blocks.Add(new Header1(definingType.GetDisplayName(false) + " Operators"));

				if (this.typesMethods != null && this.typesMethods.Count > 0) {
					SummaryTable methods = new SummaryTable();

					var sortedMethods = from method in this.typesMethods
										where !method.IsConstructor
										orderby method.Name
										select method;
					foreach(MethodDef currentMethod in sortedMethods) {
						System.Windows.Documents.Hyperlink link = new System.Windows.Documents.Hyperlink();
						link.Inlines.Add(new System.Windows.Documents.Run(currentMethod.GetDisplayName(false, false)));
						link.Tag = new EntryKey(Helper.GetUniqueKey(currentMethod.Assembly, currentMethod));
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

				this.IsGenerated = true;
			}
		}
	}
}
