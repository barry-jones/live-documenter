using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements {
	using TheBoxSoftware.Reflection;
	using TheBoxSoftware.Reflection.Comments;

	/// <summary>
	/// Represents a link that is to be displayed in a see also section
	/// of the document. This currently relates to the seealso xml code
	/// comment element.
	/// </summary>
	public sealed class SeeAlso : Block {
		/// <summary>
		/// Initialises a SeeAlso instance.
		/// </summary>
		/// <param name="assembly">The assembly the currently being documented</param>
		/// <param name="type">The <see cref="CRefPath"/> to the type being refered to</param>
		public SeeAlso(AssemblyDef assembly, CRefPath type) {
			TypeDef def = assembly.FindType(type.Namespace, type.TypeName);
			string displayName = type.TypeName;
			if (def != null) displayName = def.GetDisplayName(false);

			Hyperlink link = new Hyperlink(new Run(displayName));
			link.Tag = new CrefEntryKey(assembly, type.ToString());
			link.Click += new System.Windows.RoutedEventHandler(LinkHelper.Resolve);
			this.Link = link;
		}

		/// <summary>
		/// The link to the type being referenced in the see tag
		/// </summary>
		public Hyperlink Link { get; set; }
	}
}
