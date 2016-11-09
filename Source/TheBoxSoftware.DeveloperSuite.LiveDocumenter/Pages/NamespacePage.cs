
namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Documents;
    using TheBoxSoftware.Reflection;
    using TheBoxSoftware.Reflection.Comments;
    using TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements;

    /// <summary>
    /// A Page to display information about a namespace in the LiveDocumentor
    /// </summary>
    public class NamespacePage : Page
    {
        private KeyValuePair<string, List<TypeDef>> item;
        private XmlCodeCommentFile commentsXml;

        /// <summary>
        /// Initialises a new instance of the NamespacePage class
        /// </summary>
        /// <param name="item">The namespace details as a list of methods</param>
        /// <param name="commentsXml">The code comments file to get comments from</param>
        public NamespacePage(KeyValuePair<string, List<TypeDef>> item, XmlCodeCommentFile commentsXml)
        {
            this.item = item;
            this.commentsXml = commentsXml;
        }

        /// <summary>
        /// Generates the contents of the page
        /// </summary>
        public override void Generate()
        {
            if(!this.IsGenerated)
            {
                ICommentSource xmlFile = commentsXml.GetReusableFile();

                this.Blocks.Add(new Header1(item.Key + " Namespace"));

                // classes
                IOrderedEnumerable<TypeDef> allClasses = from type in item.Value
                                                         where !type.IsDelegate && !type.IsEnumeration && !type.IsInterface && !type.IsStructure &&
                                                            !LiveDocumentorFile.Singleton.LiveDocument.IsMemberFiltered(type)
                                                         orderby type.Name
                                                         select type;
                this.OutputTypes("Classes", allClasses, xmlFile);

                // structures
                IOrderedEnumerable<TypeDef> allStructures = from type in item.Value
                                                            where type.IsStructure &&
                                                               !LiveDocumentorFile.Singleton.LiveDocument.IsMemberFiltered(type)
                                                            orderby type.Name
                                                            select type;
                this.OutputTypes("Structures", allStructures, xmlFile);

                // delegates
                IOrderedEnumerable<TypeDef> allDelegates = from type in item.Value
                                                           where type.IsDelegate &&
                                                              !LiveDocumentorFile.Singleton.LiveDocument.IsMemberFiltered(type)
                                                           orderby type.Name
                                                           select type;
                this.OutputTypes("Delegates", allDelegates, xmlFile);

                // enumerations
                IOrderedEnumerable<TypeDef> allEnumerations = from type in item.Value
                                                              where type.IsEnumeration &&
                                                                 !LiveDocumentorFile.Singleton.LiveDocument.IsMemberFiltered(type)
                                                              orderby type.Name
                                                              select type;
                this.OutputTypes("Enumerations", allEnumerations, xmlFile);

                // interfaces
                IOrderedEnumerable<TypeDef> allInterfaces = from type in item.Value
                                                            where type.IsInterface &&
                                                               !LiveDocumentorFile.Singleton.LiveDocument.IsMemberFiltered(type)
                                                            orderby type.Name
                                                            select type;
                this.OutputTypes("Interfaces", allInterfaces, xmlFile);

                this.IsGenerated = true;
            }
        }

        /// <summary>
        /// Adds a table of the <paramref name="types"/> to the document with a header <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The header text.</param>
        /// <param name="types">The types to add to the table.</param>
        /// <param name="xmlFile">The XML file to read comments from.</param>
        private void OutputTypes(string name, IOrderedEnumerable<TypeDef> types, ICommentSource xmlFile)
        {
            if(types.Count() != 0)
            {
                SummaryTable classTable = new SummaryTable();
                foreach(TypeDef currentType in types)
                {
                    CRefPath crefPath = new CRefPath(currentType);

                    // Find the description for the type
                    Block description = this.GetSummaryFor(
                        xmlFile, 
                        currentType.Assembly, 
                        crefPath
                        );
                    Hyperlink nameLink = new Hyperlink(new Run(currentType.GetDisplayName(false)));
                    nameLink.Tag = new EntryKey(currentType.GetGloballyUniqueId());
                    nameLink.Click += new System.Windows.RoutedEventHandler(LinkHelper.Resolve);
                    classTable.AddItem(nameLink, description, Model.ElementIconConstants.GetIconPathFor(currentType));
                }
                this.Blocks.Add(new Header2(name));
                this.Blocks.Add(classTable);
            }
        }
    }
}