
namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements
{
    using System.Windows;
    using System.Windows.Documents;

    /// <summary>
    /// Represents a code formatted text that appears inline in the same
    /// run it is defined in the current document. This refers to the c
    /// code element in the code comments.
    /// </summary>
    public sealed class C : Run
    {
        public C(string code)
            : base(code)
        {
            this.Initialise();
        }

        private void Initialise()
        {
            this.Resources.MergedDictionaries.Add(DocumentationResources.BaseResources);
            this.Style = this.TryFindResource("C") as Style;
        }
    }
}