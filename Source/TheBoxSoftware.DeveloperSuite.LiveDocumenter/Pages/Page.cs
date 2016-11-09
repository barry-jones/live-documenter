using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages
{
    using TheBoxSoftware.Diagnostics;
    using TheBoxSoftware.Reflection;
    using TheBoxSoftware.Reflection.Comments;
    using TheBoxSoftware.Reflection.Syntax;
    using TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements;
    using TheBoxSoftware.Documentation;

    /// <summary>
    /// Represents a single page in the LiveDocumentor.
    /// </summary>
    public class Page : FlowDocument
    {
        /// <summary>
        /// Initialises a new Page class
        /// </summary>
        public Page()
        {
            this.Initialise();
        }

        /// <summary>
        /// Initialises a new Page class
        /// </summary>
        /// <param name="title">The title for the page</param>
        public Page(string title)
            : base(new Elements.Header1(title))
        {
            this.Initialise();
        }

        /// <summary>
        /// Initialises basic details for the page
        /// </summary>
        private void Initialise()
        {
            this.Resources.MergedDictionaries.Add(DocumentationResources.BaseResources);
            this.Style = (Style)this.FindResource("PageStyle");
        }

        /// <summary>
        /// Generates the contents of the page
        /// </summary>
        public virtual void Generate()
        {
            if(!this.IsGenerated)
            {
                this.IsGenerated = true;
            }
        }

        protected Block GetSummaryFor(ICommentSource comments, AssemblyDef assembly, CRefPath element)
        {
            Block constructorSummary = null;
            XmlCodeComment comment = comments.GetSummary(element);
            List<Block> constructorComments = Elements.Parser.Parse(assembly, comment);

            if(constructorComments != null && constructorComments.Count > 0)
            {
                constructorSummary = constructorComments.First();
            }

            return constructorSummary;
        }

        /// <summary>
        /// Adds the syntax block for the provided <paramref name="member"/>.
        /// </summary>
        /// <param name="member">The member to produce the syntax for.</param>
        protected void AddSyntaxBlock(ReflectedMember member)
        {
            IFormatter formatter = SyntaxFactory.Create(member, LiveDocumentorFile.Singleton.Language);
            if(formatter != null)
            {
                this.Blocks.Add(new Header2("Syntax"));
                Code c = Parser.ParseSyntax(formatter.Format());
                this.Blocks.Add(c);
            }
        }

        /// <summary>
        /// Adds the inheritance tree for the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to parse and display the tree for.</param>
        protected void AddInheritanceTree(TypeDef type)
        {
            Func<TypeRef, Inline> createLink = delegate (TypeRef forType)
            {
                string displayName = forType.GetDisplayName(true);  // get a default name to display
                TypeDef typeDef = forType as TypeDef;
                if(typeDef != null)
                {
                    displayName = typeDef.GetDisplayName(true);
                }

                Hyperlink link = new Hyperlink(new Run(displayName));
                if(typeDef == null)
                {
                    CRefPath parentCrefPath = new CRefPath(forType);
                    Documentation.Entry found = LiveDocumentorFile.Singleton.LiveDocument.Find(parentCrefPath);
                    if(found != null)
                    {
                        link.Tag = new EntryKey(found.Key);
                    }
                    else
                    {
                        return new Run(displayName);
                    }
                }
                else
                {
                    link.Tag = new EntryKey(typeDef.GetGloballyUniqueId());
                }
                link.Click += new System.Windows.RoutedEventHandler(LinkHelper.Resolve);
                return link;
            };

            // Add the inheritance tree
            List inheritanceList = new List();
            List lastList = inheritanceList;
            if(type.InheritsFrom != null)
            {
                // build a list of parents for the current type
                TypeRef parent = type.InheritsFrom;
                List<Inline> links = new List<Inline>();
                while(parent != null)
                {
                    links.Add(createLink(parent));

                    if(parent is TypeDef)
                    {
                        parent = ((TypeDef)parent).InheritsFrom;
                    }
                    else
                    {
                        parent = null;
                    }
                }

                // Reverse the list and build the inheritance tree
                lastList = inheritanceList;
                for(int i = links.Count - 1; i >= 0; i--)
                {
                    lastList.AddListItem(links[i]);
                    if(i > 0)
                    {
                        lastList = lastList.AddChildList(new List());
                    }
                }
            }

            // add the current type
            lastList = lastList.AddChildList(new List());
            lastList.AddListItem(type.GetDisplayName(true));

            // add all the derived types
            lastList = lastList.AddChildList(new List());
            List<TypeRef> derivedTypes = type.GetExtendingTypes();
            derivedTypes.Sort((a, b) => a.GetFullyQualifiedName().CompareTo(b.GetFullyQualifiedName()));
            for(int i = 0; i < derivedTypes.Count; i++)
            {
                Entry forType = LiveDocumentorFile.Singleton.LiveDocument.Find(
                    CRefPath.Create(derivedTypes[i])
                    );
                if(forType != null)
                {
                    lastList.AddListItem(createLink(derivedTypes[i]));
                }
            }

            this.Blocks.Add(new Header2("Inheritance Hierarchy"));
            this.Blocks.Add(inheritanceList);
        }

        /// <summary>
        /// Add the parameters section for the provided <paramref name="mathod"/>.
        /// </summary>
        /// <param name="method">The method to add the parameters for.</param>
        /// <param name="parsedBlocks">The parsed comments.</param>
        protected void AddParametersForMethod(MethodDef method, List<Block> parsedBlocks)
        {
            // Add the parameter information if available
            List<Param> parameterComments = Parser.ParseElement<Param>(parsedBlocks);
            if(method.Parameters != null && method.Parameters.Count > 0)
            {
                ParameterList parameters = null;
                foreach(ParamDef methodParam in method.Parameters)
                {
                    if(methodParam.Sequence != 0)
                    {
                        // Find the parameter comments
                        Param paramComment = null;
                        foreach(Param current in parameterComments)
                        {
                            if(current.Name == methodParam.Name)
                            {
                                paramComment = current;
                                break;
                            }
                        }

                        TypeRef typeRef = method.ResolveParameter(methodParam.Sequence);
                        EntryKey typeKey = null;
                        string typeName = typeRef.Name;
                        if(parameters == null) { parameters = new ParameterList(); }
                        if(typeRef != null)
                        {
                            if(typeRef is TypeDef)
                            {
                                typeKey = new EntryKey(typeRef.GetGloballyUniqueId());
                            }
                            else
                            {
                                CRefPath path = new CRefPath(typeRef);
                                Documentation.Entry found = LiveDocumentorFile.Singleton.LiveDocument.Find(path);
                                if(found != null)
                                {
                                    typeKey = new EntryKey(found.Key);
                                    typeName = found.Name;
                                }
                                else
                                {
                                    typeKey = null;
                                }
                            }
                            typeName = typeRef.GetDisplayName(false);
                        }
                        List<Block> paramDescription = new List<Block>();
                        if(paramComment != null && paramComment.Description != null)
                        {
                            paramDescription = paramComment.Description;
                        }
                        parameters.Add(methodParam.Name, typeName, method.Assembly, typeKey, paramDescription);
                    }
                }
                if(parameters != null)
                {
                    this.Blocks.Add(parameters);
                }
            }
        }

        /// <summary>
        /// Adds the see also section for the calling element.
        /// </summary>
        /// <param name="parsedBlocks">The parsed comments block for the element.</param>
        /// <remarks>
        /// The <paramref name="parsedBlocks"/> should contain any and all parsed comments
        /// for the calling element. This method will throw the list to the Parser and
        /// then output the seealso comment elements to a list.
        /// </remarks>
        protected void AddSeeAlso(List<Block> parsedBlocks)
        {
            List<SeeAlso> seeAlsoList = Parser.ParseElement<SeeAlso>(parsedBlocks);
            if(seeAlsoList.Count > 0)
            {
                SeeAlsoList seeAlso = new SeeAlsoList();
                seeAlso.Blocks.Add(new Header2("See Also"));
                System.Windows.Documents.List displayList = new System.Windows.Documents.List();
                foreach(SeeAlso currentSeeAlso in seeAlsoList)
                {
                    if(currentSeeAlso.IsEnabled)
                    {
                        displayList.ListItems.Add(new ListItem(new Paragraph(currentSeeAlso.Clone())));
                    }
                    else
                    {
                        displayList.ListItems.Add(new ListItem(new Paragraph(new Run(currentSeeAlso.Name))));
                    }
                }
                seeAlso.Blocks.Add(displayList);
                this.Blocks.Add(seeAlso);
            }
        }

        protected bool IsGenerated { get; set; }

        /// <summary>
        /// Utilises the information provided to create instances of specific Page classes.
        /// </summary>
        /// <param name="forItem">The item to create the Page for</param>
        /// <param name="commentsXml">The comments file where the code comments can be obtained</param>
        /// <param name="mapReference">The DocumentMap to use and to add to</param>
        /// <returns>A new Page instance that can be displayed in the LiveDocumentor</returns>
        public static Page Create(Entry entry, ICommentSource commentsXml)
        {
            TraceHelper.WriteLine("create page");
            TraceHelper.Indent();
            object forItem = entry.Item;

            Page created = null;
            if(forItem is AssemblyDef)
            {
                AssemblyDef ass = forItem as AssemblyDef;
                TraceHelper.WriteLine("assemblydef ({0})", ass.Name);
                created = new AssemblyPage(ass, commentsXml);
            }
            else if(forItem is MethodDef)
            {
                MethodDef method = forItem as MethodDef;
                TraceHelper.WriteLine("method ({0}.{1}.{2})", method.Type.Namespace, method.Type.Name, method.Name);
                created = new MethodPage(method, commentsXml);
            }
            else if(forItem is List<MethodDef>)
            {
                TraceHelper.WriteLine("methods page");
                created = new TypeMethodsPage(forItem as List<MethodDef>, commentsXml);
            }
            else if(forItem is TypeDef)
            {
                TypeDef typeDef = forItem as TypeDef;
                TraceHelper.WriteLine("typedef ({0}.{1})", typeDef.Namespace, typeDef.Name);
                if(typeDef.InheritsFrom != null && typeDef.InheritsFrom.GetFullyQualifiedName() == "System.Enum")
                {
                    created = new EnumerationPage(forItem as TypeDef, commentsXml);
                }
                else if(typeDef.IsDelegate)
                {
                    created = new DelegatePage(forItem as TypeDef, commentsXml);
                }
                else
                {
                    created = new TypePage(forItem as TypeDef, commentsXml);
                }
            }
            else if(forItem is KeyValuePair<string, List<TypeDef>>)
            {
                TraceHelper.WriteLine("namespace {0}", ((KeyValuePair<string, List<TypeDef>>)forItem).Key);
                created = new NamespacePage((KeyValuePair<string, List<TypeDef>>)forItem, commentsXml);
            }
            else if(forItem is List<FieldDef>)
            {
                TraceHelper.WriteLine("fields page");
                created = new TypeFieldsPage(forItem as List<FieldDef>, commentsXml);
            }
            else if(forItem is FieldDef)
            {
                FieldDef field = forItem as FieldDef;
                TraceHelper.WriteLine("field ({0}.{1}.{2})", field.Type.Namespace, field.Type.Name, field.Name);
                created = new FieldPage(field, commentsXml);
            }
            else if(forItem is List<PropertyDef>)
            {
                TraceHelper.WriteLine("properties page");
                created = new TypePropertiesPage(forItem as List<PropertyDef>, commentsXml);
            }
            else if(forItem is PropertyDef)
            {
                PropertyDef property = forItem as PropertyDef;
                TraceHelper.WriteLine("property ({0}.{1}.{2})", property.OwningType.Namespace, property.OwningType.Name, property.Name);
                created = new PropertyPage(property, commentsXml);
            }
            else if(forItem is List<EventDef>)
            {
                TraceHelper.WriteLine("events page");
                created = new TypeEventsPage(forItem as List<EventDef>, commentsXml);
            }
            else if(forItem is EventDef)
            {
                EventDef ev = forItem as EventDef;
                TraceHelper.WriteLine("event ({0}.{1}.{2})", ev.Type.Namespace, ev.Type.Name, ev.Name);
                created = new EventPage(ev, commentsXml);
            }
            else if(forItem is TheBoxSoftware.Documentation.EntryTypes)
            {
                TheBoxSoftware.Documentation.EntryTypes type = (Documentation.EntryTypes)forItem;
                switch(type)
                {
                    case Documentation.EntryTypes.NamespaceContainer:
                        created = new NamespaceContainerPage(entry);
                        break;
                }
            }
            else
            {
                created = new Page(forItem.ToString());
            }

            TraceHelper.Unindent();

            return created;
        }

        public static Page Create(Entry entry, string type, ICommentSource xmlComments)
        {
            object forItem = entry.Item;
            Page created = null;
            switch(type)
            {
                case "Members":
                    created = new TypeMembersPage(forItem as TypeDef, xmlComments);
                    break;
                case "Constructors":
                    created = new TypeConstructorsPage(forItem as List<MethodDef>, xmlComments);
                    break;
                case "Operators":
                    created = new TypeOperatorsPage(forItem as List<MethodDef>, xmlComments);
                    break;
                case "Component Diagram":
                    created = new DeploymentDiagram();
                    break;
            }
            return created;
        }
    }
}