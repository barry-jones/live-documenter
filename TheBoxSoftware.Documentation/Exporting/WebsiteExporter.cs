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
		/// <summary>
		/// Initializes a new instance of the <see cref="WebsiteExporter"/> class.
		/// </summary>
		/// <param name="document">The document to be exported.</param>
		/// <param name="config">The export configuration.</param>
		public WebsiteExporter(Document document, ExportSettings settings, ExportConfigFile config) 
			: base(document, settings, config) {
		}

		/// <summary>
		/// Exports the full contained documentation.
		/// </summary>
		public override void Export() {
			// we do not need the temp staging folder with this export so write direct from temp to publish.
			try {
				this.PrepareForExport();

				this.OnExportCalculated(new ExportCalculatedEventArgs(5));
				this.CurrentExportStep = 1;

				Documentation.Exporting.Rendering.DocumentMapXmlRenderer map = new Documentation.Exporting.Rendering.DocumentMapXmlRenderer(
					this.Document.Map
					);

				// export the document map
				this.OnExportStep(new ExportStepEventArgs("Export as XML...", ++this.CurrentExportStep));
				using (XmlWriter writer = XmlWriter.Create(string.Format("{0}/toc.xml", this.TempDirectory))) {
					map.Render(writer);
				}				

				// export the index page
				IndexXmlRenderer indexPage = new IndexXmlRenderer(this.Document.Map);
				using (XmlWriter writer = XmlWriter.Create(string.Format("{0}/index.xml", this.TempDirectory))) {
					indexPage.Render(writer);
				}

				// export each of the members
				foreach (Entry current in this.Document.Map) {
					this.RecursiveEntryExport(current);
				}
				GC.Collect();

				Processor p = new Processor();
				XsltTransformer transform = p.NewXsltCompiler().Compile(this.Config.GetXslt()).Load();
				transform.SetParameter(new QName(new XmlQualifiedName("directory")), new XdmAtomicValue(System.IO.Path.GetFullPath(this.TempDirectory)));

				// set output files
				this.OnExportStep(new ExportStepEventArgs("Saving output files...", ++this.CurrentExportStep));
				this.Config.SaveOutputFilesTo(this.PublishDirectory);

				this.OnExportStep(new ExportStepEventArgs("Transforming XML...", ++this.CurrentExportStep));
				string extension = this.Config.Properties.ContainsKey("extension") ? this.Config.Properties["extension"] : "htm";
				foreach (string current in Directory.GetFiles(this.TempDirectory)) {
					if (current.Substring(this.TempDirectory.Length) == "toc.xml")
						continue;
					using (FileStream fs = File.OpenRead(current)) {
						Serializer s = new Serializer();
						s.SetOutputFile(this.PublishDirectory + Path.GetFileNameWithoutExtension(current) + "." + extension);
						transform.SetInputStream(fs, new Uri(new Uri(System.Reflection.Assembly.GetExecutingAssembly().Location), this.TempDirectory));
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
				this.OnExportStep(new ExportStepEventArgs("Cleaning up", ++this.CurrentExportStep));
				this.Cleanup();
			}
		}
	}
}
