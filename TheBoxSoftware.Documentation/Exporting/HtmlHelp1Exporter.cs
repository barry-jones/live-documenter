using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Saxon.Api;
using System.IO;
using System.Threading;
using System.Runtime;
using System.Diagnostics;
using Microsoft.Win32;

namespace TheBoxSoftware.Documentation.Exporting {
	using TheBoxSoftware.Reflection;
	using TheBoxSoftware.Reflection.Comments;
	using TheBoxSoftware.Documentation.Exporting.Rendering;
	using TheBoxSoftware.Documentation.Exporting.HtmlHelp1;

	/// <summary>
	/// Exports the documentation to the HTML Help 1 format.
	/// </summary>
	public sealed class HtmlHelp1Exporter : Exporter {
		private System.Text.RegularExpressions.Regex illegalFileCharacters;

		/// <summary>
		/// Initialises a new instance of the HtmlHelp1Exporter.
		/// </summary>
		/// <param name="document">The document to be exported.</param>
		/// <param name="config">The export config file, from the LDEC container.</param>
		public HtmlHelp1Exporter(Document document, ExportSettings settings, ExportConfigFile config)
			: base(document, settings, config) {
			string regex = string.Format("{0}{1}",
				 new string(Path.GetInvalidFileNameChars()),
				 new string(Path.GetInvalidPathChars()));
						illegalFileCharacters = new System.Text.RegularExpressions.Regex(
							string.Format("[{0}]", System.Text.RegularExpressions.Regex.Escape(regex))
							);
		}

		#region Properties
		/// <summary>
		/// The path to the external HTML Help 1 compiler.
		/// </summary>
		private string HtmlHelpCompilerFilePath { get; set; }
		#endregion

		/// <summary>
		/// Exports the documentation as HTML Help 1 compiled help.
		/// </summary>
		public override void Export() {
			if (!this.FindHtmlHelpCompiler()) {
				this.OnExportFailed(new ExportFailedEventArgs("The HTML Help 1 compiler could not be located, please check that it is installed."));
				return;	// can not continue
			}

			// the temp output directory
			this.TempDirectory = System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetTempFileName()) + "\\";
			this.OutputDirectory = @"temp\output\";

			try {
				this.PrepareDirectory(this.TempDirectory);

				this.OnExportCalculated(new ExportCalculatedEventArgs(7));
				this.CurrentExportStep = 1;

				Documentation.Exporting.Rendering.DocumentMapXmlRenderer map = new Documentation.Exporting.Rendering.DocumentMapXmlRenderer(
					this.Document.Map
					);

				// export the document map
				this.OnExportStep(new ExportStepEventArgs("Export as XML...", ++this.CurrentExportStep));
				using (XmlWriter writer = XmlWriter.Create(string.Format("{0}/toc.xml", this.TempDirectory))) {
					map.Render(writer);
				}

				// export the project xml
				ProjectXmlRenderer projectXml = new ProjectXmlRenderer(this.Document.Map);
				using (XmlWriter writer = XmlWriter.Create(string.Format("{0}/project.xml", this.TempDirectory))) {
					projectXml.Render(writer);
				}

				//// export the index xml
				IndexXmlRenderer indexXml = new IndexXmlRenderer(this.Document.Map);
				using (XmlWriter writer = XmlWriter.Create(string.Format("{0}/index.xml", this.TempDirectory))) {
					indexXml.Render(writer);
				}

				// export each of the members
				foreach (Entry current in this.Document.Map) {
					this.RecursiveEntryExport(current);
				}

				Processor p = new Processor();
				Uri xsltLocation = new Uri(new Uri(System.Reflection.Assembly.GetExecutingAssembly().Location), "ApplicationData/livexmltohtml.xslt");
				XsltTransformer transform = p.NewXsltCompiler().Compile(this.Config.GetXslt()).Load();
				transform.SetParameter(new QName(new XmlQualifiedName("directory")), new XdmAtomicValue(System.IO.Path.GetFullPath(this.TempDirectory)));

				// Finally perform the user selected output xslt
				this.OnExportStep(new ExportStepEventArgs("Preparing output directory", ++this.CurrentExportStep));
				this.PrepareDirectory(this.OutputDirectory);

				// set output files
				this.OnExportStep(new ExportStepEventArgs("Saving output files...", ++this.CurrentExportStep));
				this.Config.SaveOutputFilesTo(this.OutputDirectory);

				this.OnExportStep(new ExportStepEventArgs("Transforming XML...", ++this.CurrentExportStep));
				
				// export the project file
				using (FileStream fs = File.OpenRead(string.Format("{0}/project.xml", this.TempDirectory))) {
					Serializer s = new Serializer();
					s.SetOutputFile(this.OutputDirectory + "project.hhp");
					transform.SetInputStream(fs, new Uri(new Uri(System.Reflection.Assembly.GetExecutingAssembly().Location), this.OutputDirectory));
					transform.Run(s);
				}

				// export the index file
				using (FileStream fs = File.OpenRead(string.Format("{0}/index.xml", this.TempDirectory))) {
					Serializer s = new Serializer();
					s.SetOutputFile(this.OutputDirectory + "index.hhk");
					transform.SetInputStream(fs, new Uri(new Uri(System.Reflection.Assembly.GetExecutingAssembly().Location), this.OutputDirectory));
					transform.Run(s);
				}

				// export the content file
				using (FileStream fs = File.OpenRead(string.Format("{0}/toc.xml", this.TempDirectory))) {
					Serializer s = new Serializer();
					s.SetOutputFile(this.OutputDirectory + "toc.hhc");
					transform.SetInputStream(fs, new Uri(new Uri(System.Reflection.Assembly.GetExecutingAssembly().Location), this.OutputDirectory));
					transform.Run(s);
				}

				// export the content files
				string[] exclude = { "toc.xml", "index.xml", "project.xml" };
				foreach (string current in Directory.GetFiles(this.TempDirectory)) {
					if (exclude.Contains(current.Substring(this.TempDirectory.Length)))
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
				this.CompileHelp(this.OutputDirectory + "project.hhp");
			}
			catch (Exception ex) {
				ExportException exception = new ExportException(ex.Message, ex);
				this.OnExportException(new ExportExceptionEventArgs(exception));
			}
			finally {
				// clean up the temp directory
				this.OnExportStep(new ExportStepEventArgs("Cleaning up", ++this.CurrentExportStep));
#if !DEBUG
				System.IO.Directory.Delete(this.TempDirectory, true);
#endif
			}
		}

