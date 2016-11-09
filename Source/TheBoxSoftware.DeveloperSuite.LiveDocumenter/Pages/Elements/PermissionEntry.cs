
namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements
{
    using System.Collections.Generic;
    using System.Windows.Documents;

    /// <summary>
    /// Represents the visualisation of a <see cref="PermissionXmlCodeElement"/>.
    /// </summary>
    internal class PermissionEntry : Block
    {
        /// <summary>
        /// Initialises a new instance of the PermissionEntry class.
        /// </summary>
        /// <param name="description">A block level element that contains the description of the permission.</param>
        /// <param name="name">Text or Hyperlink display name of the Permission.</param>
        public PermissionEntry(Inline name, List<Block> description)
        {
            this.Resources.MergedDictionaries.Add(DocumentationResources.BaseResources);
            this.DisplayName = name;
            this.Description = description;
        }

        /// <summary>
        /// The display name of the permission that permission entry describes.
        /// </summary>
        public Inline DisplayName { get; set; }

        /// <summary>
        /// A visualisation of the XML comments descriptive text.
        /// </summary>
        public List<Block> Description { get; set; }
    }
}