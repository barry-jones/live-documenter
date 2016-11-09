
namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements
{
    using System.Windows.Documents;

    public class Header2 : Paragraph
    {
        public Header2(string title) : base(new Run(title))
        {
            this.Resources.MergedDictionaries.Add(DocumentationResources.BaseResources);
        }
    }
}