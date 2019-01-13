
namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements
{
    using System.Collections.Generic;
    using System.Windows.Documents;

    /// <summary>
    /// This refers to a comment about the return value for the current
    /// element in the current document. This relates to the returns xml
    /// code comment element.
    /// </summary>
    public sealed class Returns : Section
    {
        public Returns(List<Block> children)
        {
            Resources.MergedDictionaries.Add(DocumentationResources.BaseResources);
            Blocks.AddRange(children);
        }
    }
}