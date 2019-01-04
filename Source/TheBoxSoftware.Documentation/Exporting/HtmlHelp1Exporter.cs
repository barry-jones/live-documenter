
namespace TheBoxSoftware.Documentation.Exporting
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using Microsoft.Win32;
    using HtmlHelp1;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Exports the documentation to the HTML Help 1 format.
    /// </summary>
    public sealed class HtmlHelp1Exporter : Exporter
    {
        private System.Text.RegularExpressions.Regex illegalFileCharacters;
        private string _htmlHelpCompilerFilePath;

        /// <summary>
        /// Initialises a new instance of the HtmlHelp1Exporter.
        /// </summary>
        /// <param name="document">The document to be exported.</param>
        /// <param name="config">The export config file, from the LDEC container.</param>
        public HtmlHelp1Exporter(Document document, ExportSettings settings, ExportConfigFile config)
            : base(document, settings, config, new FileSystem())
        {
            string regex = string.Format("{0}{1}",
                 new string(Path.GetInvalidFileNameChars()),
                 new string(Path.GetInvalidPathChars()));
            illegalFileCharacters = new System.Text.RegularExpressions.Regex(
                string.Format("[{0}]", System.Text.RegularExpressions.Regex.Escape(regex))
                );
        }

        /// <summary>
        /// Exports the documentation as HTML Help 1 compiled help.
        /// </summary>
        public override void Export()
        {
            if (!this.FindHtmlHelpCompiler())
            {
                this.OnExportFailed(new ExportFailedEventArgs("The HTML Help 1 compiler could not be located, please check that it is installed."));
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

                    // export the project xml
                    ProjectXmlRenderer projectXml = new ProjectXmlRenderer(this.Document.Map);
                    using (XmlWriter writer = XmlWriter.Create(string.Format("{0}/project.xml", this.TempDirectory)))
                    {
                        projectXml.Render(writer);
                    }

                    // export the index xml
                    IndexXmlRenderer indexXml = new IndexXmlRenderer(this.Document.Map);
                    using (XmlWriter writer = XmlWriter.Create(string.Format("{0}/index.xml", this.TempDirectory)))
                    {
                        indexXml.Render(writer);
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

                    using(Stream xsltStream = this.Config.GetXslt())
                    {
                        xsltProcessor.CompileXslt(xsltStream);
                    }

                    // set output files
                    this.OnExportStep(new ExportStepEventArgs("Saving output files...", ++this.CurrentExportStep));
                    this.Config.SaveOutputFilesTo(this.OutputDirectory);

                    this.OnExportStep(new ExportStepEventArgs("Transforming XML...", ++this.CurrentExportStep));

                    // export the project file
                    string inputFile = $"{TempDirectory}/project.xml";
                    string outputFile = OutputDirectory + "project.hhp";
                    xsltProcessor.Transform(inputFile, outputFile);

                    // export the index file
                    inputFile = $"{TempDirectory}/index.xml";
                    outputFile = OutputDirectory + "index.hhk";
                    xsltProcessor.Transform(inputFile, outputFile);

                    // export the content file
                    inputFile = $"{TempDirectory}/toc.xml";
                    outputFile = OutputDirectory + "toc.hhc";
                    xsltProcessor.Transform(inputFile, outputFile);

                    // export the content files
                    int counter = 0;
                    string[] exclude = { "toc.xml", "index.xml", "project.xml" };
                    foreach (string current in Directory.GetFiles(this.TempDirectory))
                    {
                        if (exclude.Contains(current.Substring(this.TempDirectory.Length)))
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
                    this.CompileHelp(this.OutputDirectory + "project.hhp");

                    // publish the compiled help file
                    this.OnExportStep(new ExportStepEventArgs("Publishing help...", ++this.CurrentExportStep));
                    File.Copy(this.OutputDirectory + "project.chm", this.PublishDirectory + "documentation.chm");
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
        /// Returns a collection of messages that describe any issues that this exporter has with
        /// running.
        /// </summary>
        /// <returns>The issues.</returns>
        public override List<Issue> GetIssues()
        {
            List<Issue> issues = new List<Issue>();
            if (!this.FindHtmlHelpCompiler())
            {
                issues.Add(new Issue { Description = "Could not locate the HTML Help 1 compiler 'hhc.exe'. Please check it is installed correctly." });
            }
            return issues;
        }

        /// <summary>
        /// Checks the locations the compiler could be and indicates if it was found.
        /// </summary>
        /// <returns>Boolean indicating if the compiler was found.</returns>
        private bool FindHtmlHelpCompiler()
        {
            if(!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                throw new PlatformNotSupportedException("The HTML Help Compilers are only available on windows.");
            }

            string compiler = Path.Combine(
                    Environment.GetFolderPath(
                        Environment.SpecialFolder.ProgramFilesX86),
                    @"HTML Help Workshop\hhc.exe");
            if (File.Exists(compiler))
            {
                _htmlHelpCompilerFilePath = compiler;
            }

            // Not in default directory check in registry
            RegistryKey key = Registry.ClassesRoot.OpenSubKey("hhc.file");
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
                            compiler = Path.Combine(hhw, "hhc.exe");
                        }
                    }
                }
            }
            if (File.Exists(compiler))
            {
                _htmlHelpCompilerFilePath = compiler;
            }

            return !string.IsNullOrEmpty(_htmlHelpCompilerFilePath);
        }

        /// <summary>
        /// Compiles the HTML Help 1 project file.
        /// </summary>
        /// <param name="projectFile">The project file.</param>
        private void CompileHelp(string projectFile)
        {
            Process compileProcess = new Process();

            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = _htmlHelpCompilerFilePath;
            processStartInfo.Arguments = "\"" + Path.GetFullPath(projectFile) + "\"";
            processStartInfo.ErrorDialog = false;
            processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            processStartInfo.UseShellExecute = false;
            processStartInfo.CreateNoWindow = true;
            processStartInfo.RedirectStandardError = false; //no point redirecting as HHC does not use stdErr
            processStartInfo.RedirectStandardOutput = true;

            compileProcess.StartInfo = processStartInfo;

            string stdOut = string.Empty;
            try
            {
                compileProcess.Start();

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
    }
}