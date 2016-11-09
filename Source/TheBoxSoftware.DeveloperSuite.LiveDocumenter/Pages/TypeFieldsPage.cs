
namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Documents;
    using TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements;
    using TheBoxSoftware.Reflection;
    using TheBoxSoftware.Reflection.Comments;

    /// <summary>
    /// A page that displays all of the fields that are available in a type
    /// </summary>
    public sealed class TypeFieldsPage : Page
    {
        private List<FieldDef> _fields;
        private ICommentSource _xmlComments;

        /// <summary>
        /// Initialises a new instance of the TypeFieldsPage class
        /// </summary>
        /// <param name="fields">The fields to manage</param>
        /// <param name="xmlComments">The xml comments</param>
        public TypeFieldsPage(List<FieldDef> fields, ICommentSource xmlComments)
        {
            _fields = fields;
            _xmlComments = xmlComments;
        }

        /// <summary>
        /// Generates the pages contents
        /// </summary>
        public override void Generate()
        {
            if(!this.IsGenerated)
            {
                TypeRef definingType = null;
                if(_fields.Count > 0)
                {
                    definingType = _fields[0].Type;
                }
                if(!_xmlComments.Exists())
                {
                    this.Blocks.Add(new NoXmlComments(definingType));
                }

                this.Blocks.Add(new Header1(definingType.GetDisplayName(false) + " Fields"));

                if(this._fields != null && this._fields.Count > 0)
                {
                    SummaryTable displayedFields = new SummaryTable();

                    var sortedFields = from field in this._fields
                                       orderby field.Name
                                       where !LiveDocumentorFile.Singleton.LiveDocument.IsMemberFiltered(field)
                                       select field;
                    foreach(FieldDef currentField in sortedFields)
                    {
                        CRefPath crefPath = new CRefPath(currentField);
                        System.Windows.Documents.Hyperlink link = new System.Windows.Documents.Hyperlink();
                        link.Inlines.Add(new System.Windows.Documents.Run(currentField.Name));
                        link.Tag = new EntryKey(currentField.GetGloballyUniqueId());
                        link.Click += new System.Windows.RoutedEventHandler(LinkHelper.Resolve);

                        // First we check if there is a summary for the field, then if not we check for a
                        // definition of value and use that if it is defined.
                        Block summary = null;
                        XmlCodeComment comment = _xmlComments.GetSummary(
                            crefPath
                            );
                        List<Block> parsedBlocks = Elements.Parser.Parse(currentField.Assembly, comment);
                        if(parsedBlocks != null && parsedBlocks.Count > 0)
                        {
                            summary = parsedBlocks[0];
                        }
                        else
                        {
                            XmlCodeComment value = _xmlComments.GetValue(
                                crefPath
                                );
                            parsedBlocks = Elements.Parser.Parse(currentField.Assembly, value);
                            if(parsedBlocks != null && parsedBlocks.Count > 0)
                            {
                                summary = parsedBlocks[0];
                            }
                        }

                        displayedFields.AddItem(link, summary, Model.ElementIconConstants.GetIconPathFor(currentField));
                    }
                    this.Blocks.Add(displayedFields);
                }

                this.IsGenerated = true;
            }
        }
    }
}