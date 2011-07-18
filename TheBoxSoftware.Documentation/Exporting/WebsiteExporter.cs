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
		private string tempdirectory = string.Empty;
		private ExportConfigFile config = null;
		private int currentExportStep = 1;

		/// <summary>
		/// Initializes a new instance of the <see cref="WebsiteExporter"/> class.
		/// </summary>
		/// <param name="assemblies">The assemblies.</param>
		public WebsiteExporter(List<DocumentedAssembly> assemblies, ExportSettings settings, ExportConfigFile config) 
			: base(assemblies, settings) {
			string regex = string.Format("{0}{1}",
				 new string(Path.GetInvalidFileNameChars()),
				 new string(Path.GetInvalidPathChars()));
			illegalFileCharacters = new System.Text.RegularExpressions.Regex(
				string.Format("[{0}]", System.Text.RegularExpressions.Regex.Escape(regex))
				);
			this.config = config;
		}

		/// <summary>
		/// Exports the full contained documentation.
		/// </summary>
		public override void Export() {
			// the temp output directory
			this.tempdirectory = System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetTempFileName()) + "\\";

			try {
				this.PrepareDirectory(tempdirectory);

				this.DocumentMap = DocumentMapper.Generate(this.CurrentFiles, Mappers.NamespaceFirst, this.Settings.DocumentSettings, false); ;
				this.OnExportCalculated(new ExportCalculatedEventArgs(6));
				this.currentExportStep = 1;

				Documentation.Exporting.Rendering.DocumentMapXmlRenderer map = new Documentation.Exporting.Rendering.DocumentMapXmlRenderer(
					this.DocumentMap
					);

				// export the document map
				this.OnExportStep(new ExportStepEventArgs("Export as XML...", ++this.currentExportStep));
				using (XmlWriter writer = XmlWriter.Create(string.Format("{0}/toc.xml", this.tempdirectory))) {
					map.Render(writer);
				}				

				// export the index page
				IndexXmlRenderer indexPage = new IndexXmlRenderer(this.DocumentMap);
				using (XmlWriter writer = XmlWriter.Create(string.Format("{0}/index.xml", this.tempdirectory))) {
					indexPage.Render(writer);
				}

				// export each of the members
				foreach (Entry current in this.DocumentMap) {
					this.RecursiveEntryExport(current);
				}

				Processor p = new Processor();
				Uri xsltLocation = new Uri(new Uri(System.Reflection.Assembly.GetExecutingAssembly().Location), "ApplicationData/livexmltohtml.xslt");
				XsltTransformer transform = p.NewXsltCompiler().Compile(this.config.GetXslt()).Load();
				transform.SetParameter(new QName(new XmlQualifiedName("directory")), new XdmAtomicValue(System.IO.Path.GetFullPath(tempdirectory)));

				// Finally perform the user selected output xslt
				this.OnExportStep(new ExportStepEventArgs("Preparing output directory", ++this.currentExportStep));
				this.PrepareDirectory(@"temp\output");

				// set output files
				this.OnExportStep(new ExportStepEventArgs("Saving output files...", ++this.currentExportStep));
				this.config.SaveOutputFilesTo(@"temp\output");

				this.OnExportStep(new ExportStepEventArgs("Transforming XML...", ++this.currentExportStep));
				foreach (string current in Directory.GetFiles(this.tempdirectory)) {
					if (current == "toc.xml")
						continue;
					using (FileStream fs = File.OpenRead(current)) {
						Serializer s = new Serializer();
						s.SetOutputFile(@"temp\output\" + Path.GetFileNameWithoutExtension(current) + ".htm");
						transform.SetInputStream(fs, new Uri(new Uri(System.Reflection.Assembly.GetExecutingAssembly().Location), @"temp\output\"));
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

		/// <summary>
		/// A method that recursively, through the documentation tree, exports all of the
		/// found pages for each of the entries in the documentation.
		/// </summary>
		/// <param name="currentEntry">The current entry to export</param>
		private void RecursiveEntryExport(Entry currentEntry) {
			this.Export(currentEntry);
			for (int i = 0; i < currentEntry.Children.Count; i++) {
				this.RecursiveEntryExport(currentEntry.Children[i]);
			}
		}

		private void Export(Entry current) {
			System.Diagnostics.Debug.WriteLine("SaveXaml: " + current.Name + "[" + current.Key + ", " + current.SubKey + "]");
			System.Diagnostics.Debug.Indent();

			Rendering.XmlRenderer r = Rendering.XmlRenderer.Create(current, this);

			if (r != null) {
				string filename = string.Format("{0}{1}{2}.xml", this.tempdirectory, current.Key, string.IsNullOrEmpty(current.SubKey) ? string.Empty : "-" + this.illegalFileCharacters.Replace(current.SubKey, string.Empty));
				System.Diagnostics.Debug.WriteLine("filename: " + filename);
				using (System.Xml.XmlWriter writer = XmlWriter.Create(filename)) {
					r.Render(writer);
				}
				System.Diagnostics.Debug.WriteLine("File: " + filename);
			}

			System.Diagnostics.Debug.Unindent();
		}

		/// <summary>
		/// Ensures there is an empty temp directory to proccess the documentation in.
		/// </summary>
		private void PrepareDirectory(string directory) {
			if (!Directory.Exists(directory)) {
				Directory.CreateDirectory(directory);
			}
			else {
				Directory.Delete(directory, true);
				Directory.CreateDirectory(directory);
			}
		}
	}
}
