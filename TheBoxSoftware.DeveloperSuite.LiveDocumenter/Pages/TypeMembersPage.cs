using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Xml;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages {
	using TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements;
	using TheBoxSoftware.Reflection;
	using TheBoxSoftware.Reflection.Comments;

	public class TypeMembersPage : Page {
		private TypeDef representedType;
		private XmlCodeCommentFile xmlComments;

		public TypeMembersPage(TypeDef type, XmlCodeCommentFile xmlComments) {
			this.representedType = type;
			this.xmlComments = xmlComments;
		}

		public override void Generate() {
			if (!this.IsGenerated) {
				XmlCodeCommentFile xmlFile = this.xmlComments.GetReusableFile();
				CRefPath crefPath = null;
				SummaryTable members;

				this.Blocks.Add(new Header1(this.representedType.GetDisplayName(false) + " Members"));

				List<MethodDef> constructors = this.representedType.GetConstructors();
				if (constructors != null && constructors.Count > 0) {
					this.Blocks.Add(new Header2("Constructors"));
					members = new SummaryTable();
					var sortedMethods = from method in constructors
										orderby method.Name
										select method;
					foreach (MethodDef currentMethod in sortedMethods) {
						crefPath = new CRefPath(currentMethod);
						System.Windows.Documents.Hyperlink link = new System.Windows.Documents.Hyperlink();
						link.Inlines.Add(new System.Windows.Documents.Run(currentMethod.GetDisplayName(false)));
						link.Tag = new EntryKey(Helper.GetUniqueKey(currentMethod.Assembly, currentMethod));
						link.Click += new System.Windows.RoutedEventHandler(LinkHelper.Resolve);

						Block description = this.GetSummaryFor(xmlFile, 
							currentMethod.Assembly, 
							"/doc/members/member[@name='" + crefPath.ToString() + "']/summary");

						members.AddItem(link, description, Model.ElementIconConstants.GetIconPathFor(currentMethod));
					}
					this.Blocks.Add(members);
				}
				
				List<FieldDef> fields = this.representedType.GetFields();
				if (fields != null && fields.Count > 0) {
					this.Blocks.Add(new Header2("Fields"));
					members = new SummaryTable();
					var sortedFields = from field in fields
									   orderby field.Name
									   select field;
					foreach (FieldDef currentField in sortedFields) {
						crefPath = new CRefPath(currentField);
						System.Windows.Documents.Hyperlink link = new System.Windows.Documents.Hyperlink();
						link.Inlines.Add(new System.Windows.Documents.Run(currentField.Name));
						link.Tag = new EntryKey(Helper.GetUniqueKey(currentField.Assembly, currentField));
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
					this.Blocks.Add(members);
				}
				
				List<PropertyDef> properties = this.representedType.GetProperties();
				if (properties != null && properties.Count > 0) {
					this.Blocks.Add(new Header2("Properties"));
					members = new SummaryTable();
					var sortedProperties = from property in properties
										   orderby property.GetDisplayName(false, true)
										   select property;
					foreach (PropertyDef currentProperty in sortedProperties) {
						crefPath = new CRefPath(currentProperty);
						System.Windows.Documents.Hyperlink link = new System.Windows.Documents.Hyperlink();
						link.Inlines.Add(new System.Windows.Documents.Run(currentProperty.GetDisplayName(false, true)));
						link.Tag = new EntryKey(Helper.GetUniqueKey(currentProperty.Type.Assembly, currentProperty));
						link.Click += new System.Windows.RoutedEventHandler(LinkHelper.Resolve);

						Block description = this.GetSummaryFor(xmlFile, currentProperty.Type.Assembly, "/doc/members/member[@name='" + crefPath.ToString() + "']/summary");
						members.AddItem(link, description, Model.ElementIconConstants.GetIconPathFor(currentProperty));
					}
					this.Blocks.Add(members);
				}

				List<EventDef> events = this.representedType.GetEvents();
				if (events != null && events.Count > 0) {
					this.Blocks.Add(new Header2("Events"));
					members = new SummaryTable();
					var sortedEvents = from c in events
										   orderby c.Name
										   select c;
					foreach (EventDef currentEvent in sortedEvents) {
						crefPath = new CRefPath(currentEvent);
						System.Windows.Documents.Hyperlink link = new System.Windows.Documents.Hyperlink();
						link.Inlines.Add(new System.Windows.Documents.Run(currentEvent.Name));
						link.Tag = new EntryKey(Helper.GetUniqueKey(currentEvent.Type.Assembly, currentEvent));
						link.Click += new System.Windows.RoutedEventHandler(LinkHelper.Resolve);

						Block description = this.GetSummaryFor(xmlFile, currentEvent.Type.Assembly, "/doc/members/member[@name='" + crefPath.ToString() + "']/summary");
						members.AddItem(link, description, Model.ElementIconConstants.GetIconPathFor(currentEvent));
					}
					this.Blocks.Add(members);
				}

				List<MethodDef> methods = this.representedType.GetMethods();
				if (methods != null && methods.Count > 0) {
					this.Blocks.Add(new Header2("Methods"));
					members = new SummaryTable();
					var sortedMethods = from method in methods
										orderby method.Name
										select method;
					foreach (MethodDef currentMethod in sortedMethods) {
						crefPath = new CRefPath(currentMethod);
						System.Windows.Documents.Hyperlink link = new System.Windows.Documents.Hyperlink();
						link.Inlines.Add(new System.Windows.Documents.Run(currentMethod.GetDisplayName(false)));
						link.Tag = new EntryKey(Helper.GetUniqueKey(currentMethod.Assembly, currentMethod));
						link.Click += new System.Windows.RoutedEventHandler(LinkHelper.Resolve);

						Block description = this.GetSummaryFor(xmlFile, currentMethod.Assembly, "/doc/members/member[@name='" + crefPath.ToString() + "']/summary");

						members.AddItem(link, description, Model.ElementIconConstants.GetIconPathFor(currentMethod));
					}
					this.Blocks.Add(members);
				}

				List<MethodDef> operators = this.representedType.GetOperators();
				if (operators != null && operators.Count > 0) {
					this.Blocks.Add(new Header2("Operators"));
					members = new SummaryTable();
					var sortedMethods = from method in operators
										orderby method.Name
										select method;
					foreach (MethodDef currentMethod in sortedMethods) {
						crefPath = new CRefPath(currentMethod);
						System.Windows.Documents.Hyperlink link = new System.Windows.Documents.Hyperlink();
						link.Inlines.Add(new System.Windows.Documents.Run(currentMethod.GetDisplayName(false)));
						link.Tag = new EntryKey(Helper.GetUniqueKey(currentMethod.Assembly, currentMethod));
						link.Click += new System.Windows.RoutedEventHandler(LinkHelper.Resolve);

						Block description = this.GetSummaryFor(xmlFile, currentMethod.Assembly, "/doc/members/member[@name='" + crefPath.ToString() + "']/summary");

						members.AddItem(link, description, Model.ElementIconConstants.GetIconPathFor(currentMethod));
					}
					this.Blocks.Add(members);
				}

				if (this.representedType != null && this.representedType.ExtensionMethods.Count > 0) {
					members = new SummaryTable();

					var sortedMethods = from method in this.representedType.ExtensionMethods
										where !method.IsConstructor
										orderby method.Name
										select method;
					foreach (MethodDef currentMethod in sortedMethods) {
						System.Windows.Documents.Hyperlink link = new System.Windows.Documents.Hyperlink();
						link.Inlines.Add(new System.Windows.Documents.Run(currentMethod.GetDisplayName(false)));
						link.Tag = new EntryKey(Helper.GetUniqueKey(currentMethod.Assembly, currentMethod));
						link.Click += new System.Windows.RoutedEventHandler(LinkHelper.Resolve);

						CRefPath path = new CRefPath(currentMethod);

						Block description = this.GetSummaryFor(xmlFile,
							currentMethod.Assembly,
							"/doc/members/member[@name='" + path.ToString() + "']/summary"
							);

						members.AddItem(link, description, Model.ElementIconConstants.GetIconPathFor(currentMethod));
					}
					this.Blocks.Add(new Header2("Extension Methods"));
					this.Blocks.Add(members);
				}

				this.IsGenerated = true;
			}
		}
	}
}
