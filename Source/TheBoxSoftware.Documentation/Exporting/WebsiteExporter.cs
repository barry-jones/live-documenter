using System;
using System.IO;
using System.Xml;
using Saxon.Api;
using TheBoxSoftware.Documentation.Exporting.Website;
using System.Collections.Generic;

namespace TheBoxSoftware.Documentation.Exporting 
{
	public class WebsiteExporter : Exporter 
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="WebsiteExporter"/> class.
		/// </summary>
		/// <param name="document">The document to be exported.</param>
		/// <param name="config">The export configuration.</param>
		public WebsiteExporter(Document document, ExportSettings settings, ExportConfigFile config) 
			: base(document, settings, config) 
        {
		}

		/// <summary>
		/// Exports the full contained documentation.
		/// </summary>
		public override void Export() 
        {
			// we do not need the temp staging folder with this export so write direct from temp to publish.
			try 
            {
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

				// export the documents and map as XML
				if (!this.IsCancelled) 
                {
					this.OnExportStep(new ExportStepEventArgs("Export as XML...", ++this.CurrentExportStep));
					using (XmlWriter writer = XmlWriter.Create(string.Format("{0}/toc.xml", this.TempDirectory))) 
                    {
						Rendering.DocumentMapXmlRenderer map = new Rendering.DocumentMapXmlRenderer(this.Document.Map);
						map.Render(writer);
					}

					// export the index page
					IndexXmlRenderer indexPage = new IndexXmlRenderer(this.Document.Map);
					using (XmlWriter writer = XmlWriter.Create(string.Format("{0}/index.xml", this.TempDirectory))) 
                    {
						indexPage.Render(writer);
					}

					// export each of the members
					foreach (Entry current in this.Document.Map) 
                    {
						this.RecursiveEntryExport(current);
						this.OnExportStep(new ExportStepEventArgs("Export as XML...", ++this.CurrentExportStep));
						if (this.IsCancelled) break;
					}
					GC.Collect();
				}

                // transform the produced XML to HTML
				if (!this.IsCancelled) 
                {
					Processor p = new Processor();
					using (Stream xsltStream = this.Config.GetXslt()) 
                    {
						XsltTransformer transform = p.NewXsltCompiler().Compile(xsltStream).Load();
						transform.SetParameter(new QName(new XmlQualifiedName("directory")), new XdmAtomicValue(System.IO.Path.GetFullPath(this.TempDirectory)));

						// set output files
						this.OnExportStep(new ExportStepEventArgs("Saving output files...", ++this.CurrentExportStep));
						this.Config.SaveOutputFilesTo(this.PublishDirectory);

						this.OnExportStep(new ExportStepEventArgs("Transforming XML...", ++this.CurrentExportStep));
						string extension = this.Config.Properties.ContainsKey("extension") ? this.Config.Properties["extension"] : "htm";
						int counter = 0;
						foreach (string current in Directory.GetFiles(this.TempDirectory)) 
                        {
							if (current.Substring(this.TempDirectory.Length) == "toc.xml")
								continue;
							using (FileStream fs = File.OpenRead(current)) 
                            {
								Serializer s = new Serializer();
								s.SetOutputFile(this.PublishDirectory + Path.GetFileNameWithoutExtension(current) + "." + extension);
								transform.SetInputStream(fs, new Uri(new Uri(System.Reflection.Assembly.GetExecutingAssembly().Location), this.TempDirectory));
								transform.Run(s);
								counter++;
								if (counter % this.XmlExportStep == 0) 
                                {
									this.OnExportStep(new ExportStepEventArgs("Transforming XML...", this.CurrentExportStep += 3));
								}
								s.Close();
							}
							if (this.IsCancelled) break;
						}
					}
				}

				// clean up the temp directory
				this.OnExportStep(new ExportStepEventArgs("Cleaning up", ++this.CurrentExportStep));
				this.Cleanup();
			}
			catch (Exception ex) 
            {
				this.Cleanup(); // attempt to clean up our mess before dying
				ExportException exception = new ExportException(ex.Message, ex);
				this.OnExportException(new ExportExceptionEventArgs(exception));
			}
		}

		/// <summary>
		/// Exports an individual <paramref name="entry"/>.
		/// </summary>
		/// <param name="entry"></param>
		public override Stream ExportMember(Entry entry) 
        {
			string filename = this.Export(entry);
			MemoryStream exportedMember = new MemoryStream();

			string extension = this.Config.Properties.ContainsKey("extension") ? this.Config.Properties["extension"] : "htm";
			Processor p = new Processor();
			using (Stream xsltStream = this.Config.GetXslt()) {
				XsltTransformer transform = p.NewXsltCompiler().Compile(xsltStream).Load();
				transform.SetParameter(new QName(new XmlQualifiedName("directory")), new XdmAtomicValue(System.IO.Path.GetFullPath(this.TempDirectory)));
				using (FileStream fs = File.OpenRead(filename)) {
					Serializer s = new Serializer();
					s.SetOutputStream(exportedMember);
					transform.SetInputStream(fs, new Uri(new Uri(System.Reflection.Assembly.GetExecutingAssembly().Location), this.TempDirectory));
					transform.Run(s);
					s.Close();
				}
			}

			return exportedMember;
		}

		/// <summary>
		/// Returns a collection of messages that describe any issues that this exporter has with
		/// running.
		/// </summary>
		/// <returns>The issues.</returns>
		public override List<Issue> GetIssues() 
        {
			return new List<Issue>();
		}
	}
}
