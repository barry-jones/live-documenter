using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using TheBoxSoftware.Reflection;
using TheBoxSoftware.Reflection.Core;
using TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages;
using TheBoxSoftware.Reflection.Comments;
using TheBoxSoftware.Documentation;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter 
{
	/// <summary>
	/// Represents a live document, which is a collection of pages which display
	/// code information and diagrams etc. For all of the loaded files for the
	/// current LiveDocumentFile projects.
	/// </summary>
	internal sealed class LiveDocument : Document 
    {
		/// <summary>
		/// Initialises a new instance of the LiveDocument class.
		/// </summary>
		/// <param name="assemblies">The assemblies to document.</param>
		/// <param name="filters">The visibility filters.</param>
		public LiveDocument(List<DocumentedAssembly> assemblies, List<Reflection.Visibility> filters)
			: base(assemblies, Mappers.GroupedNamespaceFirst, true, new LiveDocumenterEntryCreator())
        {
			DocumentSettings settings = new DocumentSettings();
			settings.VisibilityFilters.AddRange(filters);
			this.Settings = settings;
		}

		/// <summary>
		/// Updates the contents of the live document for the current list of
		/// file references.
		/// </summary>
		public void Update() 
        {
			this.Map = this.Mapper.GenerateMap();
		}

		/// <summary>
		/// Refreshes the specified assemblies documentation in the UI.
		/// </summary>
		/// <param name="documentedAssembly">The assembly whose documentation needs updating.</param>
		/// <remarks>
		/// <para>
		/// When an assembly is refreshed the <see cref="DocumentMapper"/> is used to regenerate the
		/// document map for that assembly.
		/// </para>
		/// <para>
		/// If the assembly was not compiled before the refresh, is has its document map generated
		/// and is then inserted in to the current <see cref="DocumentMap"/>.
		/// </para>
		/// </remarks>
		public void RefreshAssembly(DocumentedAssembly documentedAssembly) 
        {
            this.Map = this.Mapper.GenerateMap();
		}
	}
}