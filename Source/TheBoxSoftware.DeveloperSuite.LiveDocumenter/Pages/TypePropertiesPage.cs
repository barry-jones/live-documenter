
namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Documents;
    using Reflection.Signitures;
    using TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements;
    using TheBoxSoftware.Reflection;
    using TheBoxSoftware.Reflection.Comments;

    /// <summary>
    /// A page to describe and display the properties for a single TypeDef.
    /// </summary>
    public class TypePropertiesPage : Page
    {
        private List<PropertyDef> _properties;
        private XmlCodeCommentFile _xmlComments;

        /// <summary>
        /// Initialises a new instance of the TypePropertiesPage class.
        /// </summary>
        /// <param name="properties">The properties to display.</param>
        /// <param name="xmlComments">The assemblies xml comments file</param>
        public TypePropertiesPage(List<PropertyDef> properties, XmlCodeCommentFile xmlComments)
        {
            _properties = properties;
            _xmlComments = xmlComments;
        }

        /// <summary>
        /// Generates the pages contents.
        /// </summary>
        public override void Generate()
        {
            if(!IsGenerated)
            {
                TypeDef definingType = null;
                if(_properties != null && _properties.Count > 0)
                {
                    definingType = (TypeDef)_properties[0].OwningType;
                }
                XmlCodeCommentFile comments = _xmlComments.GetReusableFile();

                if(!_xmlComments.Exists())
                {
                    this.Blocks.Add(new NoXmlComments(definingType));
                }

                Blocks.Add(new Header1(definingType.GetDisplayName(false) + " Properties"));

                if(_properties != null && _properties.Count > 0)
                {
                    SummaryTable methods = new SummaryTable();

                    var sortedProperties = from property in _properties
                                           orderby new DisplayNameSignitureConvertor(property, false, true).Convert()
                                           select property;

                    foreach(PropertyDef currentProperty in sortedProperties)
                    {
                        CRefPath path = new CRefPath(currentProperty);
                        System.Windows.Documents.Hyperlink link = new System.Windows.Documents.Hyperlink();
                        link.Inlines.Add(new Run(new DisplayNameSignitureConvertor(currentProperty, false, true).Convert()));
                        link.Tag = new EntryKey(currentProperty.GetGloballyUniqueId());
                        link.Click += new System.Windows.RoutedEventHandler(LinkHelper.Resolve);

                        Block description = GetSummaryFor(comments,
                            currentProperty.OwningType.Assembly,
                            "/doc/members/member[@name='" + path.ToString() + "']/summary");

                        methods.AddItem(link, description, Model.ElementIconConstants.GetIconPathFor(currentProperty));
                    }
                    this.Blocks.Add(methods);
                }

                this.IsGenerated = true;
            }
        }
    }
}