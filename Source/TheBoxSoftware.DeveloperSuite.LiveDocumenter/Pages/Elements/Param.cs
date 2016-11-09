
namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements
{
    using System.Collections.Generic;
    using System.Windows.Documents;

    /// <summary>
    /// Represents a link to a parameter reference for the commented type
    /// in the current document. This refers to the param xml code comment.
    /// </summary>
    public sealed class Param : Block
    {
        public Param(string name, List<Block> description)
        {
            this.Resources.MergedDictionaries.Add(DocumentationResources.BaseResources);
            this.Name = name;
            this.Description = description;
        }

        public string Name { get; set; }
        public List<Block> Description { get; set; }
    }
}