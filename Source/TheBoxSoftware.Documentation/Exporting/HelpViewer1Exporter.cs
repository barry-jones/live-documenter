
namespace TheBoxSoftware.Documentation.Exporting
{
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using System.IO;
    using System.IO.Compression;
    using System.Threading.Tasks;

    /// <summary>
    /// Exports documentation in the MS Help Viewer 1 format.
    /// </summary>
    public class HelpViewer1Exporter : Exporter
    {
        /// <summary>
        /// Initialises a new instance of the HelpViewer1Exporter class.
        /// </summary>
        /// <param name="document">The document to export.</param>
        /// <param name="config">The export configuration.</param>
        public HelpViewer1Exporter(Document document, ExportSettings settings, ExportConfigFile config)
            : base(document, settings, config, new FileSystem())
        {
        }

        /// <summary>
        /// Exports the documentation to the MS Help Viewer 1 documention type.
        /// </summary>
        public override void Export()
        {
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

                    Website.IndexXmlRenderer indexPage = new Website.IndexXmlRenderer(Document.Map);
                    using (XmlWriter writer = XmlWriter.Create(string.Format("{0}/index.xml", TempDirectory)))
                    {
                        indexPage.Render(writer);
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

                // perform the transform of the full XML files produced to the Help Viewer 1 format.
                if (!IsCancelled)
                {
                    List<Task> conversionTasks = new List<Task>();
                    string outputFile = string.Empty;

                    IXsltProcessor xsltProcessor = new MsXsltProcessor(TempDirectory);
                    using (Stream xsltStream = Config.GetXslt())
                    {
                        xsltProcessor.CompileXslt(xsltStream);
                    }

                    // set output files
                    OnExportStep(new ExportStepEventArgs("Saving output files...", ++CurrentExportStep));
                    Config.SaveOutputFilesTo(OutputDirectory);

                    OnExportStep(new ExportStepEventArgs("Transforming XML...", ++CurrentExportStep));

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
                    CompileHelp(TempDirectory + "\\Documentation.mshc");
                    File.Copy(ApplicationDirectory + "\\ApplicationData\\Documentation.msha", TempDirectory + "\\Documentation.msha");

                    // publish the documentation
                    OnExportStep(new ExportStepEventArgs("Publishing help...", ++CurrentExportStep));
                    string[] files = { "Documentation.mshc", "Documentation.msha" };
                    for (int i = 0; i < files.Length; i++)
                    {
                        File.Move(
                            Path.Combine(TempDirectory, files[i]),
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

        /// <summary>
        /// Returns a collection of messages that describe any issues that this exporter has with
        /// running.
        /// </summary>
        /// <returns>The issues.</returns>
        public override List<Issue> GetIssues()
        {
            return new List<Issue>();
        }

        /// <summary>
        /// Compiles and creates the Help Viewer 1 mshc file.
        /// </summary>
        /// <param name="projectFile">The HxC file.</param>
        private void CompileHelp(string projectFile)
        {
            ZipFile.CreateFromDirectory(OutputDirectory, projectFile);
        }
    }
}