
namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements
{
    using System.Windows;
    using System.Windows.Documents;

    /// <summary>
    /// Represents a section of text that is to be displayed as code
    /// in the document. Currently this relates to the c and code xml
    /// comment types.
    /// </summary>
    public sealed class Code : Paragraph
    {
        public Code()
        {
            this.Initialise();
        }
        public Code(string code)
        {
            this.Initialise();
            this.Inlines.Add(new Run(code));
        }

        private void Initialise()
        {
            this.Resources.MergedDictionaries.Add(DocumentationResources.BaseResources);
            this.Style = (Style)this.FindResource("Code");
        }
    }
}