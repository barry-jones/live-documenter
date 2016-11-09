
namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Documents;
    using TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements;
    using TheBoxSoftware.Reflection;
    using TheBoxSoftware.Reflection.Comments;

    public class TypeConstructorsPage : Page
    {
        private List<MethodDef> typesMethods;
        private ICommentSource xmlComments;

        public TypeConstructorsPage(List<MethodDef> typesMethods, ICommentSource xmlComments)
        {
            this.typesMethods = typesMethods;
            this.xmlComments = xmlComments;
        }

        public override void Generate()
        {
            if(!this.IsGenerated)
            {
                TypeDef definingType = null;
                if(this.typesMethods != null && this.typesMethods.Count > 0)
                {
                    definingType = (TypeDef)this.typesMethods[0].Type;
                }
                if(!this.xmlComments.Exists())
                {
                    this.Blocks.Add(new NoXmlComments(definingType));
                }

                this.Blocks.Add(new Header1(definingType.GetDisplayName(false) + " Constructors"));

                if(this.typesMethods != null && this.typesMethods.Count > 0)
                {
                    SummaryTable methods = new SummaryTable();

                    var sortedMethods = from method in this.typesMethods
                                        where method.IsConstructor &&
                                            !LiveDocumentorFile.Singleton.LiveDocument.IsMemberFiltered(method)
                                        orderby method.Name
                                        select method;
                    foreach(MethodDef currentMethod in sortedMethods)
                    {
                        CRefPath crefPath = new CRefPath(currentMethod);
                        System.Windows.Documents.Hyperlink link = new System.Windows.Documents.Hyperlink();
                        link.Inlines.Add(new System.Windows.Documents.Run(currentMethod.GetDisplayName(false)));
                        link.Tag = new EntryKey(currentMethod.GetGloballyUniqueId());
                        link.Click += new System.Windows.RoutedEventHandler(LinkHelper.Resolve);

                        Block constructorSummary = this.GetSummaryFor(
                            xmlComments, 
                            currentMethod.Assembly,
                            crefPath
                            );

                        methods.AddItem(link, constructorSummary, Model.ElementIconConstants.GetIconPathFor(currentMethod));
                    }
                    this.Blocks.Add(methods);
                }

                this.IsGenerated = true;
            }
        }
    }
}