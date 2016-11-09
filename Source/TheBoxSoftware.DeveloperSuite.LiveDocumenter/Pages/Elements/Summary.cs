
namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements
{
    using System.Collections.Generic;
    using System.Windows.Documents;

    /// <summary>
    /// Rerpesents a summary of an element in the current document. This
    /// relates to the summary xml code comment element.
    /// </summary>
    public sealed class Summary : Section
    {
        public Summary(List<Block> blocks)
        {
            this.Resources.MergedDictionaries.Add(DocumentationResources.BaseResources);
            this.Blocks.AddRange(blocks);
        }
    }
}