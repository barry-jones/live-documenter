
namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter
{
    using System;
    using System.Collections.Generic;
    using TheBoxSoftware.Documentation;
    using TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages;
    using TheBoxSoftware.Reflection.Comments;

    /// <summary>
    /// An implementation of <see cref="Entry"/> specific for the LiveDocument DocumentMaps.
    /// </summary>
    internal class LiveDocumenterEntry : Entry
    {
        /// <summary>
        /// Initialises a new Entry instance
        /// </summary>
        /// <param name="item">The item that represents this entry</param>
        /// <param name="displayName">The display name for this Entry.</param>
        /// <param name="xmlComments">The XmlComments file</param>
        public LiveDocumenterEntry(object item, string displayName, ICommentSource xmlComments)
            : base(item, displayName, xmlComments)
        {
        }

        /// <summary>
        /// Initialises a new instance of the Entry class.
        /// </summary>
        /// <param name="item">The item associated with the entry.</param>
        /// <param name="displayName">The display name of the entry.</param>
        /// <param name="xmlComments">The xml comments file for the assembly.</param>
        /// <param name="parent">The parent node.</param>
        public LiveDocumenterEntry(object item, string displayName, ICommentSource xmlComments, Entry parent)
            : this(item, displayName, xmlComments)
        {
            this.Parent = parent;
        }

        /// <summary>
        /// The page to display for this entry
        /// </summary>
        public Page Page
        {
            get
            {
                Page toLoad = null;
                if(Name == "Members" || Name == "Constructors" || Name == "Component Diagram" || Name == "Operators")
                {
                    toLoad = Page.Create(this, Name, XmlCommentFile);
                }
                else
                {
                    toLoad = Page.Create(this, XmlCommentFile);
                }

                Exception generateException = null;
                try
                {
                    toLoad.Generate();
                }
                catch(Exception ex)
                {
                    generateException = ex;
                }
                if(generateException != null)
                {
                    toLoad = new ErrorPage();
                    Diagnostics.ErrorReporting reporting = new Diagnostics.ErrorReporting();
                    reporting.SetException(generateException);
                    reporting.Show();
                }
                return toLoad;
            }
        }

        /// <summary>
        /// Returns a path to an icon that represents this entry in the documentmap.
        /// </summary>
        public string IconPath
        {
            get
            {
                string path = Model.ElementIconConstants.GetIconPathFor(this.Item);
                if(string.IsNullOrEmpty(path) && this.Item is KeyValuePair<string, List<TheBoxSoftware.Reflection.TypeDef>>)
                {
                    path = "Resources/ElementIcons/vsobject_namespace.png";
                }
                return string.IsNullOrEmpty(path) ? "Resources/default.png" : path;
            }
        }
    }
}