
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
    using System.Threading.Tasks;

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
            if (!FindHtmlHelpCompiler())
            {
                OnExportFailed(new ExportFailedEventArgs("The HTML Help 1 compiler could not be located, please check that it is installed."));
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

                    // export the project xml
                    ProjectXmlRenderer projectXml = new ProjectXmlRenderer(Document.Map);
                    using (XmlWriter writer = XmlWriter.Create(string.Format("{0}/project.xml", TempDirectory)))
                    {
                        projectXml.Render(writer);
                    }

                    // export the index xml
                    IndexXmlRenderer indexXml = new IndexXmlRenderer(Document.Map);
                    using (XmlWriter writer = XmlWriter.Create(string.Format("{0}/index.xml", TempDirectory)))
                    {
                        indexXml.Render(writer);
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

                    // export the project file
                    string inputFile = $"{TempDirectory}/project.xml";
                    string outputFile = OutputDirectory + "project.hhp";
                    conversionTasks.Add(xsltProcessor.TransformAsync(inputFile, outputFile));

                    // export the index file
                    inputFile = $"{TempDirectory}/index.xml";
                    outputFile = OutputDirectory + "index.hhk";
                    conversionTasks.Add(xsltProcessor.TransformAsync(inputFile, outputFile));

                    // export the content file
                    inputFile = $"{TempDirectory}/toc.xml";
                    outputFile = OutputDirectory + "toc.hhc";
                    conversionTasks.Add(xsltProcessor.TransformAsync(inputFile, outputFile));

                    // export the content files
                    int counter = 0;
                    string[] exclude = { "toc.xml", "index.xml", "project.xml" };
                    foreach (string current in Directory.GetFiles(TempDirectory))
                    {
                        if (exclude.Contains(current.Substring(TempDirectory.Length)))
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
                    CompileHelp(OutputDirectory + "project.hhp");

                    // publish the compiled help file
                    OnExportStep(new ExportStepEventArgs("Publishing help...", ++CurrentExportStep));
                    File.Copy(OutputDirectory + "project.chm", PublishDirectory + "documentation.chm");
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