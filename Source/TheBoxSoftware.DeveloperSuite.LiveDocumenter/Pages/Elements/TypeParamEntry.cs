
namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements
{
    using System.Windows.Documents;

    public class TypeParamEntry : Block
    {
        public TypeParamEntry(string name, string description)
        {
            this.Resources.MergedDictionaries.Add(DocumentationResources.BaseResources);
            this.Param = name;
            this.Description = description;
        }

        public string Param { get; set; }

        public string Description { get; set; }
    }
}