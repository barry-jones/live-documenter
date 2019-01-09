
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
    using System.Threading.Tasks;

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
            if (!FindHtmlHelpCompiler())
            {
                OnExportFailed(new ExportFailedEventArgs("The HTML Help 2 compiler could not be located, please check that it is installed."));
                return; // can not continue
            }

            try
            {
                PrepareForExport();

                // calculate the export steps
                int numberOfSteps = 0;
                numberOfSteps += 1; // toc and index steps
                numberOfSteps += Document.Map.Count; // top level entries for recursive export
                numberOfSteps += 1; // output files
                numberOfSteps += ((Document.Map.NumberOfEntries / XmlExportStep) * 3); // xml export stage
                numberOfSteps += 1; // publish files
                numberOfSteps += 1; // cleanup files

                OnExportCalculated(new ExportCalculatedEventArgs(numberOfSteps));
                CurrentExportStep = 1;

                if (!IsCancelled)
                {
                    // export the document map
                    OnExportStep(new ExportStepEventArgs("Export as XML...", ++CurrentExportStep));
                    using (XmlWriter writer = XmlWriter.Create(string.Format("{0}/toc.xml", TempDirectory)))
                    {
                        Rendering.DocumentMapXmlRenderer map = new Rendering.DocumentMapXmlRenderer(
                            Document.Map
                            );
                        map.Render(writer);
                    }

                    // export each of the members
                    foreach (Entry current in Document.Map)
                    {
                        RecursiveEntryExport(current);
                        OnExportStep(new ExportStepEventArgs("Export as XML...", ++CurrentExportStep));
                        if (IsCancelled) break;
                    }
                    GC.Collect();
                }

                if (!IsCancelled)
                {
                    List<Task> conversionTasks = new List<Task>();
                    IXsltProcessor xsltProcessor = new MsXsltProcessor(TempDirectory);

                    using(Stream xsltStream = Config.GetXslt())
                    {
                        xsltProcessor.CompileXslt(xsltStream);
                    }

                    // set output files
                    OnExportStep(new ExportStepEventArgs("Saving output files...", ++CurrentExportStep));
                    Config.SaveOutputFilesTo(OutputDirectory);

                    OnExportStep(new ExportStepEventArgs("Transforming XML...", ++CurrentExportStep));

                    // export the project xml, we cant render the XML because the DTD protocol causes loads of probs with Saxon
                    CollectionXmlRenderer collectionXml = new CollectionXmlRenderer(Document.Map, string.Empty);
                    using (XmlWriter writer = XmlWriter.Create(string.Format("{0}/Documentation.HxC", OutputDirectory)))
                    {
                        collectionXml.Render(writer);
                    }

                    // export the incldue file xml
                    IncludeFileXmlRenderer includeXml = new IncludeFileXmlRenderer(Config);
                    using (XmlWriter writer = XmlWriter.Create(string.Format("{0}/Documentation.HxF", OutputDirectory)))
                    {
                        includeXml.Render(writer);
                    }

                    string outputFile = OutputDirectory + "Documentation.HxT";
                    string inputFile = $"{TempDirectory}/toc.xml";
                    conversionTasks.Add(xsltProcessor.TransformAsync(inputFile, outputFile));

                    // export the content files
                    int counter = 0;
                    foreach (string current in Directory.GetFiles(TempDirectory))
                    {
                        if (current.Substring(TempDirectory.Length) == "toc.xml")
                            continue;

                        outputFile = OutputDirectory + Path.GetFileNameWithoutExtension(current) + ".htm";
                        conversionTasks.Add(xsltProcessor.TransformAsync(current, outputFile));

                        counter++;
                        if (counter % XmlExportStep == 0)
                        {
                            OnExportStep(new ExportStepEventArgs("Transforming XML...", CurrentExportStep += 3));
                        }
                        if (IsCancelled) break;
                    }

                    Task.WaitAll(conversionTasks.ToArray());
                }

                if (!IsCancelled)
                {
                    // compile the html help file
                    OnExportStep(new ExportStepEventArgs("Compiling help...", ++CurrentExportStep));
                    CompileHelp(OutputDirectory + "/Documentation.HxC");

                    // publish the help
                    OnExportStep(new ExportStepEventArgs("Publishing help...", ++CurrentExportStep));
                    string[] files = {
                                         "Documentation.HxC", "Documentation.HxF", "Documentation.HxT", "Documentation.HxS",
                                         "Documentation_A.HxK", "Documentation_B.HxK", "Documentation_F.HxK", "Documentation_K.HxK",
                                         "Documentation_NamedUrl.HxK", "Documentation_S.HxK","stopwords.txt"
                                     };
                    for (int i = 0; i < files.Length; i++)
                    {
                        string from = Path.Combine(OutputDirectory, files[i]);

                        if (!File.Exists(from)) continue;
                        File.Move(
                            from,
                            Path.Combine(PublishDirectory, files[i])
                            ); ;
                    }
                }

                // clean up the temp directory
                OnExportStep(new ExportStepEventArgs("Cleaning up", ++CurrentExportStep));
                Cleanup();
            }
            catch (Exception ex)
            {
                Cleanup(); // attempt to clean up our mess before dying
                ExportException exception = new ExportException(ex.Message, ex);
                OnExportException(new ExportExceptionEventArgs(exception));
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
            if (!FindHtmlHelpCompiler())
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
                HtmlHelpCompilerFilePath = compiler;
            }

            compiler = Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.ProgramFiles),
                    @"Common Files\microsoft shared\Help 2.0 Compiler\hxcomp.exe");
            if (File.Exists(compiler))
            {
                HtmlHelpCompilerFilePath = compiler;
            }

            // Not in default directory check in registry
            if (string.IsNullOrEmpty(HtmlHelpCompilerFilePath))
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
                    HtmlHelpCompilerFilePath = compiler;
                }
            }

            return !string.IsNullOrEmpty(HtmlHelpCompilerFilePath);
        }

        /// <summary>
        /// Compiles the HTML Help 2 project file.
        /// </summary>
        /// <param name="projectFile">The HxC file.</param>
        private void CompileHelp(string projectFile)
        {
            Process compileProcess = new Process();

            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = HtmlHelpCompilerFilePath;
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