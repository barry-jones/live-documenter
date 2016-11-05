
namespace TheBoxSoftware.Documentation
{
    using System.Collections.Generic;
    using TheBoxSoftware.Reflection;

    /// <summary>
    /// Stores the settings which describe how the documentation should be produced and viewed.
    /// </summary>
    public sealed class DocumentSettings
    {
        private List<Visibility> _filters;

        // TODO: use for both export and live, allow live to be used as export (checkbox on settings)
        // TODO: implement other things such as inherited members, inherited documentation settings ala sandcastle

        /// <summary>
        /// Initialises a new instance of the DocumentSettings class.
        /// </summary>
        public DocumentSettings()
        {
            _filters = new List<Visibility>();
        }

        /// <summary>
        /// A list of Visibility flags on types and members which should be visible.
        /// </summary>
        public List<Visibility> VisibilityFilters
        {
            get { return _filters; }
            set { _filters = value; }
        }
    }
}