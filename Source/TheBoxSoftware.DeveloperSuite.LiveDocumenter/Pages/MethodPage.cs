
namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages
{
    using System.Collections.Generic;
    using System.Windows.Documents;
    using TheBoxSoftware.Reflection;
    using TheBoxSoftware.Reflection.Comments;
    using TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements;

    /// <summary>
    /// A page that constructs and displays the comments and details for
    /// a <see cref="MethodDef"/> instance.
    /// </summary>
    public class MethodPage : Page
    {
        private MethodDef _method;
        private readonly ICommentSource _commentsXml;

        /// <summary>
        /// Initialises a new MethodPage class
        /// </summary>
        /// <param name="method">The method this page is to document</param>
        /// <param name="commentsXml">The comments document</param>
        public MethodPage(MethodDef method, ICommentSource commentsXml)
            : base()
        {
            _method = method;
            _commentsXml = commentsXml;
        }

        /// <summary>
        /// Generates the page contents
        /// </summary>
        public override void Generate()
        {
            if(!IsGenerated)
            {
                CRefPath crefPath = new CRefPath(_method);
                List<Block> parsedBlocks = Elements.Parser.Parse(_method.Assembly, _commentsXml, crefPath);

                if (!_commentsXml.Exists())
                {
                    Blocks.Add(new NoXmlComments(_method));
                }

                Blocks.Add(new Elements.Header1(_method.GetDisplayName(false)));

                // Add the summary if it exists
                if (parsedBlocks != null)
                {
                    Block summary = parsedBlocks.Find(currentBlock => currentBlock is Summary);
                    if (summary != null)
                    {
                        Blocks.Add(summary);
                    }
                }

                AddSyntaxBlock(_method);

                // Add the type parameters if they exist
                if (parsedBlocks != null)
                {
                    List<Block> typeParams = parsedBlocks.FindAll(currentBlock => currentBlock is TypeParamEntry);
                    if (typeParams.Count > 0)
                    {
                        TypeParamSection typeParamSection = new TypeParamSection();
                        foreach (GenericTypeRef genericType in _method.GenericTypes)
                        {
                            string name = genericType.Name;
                            string description = string.Empty;
                            foreach (TypeParamEntry current in typeParams)
                            {
                                if (current.Param == genericType.Name)
                                {
                                    description = current.Description;
                                }
                            }
                            typeParamSection.AddEntry(new TypeParamEntry(name, description));
                        }
                        Blocks.Add(typeParamSection);
                    }
                }

                AddParametersForMethod(_method, parsedBlocks);
                AddReturnDetails(parsedBlocks);

                // Add the exception table if it exists
                if (parsedBlocks != null)
                {
                    Block exceptions = parsedBlocks.Find(currentBlock => currentBlock is ExceptionList);
                    if (exceptions != null)
                    {
                        Blocks.Add(exceptions);
                    }
                }

                if (parsedBlocks != null)
                {
                    Block permissions = parsedBlocks.Find(current => current is PermissionList);
                    if (permissions != null)
                    {
                        Blocks.Add(permissions);
                    }
                }

                // Add the remarks if it exists
                if (parsedBlocks != null)
                {
                    Block remarks = parsedBlocks.Find(currentBlock => currentBlock is Remarks);
                    if (remarks != null)
                    {
                        Blocks.Add(remarks);
                    }
                }

                // Add the example if it exists
                if (parsedBlocks != null)
                {
                    Block summary = parsedBlocks.Find(currentBlock => currentBlock is Example);
                    if (summary != null)
                    {
                        Blocks.Add(new Header2("Examples"));
                        Blocks.Add(summary);
                    }
                }

                // Add the seealso list if it exists
                AddSeeAlso(parsedBlocks);

                IsGenerated = true;
            }
        }

        private void AddReturnDetails(List<Block> parsedBlocks)
        {
            TypeRef returnTypeRef = _method.GetReturnType();

            if (returnTypeRef == WellKnownTypeDef.Void)
                return;

            Blocks.Add(new Header3("Returns"));

            (EntryKey typeKey, string typeName) = CreateEntryKey(returnTypeRef);

            // build the page output
            Inline type = new Run(typeName);
            if (typeKey != null)
            {
                type = new Hyperlink(new Run(typeName));
                ((Hyperlink)type).Tag = typeKey;
                ((Hyperlink)type).Click += new System.Windows.RoutedEventHandler(LinkHelper.Resolve);
            }

            Blocks.Add(new Paragraph(type));

            if (parsedBlocks != null)
            {
                Block found = parsedBlocks.Find(currentBlock => currentBlock is Returns);
                if (found != null)
                    Blocks.Add(found);
            }
        }

        /// <summary>
        /// Attempts to resolve the provided TypeRef to an EntryKey which can be used to create
        /// a link to the return type.
        /// </summary>
        /// <param name="typeReference">The TypeRef to resolve</param>
        /// <returns>The resolved EntryKey or null if not found.</returns>
        private (EntryKey, string) CreateEntryKey(TypeRef typeReference)
        {
            EntryKey typeKey = null;
            string typeName = typeReference.GetDisplayName(false);
            if (typeReference is TypeDef) // TypeDef as it is defined in same library as method
            {
                typeKey = new EntryKey(typeReference.GetGloballyUniqueId());
            }
            else
            {
                // check all other loaded libraries to try and resolve type
                CRefPath path = new CRefPath(typeReference);
                Documentation.Entry found = LiveDocumentorFile.Singleton.LiveDocument.Find(path);
                if (found != null)
                {
                    typeKey = new EntryKey(found.Key);
                    typeName = found.Name;
                }
                else
                {
                    typeKey = null;
                }
            }
            return (typeKey, typeName);
        }
    }
}