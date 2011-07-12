using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter {
	using TheBoxSoftware.Documentation;
	using TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages;
	using TheBoxSoftware.Reflection.Comments;

	public class LiveDocumenterEntry : Entry {
		/// <summary>
		/// Initialises a new Entry instance
		/// </summary>
		/// <param name="item">The item that represents this entry</param>
		/// <param name="xmlComments">The XmlComments file</param>
		public LiveDocumenterEntry(object item, string displayName, XmlCodeCommentFile xmlComments)
			: base(item, displayName, xmlComments) {
		}

		/// <summary>
		/// Initialises a new instance of the Entry class.
		/// </summary>
		/// <param name="item">The item associated with the entry.</param>
		/// <param name="displayName">The display name of the entry.</param>
		/// <param name="xmlComments">The xml comments file for the assembly.</param>
		/// <param name="parent">The parent node.</param>
		public LiveDocumenterEntry(object item, string displayName, XmlCodeCommentFile xmlComments, Entry parent)
			: this(item, displayName, xmlComments) {
			this.Parent = parent;
		}

		/// <summary>
		/// The page to display for this entry
		/// </summary>
		public Page Page {
			get {
				Page toLoad = null;
				if (this.Name == "Members" || this.Name == "Constructors" || this.Name == "Component Diagram" || this.Name == "Operators") {
					toLoad = Page.Create(this.Item, this.Name, this.XmlCommentFile);
				}
				else {
					toLoad = Page.Create(this.Item, this.XmlCommentFile);
				}
				toLoad.Generate();
				return toLoad;
			}
		}

		/// <summary>
		/// Returns a path to an icon that represents this entry in the documentmap.
		/// </summary>
		public string IconPath {
			get {
				string path = Model.ElementIconConstants.GetIconPathFor(this.Item);
				if (string.IsNullOrEmpty(path) && this.Item is KeyValuePair<string, List<TheBoxSoftware.Reflection.TypeDef>>) {
					path = "Resources/ElementIcons/vsobject_namespace.png";
				}
				return string.IsNullOrEmpty(path) ? "Resources/default.png" : path;
			}
		}
	}
}