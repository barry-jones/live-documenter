
namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Documents;
    using TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements;
    using TheBoxSoftware.Reflection;
    using TheBoxSoftware.Reflection.Comments;
    using TheBoxSoftware.Reflection.Signitures;

    public class TypeMembersPage : Page
    {
        private TypeDef representedType;
        private ICommentSource xmlComments;

        public TypeMembersPage(TypeDef type, ICommentSource xmlComments)
        {
            this.representedType = type;
            this.xmlComments = xmlComments;
        }

        public override void Generate()
        {
            if(!this.IsGenerated)
            {
                CRefPath crefPath = null;
                SummaryTable members;

                if(!this.xmlComments.Exists())
                {
                    this.Blocks.Add(new NoXmlComments(this.representedType));
                }

                this.Blocks.Add(new Header1(this.representedType.GetDisplayName(false) + " Members"));

                List<MethodDef> constructors = this.representedType.GetConstructors();
                if(constructors != null && constructors.Count > 0)
                {
                    this.Blocks.Add(new Header2("Constructors"));
                    members = new SummaryTable();
                    var sortedMethods = from method in constructors
                                        orderby method.Name
                                        where !LiveDocumentorFile.Singleton.LiveDocument.IsMemberFiltered(method)
                                        select method;
                    foreach(MethodDef currentMethod in sortedMethods)
                    {
                        crefPath = new CRefPath(currentMethod);
                        System.Windows.Documents.Hyperlink link = new System.Windows.Documents.Hyperlink();
                        link.Inlines.Add(new System.Windows.Documents.Run(currentMethod.GetDisplayName(false)));
                        link.Tag = new EntryKey(currentMethod.GetGloballyUniqueId());
                        link.Click += new System.Windows.RoutedEventHandler(LinkHelper.Resolve);

                        Block description = this.GetSummaryFor(xmlComments,
                            currentMethod.Assembly,
                            crefPath);

                        members.AddItem(link, description, Model.ElementIconConstants.GetIconPathFor(currentMethod));
                    }
                    this.Blocks.Add(members);
                }

                List<FieldDef> fields = this.representedType.GetFields();
                if(fields != null && fields.Count > 0)
                {
                    this.Blocks.Add(new Header2("Fields"));
                    members = new SummaryTable();
                    var sortedFields = from field in fields
                                       orderby field.Name
                                       where !LiveDocumentorFile.Singleton.LiveDocument.IsMemberFiltered(field)
                                       select field;
                    foreach(FieldDef currentField in sortedFields)
                    {
                        crefPath = new CRefPath(currentField);
                        System.Windows.Documents.Hyperlink link = new System.Windows.Documents.Hyperlink();
                        link.Inlines.Add(new System.Windows.Documents.Run(currentField.Name));
                        link.Tag = new EntryKey(currentField.GetGloballyUniqueId());
                        link.Click += new System.Windows.RoutedEventHandler(LinkHelper.Resolve);

                        Block description = this.GetSummaryFor(xmlComments,
                            currentField.Assembly,
                            crefPath);
                        Block value = this.GetSummaryFor(xmlComments,
                            currentField.Assembly,
                            crefPath);
                        if(description != null)
                        {
                            members.AddItem(link, description, Model.ElementIconConstants.GetIconPathFor(currentField));
                        }
                        else
                        {
                            members.AddItem(link, value, Model.ElementIconConstants.GetIconPathFor(currentField));
                        }
                    }
                    this.Blocks.Add(members);
                }

                List<PropertyDef> properties = this.representedType.GetProperties();
                if(properties != null && properties.Count > 0)
                {
                    this.Blocks.Add(new Header2("Properties"));
                    members = new SummaryTable();

                    var sortedProperties = from property in properties
                                           orderby new DisplayNameSignitureConvertor(property, false, true).Convert()
                                           where !LiveDocumentorFile.Singleton.LiveDocument.IsMemberFiltered(property)
                                           select property;

                    foreach(PropertyDef currentProperty in sortedProperties)
                    {
                        crefPath = new CRefPath(currentProperty);
                        System.Windows.Documents.Hyperlink link = new System.Windows.Documents.Hyperlink();
                        link.Inlines.Add(new Run(new DisplayNameSignitureConvertor(currentProperty, false, true).Convert()));
                        link.Tag = new EntryKey(currentProperty.GetGloballyUniqueId());
                        link.Click += new System.Windows.RoutedEventHandler(LinkHelper.Resolve);

                        Block description = this.GetSummaryFor(xmlComments, currentProperty.OwningType.Assembly, crefPath);
                        members.AddItem(link, description, Model.ElementIconConstants.GetIconPathFor(currentProperty));
                    }
                    this.Blocks.Add(members);
                }

                List<EventDef> events = this.representedType.GetEvents();
                if(events != null && events.Count > 0)
                {
                    this.Blocks.Add(new Header2("Events"));
                    members = new SummaryTable();
                    var sortedEvents = from c in events
                                       orderby c.Name
                                       where !LiveDocumentorFile.Singleton.LiveDocument.IsMemberFiltered(c)
                                       select c;
                    foreach(EventDef currentEvent in sortedEvents)
                    {
                        crefPath = new CRefPath(currentEvent);
                        System.Windows.Documents.Hyperlink link = new System.Windows.Documents.Hyperlink();
                        link.Inlines.Add(new System.Windows.Documents.Run(currentEvent.Name));
                        link.Tag = new EntryKey(currentEvent.GetGloballyUniqueId());
                        link.Click += new System.Windows.RoutedEventHandler(LinkHelper.Resolve);

                        Block description = this.GetSummaryFor(xmlComments, currentEvent.Type.Assembly, 
                            crefPath);
                        members.AddItem(link, description, Model.ElementIconConstants.GetIconPathFor(currentEvent));
                    }
                    this.Blocks.Add(members);
                }

                List<MethodDef> methods = this.representedType.GetMethods();
                if(methods != null && methods.Count > 0)
                {
                    this.Blocks.Add(new Header2("Methods"));
                    members = new SummaryTable();
                    var sortedMethods = from method in methods
                                        orderby method.Name
                                        where !LiveDocumentorFile.Singleton.LiveDocument.IsMemberFiltered(method)
                                        select method;
                    foreach(MethodDef currentMethod in sortedMethods)
                    {
                        crefPath = new CRefPath(currentMethod);
                        System.Windows.Documents.Hyperlink link = new System.Windows.Documents.Hyperlink();
                        link.Inlines.Add(new System.Windows.Documents.Run(currentMethod.GetDisplayName(false)));
                        link.Tag = new EntryKey(currentMethod.GetGloballyUniqueId());
                        link.Click += new System.Windows.RoutedEventHandler(LinkHelper.Resolve);

                        Block description = this.GetSummaryFor(xmlComments, currentMethod.Assembly, 
                            crefPath);

                        members.AddItem(link, description, Model.ElementIconConstants.GetIconPathFor(currentMethod));
                    }
                    this.Blocks.Add(members);
                }

                List<MethodDef> operators = this.representedType.GetOperators();
                if(operators != null && operators.Count > 0)
                {
                    this.Blocks.Add(new Header2("Operators"));
                    members = new SummaryTable();
                    var sortedMethods = from method in operators
                                        orderby method.Name
                                        where !LiveDocumentorFile.Singleton.LiveDocument.IsMemberFiltered(method)
                                        select method;
                    foreach(MethodDef currentMethod in sortedMethods)
                    {
                        crefPath = new CRefPath(currentMethod);
                        System.Windows.Documents.Hyperlink link = new System.Windows.Documents.Hyperlink();
                        link.Inlines.Add(new System.Windows.Documents.Run(currentMethod.GetDisplayName(false)));
                        link.Tag = new EntryKey(currentMethod.GetGloballyUniqueId());
                        link.Click += new System.Windows.RoutedEventHandler(LinkHelper.Resolve);

                        Block description = this.GetSummaryFor(xmlComments, currentMethod.Assembly, 
                            crefPath);

                        members.AddItem(link, description, Model.ElementIconConstants.GetIconPathFor(currentMethod));
                    }
                    this.Blocks.Add(members);
                }

                if(this.representedType != null && this.representedType.ExtensionMethods.Count > 0)
                {
                    members = new SummaryTable();

                    var sortedMethods = from method in this.representedType.ExtensionMethods
                                        where !method.IsConstructor
                                        orderby method.Name
                                        where !LiveDocumentorFile.Singleton.LiveDocument.IsMemberFiltered(method)
                                        select method;
                    foreach(MethodDef currentMethod in sortedMethods)
                    {
                        DisplayNameSignitureConvertor displayNameSig = new DisplayNameSignitureConvertor(currentMethod, false, true, true);
                        System.Windows.Documents.Hyperlink link = new System.Windows.Documents.Hyperlink();
                        link.Inlines.Add(new System.Windows.Documents.Run(displayNameSig.Convert()));
                        link.Tag = new EntryKey(currentMethod.GetGloballyUniqueId());
                        link.Click += new System.Windows.RoutedEventHandler(LinkHelper.Resolve);

                        CRefPath path = new CRefPath(currentMethod);

                        Block description = this.GetSummaryFor(xmlComments,
                            currentMethod.Assembly,
                            path
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