		/// <summary>
		/// Checks the locations the compiler could be and indicates if it was found. The
		/// property <see cref="HtmlHelpCompilerFilePath"/> is set.
		/// </summary>
		/// <returns>Boolean indicating if the compiler was found.</returns>
		private bool FindHtmlHelpCompiler() {
			string compiler = Path.Combine(
					Environment.GetFolderPath(
						Environment.SpecialFolder.ProgramFiles),
					@"HTML Help Workshop\hhc.exe");
			if (File.Exists(compiler)) {
				this.HtmlHelpCompilerFilePath = compiler;
			}

			// Not in default directory check in registry
			RegistryKey key = Registry.ClassesRoot.OpenSubKey("hhc.file");
			if (key != null) {
				key = key.OpenSubKey("DefaultIcon");
				if (key != null) {
					object val = key.GetValue(null);
					if (val != null) {
						string hhw = (string)val;
						if (hhw.Length > 0) {
							hhw = hhw.Split(new Char[] { ',' })[0];
							hhw = Path.GetDirectoryName(hhw);
							compiler = Path.Combine(hhw, "hhc.exe");
						}
					}
				}
			}
			if (File.Exists(compiler)) {
				this.HtmlHelpCompilerFilePath = compiler;
			}

			return !string.IsNullOrEmpty(this.HtmlHelpCompilerFilePath);
		}

		/// <summary>
		/// Compiles the HTML Help 1 project file.
		/// </summary>
		/// <param name="projectFile">The project file.</param>
		private void CompileHelp(string projectFile) {
			Process compileProcess = new Process();

			ProcessStartInfo processStartInfo = new ProcessStartInfo();
			processStartInfo.FileName = this.HtmlHelpCompilerFilePath;
			processStartInfo.Arguments = "\"" + Path.GetFullPath(projectFile) + "\"";
			processStartInfo.ErrorDialog = false;
			// processStartInfo.WorkingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			processStartInfo.UseShellExecute = false;
			processStartInfo.CreateNoWindow = true;
			processStartInfo.RedirectStandardError = false; //no point redirecting as HHC does not use stdErr
			processStartInfo.RedirectStandardOutput = true;

			compileProcess.StartInfo = processStartInfo;

			// Start the help compile and bail if it takes longer than 10 minutes.
			Trace.WriteLine("Compiling Html Help file");

			string stdOut = string.Empty;

			try {
				compileProcess.Start();

				// Read the standard output of the spawned process.
				stdOut = compileProcess.StandardOutput.ReadToEnd();
				// compiler std out includes a bunch of unneccessary line feeds + new lines
				// remplace all the line feed and keep the new lines
				stdOut = stdOut.Replace("\r", "");
			}
			catch (Exception) {
				throw;
			}
		}
	}
}
