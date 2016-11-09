
namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Documents;
    using TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements;
    using TheBoxSoftware.Reflection;
    using TheBoxSoftware.Reflection.Comments;

    /// <summary>
    /// A page that describes and displays information about an
    /// <see cref="Enum"/> derived type.
    /// </summary>
    public class EnumerationPage : Page
    {
        private TypeDef _representedType;
        private ICommentSource _commentsXml;

        /// <summary>
        /// Initialises a new instance of the EnumerationPage class.
        /// </summary>
        /// <param name="type">The type to display in the page</param>
        /// <param name="xmlComments">The xml comments document for the assembly</param>
        public EnumerationPage(TypeDef type, ICommentSource xmlComments)
        {
            _representedType = type;
            _commentsXml = xmlComments;
        }

        /// <summary>
        /// Generates the contents of the page, utilising details of the
        /// type and its associated xml comments.
        /// </summary>
        public override void Generate()
        {
            if(!this.IsGenerated)
            {
                CRefPath crefPath = new CRefPath(this._representedType);
                List<Block> parsedBlocks = Elements.Parser.Parse(this._representedType.Assembly, _commentsXml, crefPath);

                if(!_commentsXml.Exists())
                {
                    this.Blocks.Add(new NoXmlComments(this._representedType));
                }

                this.Blocks.Add(new Header1(this._representedType.Name + " Enumeration"));

                // Add the summary if it exists
                if(parsedBlocks != null)
                {
                    Block summary = parsedBlocks.Find(currentBlock => currentBlock is Summary);
                    if(summary != null)
                    {
                        this.Blocks.Add(summary);
                    }
                }

                this.AddSyntaxBlock(this._representedType);

                // Add the table of classes to the page
                List<FieldDef> fields = this._representedType.GetFields();
                SummaryTable classTable = new SummaryTable("Member Name", "Description", false);
                var sortedFields = from field in fields
                                   orderby field.Name
                                   select field;
                foreach(FieldDef currentField in sortedFields)
                {
                    if(currentField.IsSystemGenerated)
                    {
                        continue;
                    }
                    Block description = this.GetSummaryFor(
                        _commentsXml, 
                        currentField.Assembly, 
                        new CRefPath(currentField)
                        );
                    classTable.AddItem(currentField.Name, description);
                }
                this.Blocks.Add(new Header2("Members"));
                this.Blocks.Add(classTable);

                if(parsedBlocks != null)
                {
                    Block permissions = parsedBlocks.Find(current => current is PermissionList);
                    if(permissions != null)
                    {
                        this.Blocks.Add(permissions);
                    }
                }

                // Add the remarks if it exists
                if(parsedBlocks != null)
                {
                    Block remarks = parsedBlocks.Find(currentBlock => currentBlock is Remarks);
                    if(remarks != null)
                    {
                        this.Blocks.Add(remarks);
                    }
                }

                // Add the example if it exists
                if(parsedBlocks != null)
                {
                    Block summary = parsedBlocks.Find(currentBlock => currentBlock is Example);
                    if(summary != null)
                    {
                        this.Blocks.Add(new Header2("Examples"));
                        this.Blocks.Add(summary);
                    }
                }

                // Add the seealso list if it exists
                this.AddSeeAlso(parsedBlocks);

                this.IsGenerated = true;
            }
        }
    }
}