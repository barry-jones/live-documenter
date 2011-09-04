using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages {
	using TheBoxSoftware.Documentation;

	/// <summary>
	/// A page that provides a deployment diagram built from the details about the
	/// files and references made in this document.
	/// </summary>
	public class DeploymentDiagram : Page {
		public DeploymentDiagram() {
		}

		public override void Generate() {
			List<DocumentedAssembly> assemblies = LiveDocumentorFile.Singleton.LiveDocument.Assemblies;

			this.Blocks.Add(new Elements.Header1("Deployment Diagram"));
			Diagram diagram = new Diagram();
			BlockUIContainer diagramContainer = new BlockUIContainer(diagram);
			this.Blocks.Add(diagramContainer);
		}
	}
}
