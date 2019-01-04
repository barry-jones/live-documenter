
namespace TheBoxSoftware.Documentation.Exporting
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Xml;
    using Microsoft.Win32;
    using TheBoxSoftware.Documentation.Exporting.HtmlHelp2;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Produces HTML Help 2 compiled help documentation.
    /// </summary>
    public sealed class HtmlHelp2Exporter : Exporter
    {
        private System.Text.RegularExpressions.Regex illegalFileCharacters;

        /// <summary>
        /// Initialises a new instance of the HtmlHelp1Exporter.
        /// </summary>
        /// <param name="document">The document to be exported.</param>
        /// <param name="config">The export config file, from the LDEC container.</param>
        public HtmlHelp2Exporter(Document document, ExportSettings settings, ExportConfigFile config)
            : base(document, settings, config, new FileSystem())
        {
            string regex = string.Format("{0}{1}",
                 new string(Path.GetInvalidFileNameChars()),
                 new string(Path.GetInvalidPathChars()));
            illegalFileCharacters = new System.Text.RegularExpressions.Regex(
                string.Format("[{0}]", System.Text.RegularExpressions.Regex.Escape(regex))
                );
        }

        #region Properties
        /// <summary>
        /// The path to the external HTML Help 2 compiler.
        /// </summary>
        private string HtmlHelpCompilerFilePath
        {
            get; set;
        }
        #endregion

        /// <summary>
        /// Performs the export to HTML Help 2.
        /// </summary>
        public override void Export()
        {
            if (!this.FindHtmlHelpCompiler())
            {
                this.OnExportFailed(new ExportFailedEventArgs("The HTML Help 2 compiler could not be located, please check that it is installed."));
                return; // can not continue
            }

            try
            {
                this.PrepareForExport();

                // calculate the export steps
                int numberOfSteps = 0;
                numberOfSteps += 1; // toc and index steps
                numberOfSteps += this.Document.Map.Count; // top level entries for recursive export
                numberOfSteps += 1; // output files
                numberOfSteps += ((this.Document.Map.NumberOfEntries / this.XmlExportStep) * 3); // xml export stage
                numberOfSteps += 1; // publish files
                numberOfSteps += 1; // cleanup files

                this.OnExportCalculated(new ExportCalculatedEventArgs(numberOfSteps));
                this.CurrentExportStep = 1;

                if (!this.IsCancelled)
                {
                    // export the document map
                    this.OnExportStep(new ExportStepEventArgs("Export as XML...", ++this.CurrentExportStep));
                    using (XmlWriter writer = XmlWriter.Create(string.Format("{0}/toc.xml", this.TempDirectory)))
                    {
                        Rendering.DocumentMapXmlRenderer map = new Rendering.DocumentMapXmlRenderer(
                            this.Document.Map
                            );
                        map.Render(writer);
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

                if (!this.IsCancelled)
                {
                    IXsltProcessor xsltProcessor = new MsXsltProcessor(TempDirectory);
                    using(Stream xsltStream = Config.GetXslt())
                    {
                        xsltProcessor.CompileXslt(xsltStream);
                    }

                    // set output files
                    this.OnExportStep(new ExportStepEventArgs("Saving output files...", ++this.CurrentExportStep));
                    this.Config.SaveOutputFilesTo(this.OutputDirectory);

                    this.OnExportStep(new ExportStepEventArgs("Transforming XML...", ++this.CurrentExportStep));

                    // export the project xml, we cant render the XML because the DTD protocol causes loads of probs with Saxon
                    CollectionXmlRenderer collectionXml = new CollectionXmlRenderer(this.Document.Map, string.Empty);
                    using (XmlWriter writer = XmlWriter.Create(string.Format("{0}/Documentation.HxC", this.OutputDirectory)))
                    {
                        collectionXml.Render(writer);
                    }

                    // export the incldue file xml
                    IncludeFileXmlRenderer includeXml = new IncludeFileXmlRenderer(this.Config);
                    using (XmlWriter writer = XmlWriter.Create(string.Format("{0}/Documentation.HxF", this.OutputDirectory)))
                    {
                        includeXml.Render(writer);
                    }

                    string outputFile = OutputDirectory + "Documentation.HxT";
                    string inputFile = $"{TempDirectory}/toc.xml";
                    xsltProcessor.Transform(inputFile, outputFile);

                    // export the content files
                    int counter = 0;
                    foreach (string current in Directory.GetFiles(this.TempDirectory))
                    {
                        if (current.Substring(this.TempDirectory.Length) == "toc.xml")
                            continue;

                        outputFile = OutputDirectory + Path.GetFileNameWithoutExtension(current) + ".htm";
                        xsltProcessor.Transform(current, outputFile);

                        counter++;
                        if (counter % this.XmlExportStep == 0)
                        {
                            this.OnExportStep(new ExportStepEventArgs("Transforming XML...", this.CurrentExportStep += 3));
                        }
                        if (this.IsCancelled) break;
                    }
                }

                if (!this.IsCancelled)
                {
                    // compile the html help file
                    this.OnExportStep(new ExportStepEventArgs("Compiling help...", ++this.CurrentExportStep));
                    this.CompileHelp(this.OutputDirectory + "/Documentation.HxC");

                    // publish the help
                    this.OnExportStep(new ExportStepEventArgs("Publishing help...", ++this.CurrentExportStep));
                    string[] files = {
                                         "Documentation.HxC", "Documentation.HxF", "Documentation.HxT", "Documentation.HxS",
                                         "Documentation_A.HxK", "Documentation_B.HxK", "Documentation_F.HxK", "Documentation_K.HxK",
                                         "Documentation_NamedUrl.HxK", "Documentation_S.HxK","stopwords.txt"
                                     };
                    for (int i = 0; i < files.Length; i++)
                    {
                        string from = Path.Combine(this.OutputDirectory, files[i]);

                        if (!File.Exists(from)) continue;
                        File.Move(
                            from,
                            Path.Combine(this.PublishDirectory, files[i])
                            ); ;
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

        /*
        /// <summary>
        /// Member level exporting is not supported in this exporter.
        /// </summary>
        /// <param name="entry"></param>
        public override Stream ExportMember(Entry entry)
        {
            throw new InvalidOperationException("Member level exporting is not supported in this exporter.");
        }
        */

        /// <summary>
        /// Returns a collection of messages that describe any issues that this exporter has with
        /// running.
        /// </summary>
        /// <returns>The issues.</returns>
        public override List<Issue> GetIssues()
        {
            List<Issue> issues = new List<Issue>();
            if (!this.FindHtmlHelpCompiler())
            {
                issues.Add(new Issue { Description = "Could not locate the HTML Help 2.0 compiler 'hxcomp.exe'. Please check it is installed correctly." });
            }
            return issues;
        }

        #region Helper Methods
        /// <summary>
        /// Checks the locations the compiler could be and indicates if it was found. The
        /// property <see cref="HtmlHelpCompilerFilePath"/> is set.
        /// </summary>
        /// <returns>Boolean indicating if the compiler was found.</returns>
        private bool FindHtmlHelpCompiler()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                throw new PlatformNotSupportedException("The HTML Help Compilers are only available on windows.");
            }

            // test local paths for compiler
            string compiler = Path.Combine(
                    Environment.GetFolderPath(
                        Environment.SpecialFolder.ProgramFilesX86),
                    @"Common Files\microsoft shared\Help 2.0 Compiler\hxcomp.exe");
            if (File.Exists(compiler))
            {
                this.HtmlHelpCompilerFilePath = compiler;
            }

            compiler = Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.ProgramFiles),
                    @"Common Files\microsoft shared\Help 2.0 Compiler\hxcomp.exe");
            if (File.Exists(compiler))
            {
                this.HtmlHelpCompilerFilePath = compiler;
            }

            // Not in default directory check in registry
            if (string.IsNullOrEmpty(this.HtmlHelpCompilerFilePath))
            {
                RegistryKey key = Registry.ClassesRoot.OpenSubKey("MSHelp.hxs.2.5");
                if (key != null)
                {
                    key = key.OpenSubKey("DefaultIcon");
                    if (key != null)
                    {
                        object val = key.GetValue(null);
                        if (val != null)
                        {
                            string hhw = (string)val;
                            if (hhw.Length > 0)
                            {
                                hhw = hhw.Split(new Char[] { ',' })[0];
                                hhw = Path.GetDirectoryName(hhw);
                                compiler = Path.Combine(hhw, "hxcomp.exe");
                            }
                        }
                    }
                }
                if (File.Exists(compiler))
                {
                    this.HtmlHelpCompilerFilePath = compiler;
                }
            }

            return !string.IsNullOrEmpty(this.HtmlHelpCompilerFilePath);
        }

        /// <summary>
        /// Compiles the HTML Help 2 project file.
        /// </summary>
        /// <param name="projectFile">The HxC file.</param>
        private void CompileHelp(string projectFile)
        {
            Process compileProcess = new Process();

            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = this.HtmlHelpCompilerFilePath;
            processStartInfo.Arguments = "-p \"" + Path.GetFullPath(projectFile) + "\" -r \"" + Directory.GetParent(projectFile) + "\" -l \"I:\\current\\log.log\"";
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

            try
            {
                bool ok = compileProcess.Start();
                compileProcess.WaitForExit();

                // Read the standard output of the spawned process.
                stdOut = compileProcess.StandardOutput.ReadToEnd();
                // compiler std out includes a bunch of unneccessary line feeds + new lines
                // remplace all the line feed and keep the new lines
                stdOut = stdOut.Replace("\r", "");
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}