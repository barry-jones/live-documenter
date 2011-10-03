using System;
using System.IO;
using System.Xml;
using TheBoxSoftware.Documentation.Exporting.Website;
using System.Collections.Generic;

namespace TheBoxSoftware.Documentation.Exporting {
	/// <summary>
	/// Exporter that exports the docuementation to XML.
	/// </summary>
	public class XmlExporter : Exporter {
		/// <summary>
		/// Initializes a new instance of the XmlExporter class.
		/// </summary>
		/// <param name="document">The document to be exported.</param>
		/// <param name="settings">Any settings for the export.</param>
		/// <param name="config">The export configuration.</param>
		public XmlExporter(Document document, ExportSettings settings, ExportConfigFile config) 
			: base(document, settings, config) {
		}

		/// <summary>
		/// Exports the full contained documentation.
		/// </summary>
		public override void Export() {
			// we do not need the temp staging folder with this export so write direct from temp to publish.
			try {
				this.PrepareForExport();

				// calculate the export steps
				int numberOfSteps = 0;
				numberOfSteps += 1; // toc and index steps
				numberOfSteps += this.Document.Map.Count; // top level entries for recursive export
				numberOfSteps += 1; // output files
				numberOfSteps += ((this.Document.Map.NumberOfEntries / this.XmlExportStep) * 3); // xml export stage
				numberOfSteps += 1; // cleanup files

				this.OnExportCalculated(new ExportCalculatedEventArgs(numberOfSteps));
				this.CurrentExportStep = 1;

				// export the document map
				if (!this.IsCancelled) {
					this.OnExportStep(new ExportStepEventArgs("Export as XML...", ++this.CurrentExportStep));
					using (XmlWriter writer = XmlWriter.Create(string.Format("{0}/toc.xml", this.TempDirectory))) {
						Rendering.DocumentMapXmlRenderer map = new Rendering.DocumentMapXmlRenderer(this.Document.Map);
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
						this.OnExportStep(new ExportStepEventArgs("Export as XML...", ++this.CurrentExportStep));
						if (this.IsCancelled) break;
					}
					GC.Collect();
				}

				if (!this.IsCancelled) {
					// set output files
					this.OnExportStep(new ExportStepEventArgs("Saving output files...", ++this.CurrentExportStep));
					this.Config.SaveOutputFilesTo(this.PublishDirectory);

					// move all the temporary export files to the publish directory
					int counter = 0;
					foreach (string file in Directory.GetFiles(this.TempDirectory)) {
						File.Copy(file, Path.Combine(this.PublishDirectory, Path.GetFileName(file)));
						if (counter % this.XmlExportStep == 0) {
							this.OnExportStep(new ExportStepEventArgs("Publishing XML files...", this.CurrentExportStep += 3));
						}
					}
				}

				// clean up the temp directory
				this.OnExportStep(new ExportStepEventArgs("Cleaning up", ++this.CurrentExportStep));
				this.Cleanup();
			}
			catch (Exception ex) {
				this.Cleanup(); // attempt to clean up our mess before dying
				ExportException exception = new ExportException(ex.Message, ex);
				this.OnExportException(new ExportExceptionEventArgs(exception));
			}
		}

		/// <summary>
		/// Exports an individual <paramref name="entry"/>.
		/// </summary>
		/// <param name="entry"></param>
		public override Stream ExportMember(Entry entry) {
			string filename = this.Export(entry);

			MemoryStream exportedMember = new MemoryStream();
			StreamWriter writer = new StreamWriter(exportedMember);
			writer.Write(File.ReadAllText(filename));
			return exportedMember;
		}

		/// <summary>
		/// Returns a collection of messages that describe any issues that this exporter has with
		/// running.
		/// </summary>
		/// <returns>The issues.</returns>
		public override List<Issue> GetIssues() {
			return new List<Issue>();
		}
	}
}
