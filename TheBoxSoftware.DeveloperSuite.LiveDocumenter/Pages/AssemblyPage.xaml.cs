using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages {
	using TheBoxSoftware.Reflection;
	using TheBoxSoftware.Reflection.Comments;
	using TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements;

	/// <summary>
	/// Interaction logic for AssemblyPage.xaml
	/// </summary>
	public partial class AssemblyPage : Page {
		private AssemblyDef assembly;

		/// <summary>
		/// Initialises a new instance of the AssemblyPage class.
		/// </summary>
		/// <param name="assembly">The assembly to document.</param>
		/// <param name="xmlComments">The xml code comments file associated with the assembly.</param>
		public AssemblyPage(AssemblyDef assembly, XmlCodeCommentFile xmlComments) : base() {
			this.InitializeComponent();
			this.assembly = assembly;
		}

		/// <summary>
		/// Generates the contents of the page.
		/// </summary>
		public override void Generate() {
			if (!this.IsGenerated) {
				this.Blocks.Add(new Header1(System.IO.Path.GetFileName(assembly.File.FileName)));
				Paragraph versionDetails = new Paragraph();
				versionDetails.Inlines.Add(new Bold(new Run("Version: ")));
				versionDetails.Inlines.Add(new Run(assembly.Version.ToString()));
				this.Blocks.Add(versionDetails);

				// Output a list of namespaces in this assembly as links
				this.Blocks.Add(new Header2("Namespaces"));
				SummaryTable namespaces = new SummaryTable("Name", string.Empty, false, false);
				List<string> allNamespaces = this.assembly.GetNamespaces();

				// Sort the namespaces
				allNamespaces.Sort();

				int count = allNamespaces.Count;
				for (int i = 0; i < count; i++) {
					string current = allNamespaces[i];
					if (string.IsNullOrEmpty(current)) {
						continue;	// Skip the empty namespace defined in all assemblies
					}
					Hyperlink link = new Hyperlink(new Run(current));
					link.Tag = new CrefEntryKey(assembly, "N:" + current);
					link.Click += new RoutedEventHandler(LinkHelper.Resolve);
					namespaces.AddItem(link, string.Empty);
				}
				this.Blocks.Add(namespaces);

				this.IsGenerated = true;
			}
		}
	}
}
