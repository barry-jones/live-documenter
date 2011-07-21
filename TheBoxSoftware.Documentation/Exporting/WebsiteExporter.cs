using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Saxon.Api;
using System.IO;

namespace TheBoxSoftware.Documentation.Exporting {
	using TheBoxSoftware.Reflection;
	using TheBoxSoftware.Reflection.Comments;
	using TheBoxSoftware.Documentation.Exporting.Website;

	public class WebsiteExporter : Exporter {
		private System.Text.RegularExpressions.Regex illegalFileCharacters;
		private ExportConfigFile config = null;
		private int currentExportStep = 1;

		/// <summary>
		/// Initializes a new instance of the <see cref="WebsiteExporter"/> class.
		/// </summary>
		/// <param name="assemblies">The assemblies.</param>
		public WebsiteExporter(List<DocumentedAssembly> assemblies, ExportSettings settings, ExportConfigFile config) 
			: base(assemblies, settings) {
			this.config = config;
		}

		/// <summary>
		/// Exports the full contained documentation.
		/// </summary>
		public override void Export() {
			// the temp output directory
			this.TempDirectory = System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetTempFileName()) + "\\";
			this.OutputDirectory = @"temp\output\";

			try {
				this.PrepareDirectory(this.TempDirectory);

				this.DocumentMap = DocumentMapper.Generate(this.CurrentFiles, Mappers.NamespaceFirst, this.Settings.DocumentSettings, false, new EntryCreator());
				this.OnExportCalculated(new ExportCalculatedEventArgs(6));
				this.currentExportStep = 1;

				Documentation.Exporting.Rendering.DocumentMapXmlRenderer map = new Documentation.Exporting.Rendering.DocumentMapXmlRenderer(
					this.DocumentMap
					);

				// export the document map
				this.OnExportStep(new ExportStepEventArgs("Export as XML...", ++this.currentExportStep));
				using (XmlWriter writer = XmlWriter.Create(string.Format("{0}/toc.xml", this.TempDirectory))) {
					map.Render(writer);
				}				

				// export the index page
				IndexXmlRenderer indexPage = new IndexXmlRenderer(this.DocumentMap);
				using (XmlWriter writer = XmlWriter.Create(string.Format("{0}/index.xml", this.TempDirectory))) {
					indexPage.Render(writer);
				}

				// export each of the members
				foreach (Entry current in this.DocumentMap) {
					this.RecursiveEntryExport(current);
				}

				Processor p = new Processor();
				XsltTransformer transform = p.NewXsltCompiler().Compile(this.config.GetXslt()).Load();
				transform.SetParameter(new QName(new XmlQualifiedName("directory")), new XdmAtomicValue(System.IO.Path.GetFullPath(this.TempDirectory)));

				// Finally perform the user selected output xslt
				this.OnExportStep(new ExportStepEventArgs("Preparing output directory", ++this.currentExportStep));
				this.PrepareDirectory(this.OutputDirectory);

				// set output files
				this.OnExportStep(new ExportStepEventArgs("Saving output files...", ++this.currentExportStep));
				this.config.SaveOutputFilesTo(this.OutputDirectory);

				this.OnExportStep(new ExportStepEventArgs("Transforming XML...", ++this.currentExportStep));
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
			}
			catch (Exception ex) {
				ExportException exception = new ExportException(ex.Message, ex);
				this.OnExportException(new ExportExceptionEventArgs(exception));
			}
			finally {
				// clean up the temp directory
				this.OnExportStep(new ExportStepEventArgs("Cleaning up", ++this.currentExportStep));
#if !DEBUG
				System.IO.Directory.Delete(this.tempdirectory, true);
#endif
			}
		}
	}
}
