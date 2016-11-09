
namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages
{
    using System.Collections.Generic;
    using System.Linq;
    using TheBoxSoftware.Reflection;
    using TheBoxSoftware.Reflection.Comments;
    using TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements;

    /// <summary>
    /// Displays the details of all of the events for a specific type and provides
    /// links to the individual event page entries.
    /// </summary>
    public class TypeEventsPage : Page
    {
        private List<EventDef> typesEvents;
        private XmlCodeCommentFile xmlComments;

        /// <summary>
        /// Initialises a new instance of the TypeEventsPage class.
        /// </summary>
        /// <param name="typesEvents">The events defined in the type.</param>
        /// <param name="xmlComments">The associated defining libraries xml code comments file.</param>
        public TypeEventsPage(List<EventDef> typesEvents, XmlCodeCommentFile xmlComments)
        {
            this.typesEvents = typesEvents;
            this.xmlComments = xmlComments;
        }

        public override void Generate()
        {
            if(!this.IsGenerated)
            {
                TypeDef definingType = null;
                if(this.typesEvents != null && this.typesEvents.Count > 0)
                {
                    definingType = (TypeDef)this.typesEvents[0].Type;
                }
                ICommentSource comments = this.xmlComments.GetReusableFile();

                if(!this.xmlComments.Exists())
                {
                    this.Blocks.Add(new NoXmlComments(definingType));
                }

                this.Blocks.Add(new Header1(definingType.GetDisplayName(false) + " Events"));

                if(this.typesEvents != null && this.typesEvents.Count > 0)
                {
                    SummaryTable methods = new SummaryTable();

                    var sortedMethods = from method in this.typesEvents
                                        orderby method.Name
                                        where !LiveDocumentorFile.Singleton.LiveDocument.IsMemberFiltered(method)
                                        select method;
                    foreach(EventDef currentMethod in sortedMethods)
                    {
                        System.Windows.Documents.Hyperlink link = new System.Windows.Documents.Hyperlink();
                        link.Inlines.Add(new System.Windows.Documents.Run(currentMethod.Name));
                        link.Tag = new EntryKey(currentMethod.GetGloballyUniqueId());
                        link.Click += new System.Windows.RoutedEventHandler(LinkHelper.Resolve);

                        CRefPath path = new CRefPath(currentMethod);

                        System.Windows.Documents.Block description = this.GetSummaryFor(
                            comments,
                            currentMethod.Type.Assembly,
                            path
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