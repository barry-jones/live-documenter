
namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages
{
    using System.Collections.Generic;
    using System.Linq;
    using TheBoxSoftware.Reflection;
    using TheBoxSoftware.Reflection.Comments;
    using TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements;
    using TheBoxSoftware.Reflection.Signitures;

    public class TypeMethodsPage : Page
    {
        private List<MethodDef> _typesMethods;
        private XmlCodeCommentFile _xmlComments;

        public TypeMethodsPage(List<MethodDef> typesMethods, XmlCodeCommentFile xmlComments)
        {
            _typesMethods = typesMethods;
            _xmlComments = xmlComments;
        }

        public override void Generate()
        {
            if(!IsGenerated)
            {
                TypeDef definingType = null;
                if(_typesMethods != null && _typesMethods.Count > 0)
                {
                    definingType = _typesMethods[0].Type as TypeDef;
                }
                XmlCodeCommentFile comments = _xmlComments.GetReusableFile();

                if(!_xmlComments.Exists())
                {
                    Blocks.Add(new NoXmlComments(definingType));
                }

                Blocks.Add(new Header1(definingType.GetDisplayName(false) + " Methods"));

                if(_typesMethods != null && _typesMethods.Count > 0)
                {
                    SummaryTable methods = new SummaryTable();

                    var sortedMethods = from method in this._typesMethods
                                        where !method.IsConstructor
                                        orderby method.Name
                                        where !LiveDocumentorFile.Singleton.LiveDocument.IsMemberFiltered(method)
                                        select method;

                    foreach(MethodDef currentMethod in sortedMethods)
                    {
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
                    Blocks.Add(methods);
                }

                if(definingType != null && definingType.ExtensionMethods.Count > 0)
                {
                    SummaryTable methods = new SummaryTable();

                    var sortedMethods = from method in definingType.ExtensionMethods
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

                        System.Windows.Documents.Block description = this.GetSummaryFor(comments,
                            currentMethod.Assembly,
                            "/doc/members/member[@name='" + path.ToString() + "']/summary"
                            );

                        methods.AddItem(link, description, Model.ElementIconConstants.GetIconPathFor(currentMethod));
                    }
                    Blocks.Add(new Header2("Extension Methods"));
                    Blocks.Add(methods);
                }

                IsGenerated = true;
                // we also no longer need to store a reference to the XML file I think so we can remove it
                _xmlComments = null;
                _typesMethods = null;
            }
        }
    }
}