
namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements
{
    public class Keyword : System.Windows.Documents.Run
    {
        public Keyword() : base()
        {
            this.Initialise();
        }

        public Keyword(string content) : base(content)
        {
            this.Initialise();
        }

        private void Initialise()
        {
            this.Resources.MergedDictionaries.Add(DocumentationResources.BaseResources);
        }
    }
}