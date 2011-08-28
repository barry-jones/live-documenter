using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using Saxon.Api;

namespace TheBoxSoftware.Documentation.Exporting {
	using TheBoxSoftware.Documentation;

	/// <summary>
	/// Exports documentation in the MS Help Viewer 1 format.
	/// </summary>
	public class HelpViewer1Exporter : Exporter {
		/// <summary>
		/// Initialises a new instance of the HelpViewer1Exporter class.
		/// </summary>
		/// <param name="document">The document to export.</param>
		/// <param name="config">The export configuration.</param>
		public HelpViewer1Exporter(Document document, ExportSettings settings, ExportConfigFile config)
			: base(document, settings, config) {
		}

		/// <summary>
		/// Exports the documentation to the MS Help Viewer 1 documention type.
		/// </summary>
		public override void Export() {
			try {
				this.PrepareForExport();

				this.OnExportCalculated(new ExportCalculatedEventArgs(6));
				this.CurrentExportStep = 1;

				Documentation.Exporting.Rendering.DocumentMapXmlRenderer map = new Documentation.Exporting.Rendering.DocumentMapXmlRenderer(
					this.Document.Map
					);

				// export the document map
				this.OnExportStep(new ExportStepEventArgs("Export as XML...", ++this.CurrentExportStep));
				using (XmlWriter writer = XmlWriter.Create(string.Format("{0}/toc.xml", this.TempDirectory))) {
					map.Render(writer);
				}

				Website.IndexXmlRenderer indexPage = new Website.IndexXmlRenderer(this.Document.Map);
				using (XmlWriter writer = XmlWriter.Create(string.Format("{0}/index.xml", this.TempDirectory))) {
					indexPage.Render(writer);
				}

				// export each of the members
				foreach (Entry current in this.Document.Map) {
					this.RecursiveEntryExport(current);
				}

				Processor p = new Processor();
				XsltTransformer transform = p.NewXsltCompiler().Compile(this.Config.GetXslt()).Load();
				transform.SetParameter(new QName(new XmlQualifiedName("directory")), new XdmAtomicValue(System.IO.Path.GetFullPath(this.TempDirectory)));

				// set output files
				this.OnExportStep(new ExportStepEventArgs("Saving output files...", ++this.CurrentExportStep));
				this.Config.SaveOutputFilesTo(this.OutputDirectory);

				this.OnExportStep(new ExportStepEventArgs("Transforming XML...", ++this.CurrentExportStep));

				// export the content files
				foreach (string current in Directory.GetFiles(this.TempDirectory)) {
					if (current.Substring(this.TempDirectory.Length) == "toc.xml")
						continue;
					using (FileStream fs = File.OpenRead(current)) {
						Serializer s = new Serializer();
						s.SetOutputFile(this.OutputDirectory + Path.GetFileNameWithoutExtension(current) + ".htm");
						transform.SetInputStream(fs, new Uri(new Uri(System.Reflection.Assembly.GetExecutingAssembly().Location), this.OutputDirectory));
						transform.Run(s);
					}
				}

				// compile the html help file
				this.OnExportStep(new ExportStepEventArgs("Compiling help...", ++this.CurrentExportStep));
				this.CompileHelp(this.OutputDirectory + "\\Documentation.mshc");
				File.Copy(this.ApplicationDirectory + "\\ApplicationData\\Documentation.msha", this.OutputDirectory + "\\Documentation.msha");

				// publish the documentation
				this.OnExportStep(new ExportStepEventArgs("Publishing help...", ++this.CurrentExportStep));
				string[] files = { "Documentation.mshc", "Documentation.msha" };
				for (int i = 0; i < files.Length; i++) {
					File.Move(
						Path.Combine(this.OutputDirectory, files[i]),
						Path.Combine(this.PublishDirectory, files[i])
						); ;
				}
			}
			catch (Exception ex) {
				ExportException exception = new ExportException(ex.Message, ex);
				this.OnExportException(new ExportExceptionEventArgs(exception));
			}
			finally {
				// clean up the temp directory
				this.OnExportStep(new ExportStepEventArgs("Cleaning up", ++this.CurrentExportStep));
				this.Cleanup();
			}
		}

		/// <summary>
		/// Compiles and creates the Help Viewer 1 mshc file.
		/// </summary>
		/// <param name="projectFile">The HxC file.</param>
		private void CompileHelp(string projectFile) {
			// zip up the content director and rename it
			Ionic.Zip.ZipFile outputFile = new Ionic.Zip.ZipFile();
			outputFile.AddDirectory(this.OutputDirectory);
			outputFile.Save(projectFile);

			// drop the installer files
		}
	}
}
