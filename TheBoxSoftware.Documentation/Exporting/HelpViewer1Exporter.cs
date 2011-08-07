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
		/// <param name="files">The files to document.</param>
		/// <param name="settings">The export settings.</param>
		/// <param name="config">The export configuration.</param>
		public HelpViewer1Exporter(List<DocumentedAssembly> files, ExportSettings settings, ExportConfigFile config)
			: base(files, settings, config) {
		}

		/// <summary>
		/// Exports the documentation to the MS Help Viewer 1 documention type.
		/// </summary>
		public override void Export() {
			// the temp output directory
			this.TempDirectory = System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetTempFileName()) + "\\";
			this.OutputDirectory = @"temp\output\";

			try {
				this.PrepareDirectory(this.TempDirectory);

				this.DocumentMap = DocumentMapper.Generate(this.CurrentFiles, Mappers.NamespaceFirst, this.Settings.DocumentSettings, false, new EntryCreator());
				this.OnExportCalculated(new ExportCalculatedEventArgs(7));
				this.CurrentExportStep = 1;

				Documentation.Exporting.Rendering.DocumentMapXmlRenderer map = new Documentation.Exporting.Rendering.DocumentMapXmlRenderer(
					this.DocumentMap
					);

				// export the document map
				this.OnExportStep(new ExportStepEventArgs("Export as XML...", ++this.CurrentExportStep));
				using (XmlWriter writer = XmlWriter.Create(string.Format("{0}/toc.xml", this.TempDirectory))) {
					map.Render(writer);
				}

				// export each of the members
				foreach (Entry current in this.DocumentMap) {
					this.RecursiveEntryExport(current);
				}

				Processor p = new Processor();
				XsltTransformer transform = p.NewXsltCompiler().Compile(this.Config.GetXslt()).Load();
				transform.SetParameter(new QName(new XmlQualifiedName("directory")), new XdmAtomicValue(System.IO.Path.GetFullPath(this.TempDirectory)));

				// Finally perform the user selected output xslt
				this.OnExportStep(new ExportStepEventArgs("Preparing output directory", ++this.CurrentExportStep));
				this.PrepareDirectory(this.OutputDirectory);

				// set output files
				this.OnExportStep(new ExportStepEventArgs("Saving output files...", ++this.CurrentExportStep));
				this.Config.SaveOutputFilesTo(this.OutputDirectory);

				this.OnExportStep(new ExportStepEventArgs("Transforming XML...", ++this.CurrentExportStep));

				// export the project xml, we cant render the XML because the DTD protocol causes loads of probs with Saxon
				//CollectionXmlRenderer collectionXml = new CollectionXmlRenderer(this.DocumentMap, string.Empty);
				//using (XmlWriter writer = XmlWriter.Create(string.Format("{0}/Documentation.HxC", this.OutputDirectory))) {
				//    collectionXml.Render(writer);
				//}

				// export the incldue file xml
				//IncludeFileXmlRenderer includeXml = new IncludeFileXmlRenderer(this.Config);
				//using (XmlWriter writer = XmlWriter.Create(string.Format("{0}/Documentation.HxF", this.OutputDirectory))) {
				//    includeXml.Render(writer);
				//}

				// export the content file
				//using (FileStream fs = File.OpenRead(string.Format("{0}/toc.xml", this.TempDirectory))) {
				//    Serializer s = new Serializer();
				//    s.SetOutputFile(this.OutputDirectory + "Documentation.HxT");
				//    transform.SetInputStream(fs, new Uri(new Uri(System.Reflection.Assembly.GetExecutingAssembly().Location), this.OutputDirectory));
				//    transform.Run(s);
				//}

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
				this.CompileHelp(this.OutputDirectory + "/Documentation.HxC");
			}
			catch (Exception ex) {
				ExportException exception = new ExportException(ex.Message, ex);
				this.OnExportException(new ExportExceptionEventArgs(exception));
			}
			finally {
				// clean up the temp directory
				this.OnExportStep(new ExportStepEventArgs("Cleaning up", ++this.CurrentExportStep));
#if !DEBUG
				System.IO.Directory.Delete(this.tempdirectory, true);
#endif
			}
		}

		/// <summary>
		/// Compiles and creates the Help Viewer 1 mshc file.
		/// </summary>
		/// <param name="projectFile">The HxC file.</param>
		private void CompileHelp(string projectFile) {
			
		}
	}
}
