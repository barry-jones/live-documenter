
namespace TheBoxSoftware.Documentation.Exporting
{
    using System;
    using System.IO;
    using System.Xml;
    using Website;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class WebsiteExporter : Exporter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebsiteExporter"/> class.
        /// </summary>
        /// <param name="document">The document to be exported.</param>
        /// <param name="config">The export configuration.</param>
        public WebsiteExporter(Document document, ExportSettings settings, ExportConfigFile config)
            : base(document, settings, config, new FileSystem())
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
                PrepareForExport();

                // calculate the export steps
                int numberOfSteps = 0;
                numberOfSteps += 1; // toc and index steps
                numberOfSteps += Document.Map.Count; // top level entries for recursive export
                numberOfSteps += 1; // output files
                numberOfSteps += ((Document.Map.NumberOfEntries / XmlExportStep) * 3); // xml export stage
                numberOfSteps += 1; // cleanup files

                OnExportCalculated(new ExportCalculatedEventArgs(numberOfSteps));
                CurrentExportStep = 1;

                ExportContentAsXml();
                ConvertXmlToWebsite();

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

        private void ConvertXmlToWebsite()
        {
            List<Task> conversionTasks = new List<Task>();
            if(!IsCancelled)
            {
                IXsltProcessor xsltProcessor = new MsXsltProcessor(TempDirectory);
                using(Stream xsltStream = Config.GetXslt())
                {
                    xsltProcessor.CompileXslt(xsltStream);
                }

                // set output files
                OnExportStep(new ExportStepEventArgs("Saving output files...", ++CurrentExportStep));
                Config.SaveOutputFilesTo(PublishDirectory);

                OnExportStep(new ExportStepEventArgs("Transforming XML...", ++CurrentExportStep));
                string extension = Config.Properties.ContainsKey("extension") ? Config.Properties["extension"] : "htm";
                int counter = 0;
                foreach(string current in Directory.GetFiles(TempDirectory))
                {
                    if(current.Substring(TempDirectory.Length) == "toc.xml")
                        continue;

                    string outputFile = PublishDirectory + Path.GetFileNameWithoutExtension(current) + "." + extension;

                    conversionTasks.Add(xsltProcessor.TransformAsync(current, outputFile).ContinueWith(
                        (Task outer) =>
                        {
                            counter++;
                            if (counter % XmlExportStep == 0)
                            {
                                OnExportStep(new ExportStepEventArgs("Transforming XML...", CurrentExportStep += 3));
                            }
                        }
                        ));

                    if(IsCancelled) break;
                }
            }
            Task.WaitAll(conversionTasks.ToArray());
        }

        private void ExportContentAsXml()
        {
            List<Task> conversionTasks = new List<Task>();
            if(!IsCancelled)
            {
                OnExportStep(new ExportStepEventArgs("Export as XML...", ++CurrentExportStep));
                using(XmlWriter writer = XmlWriter.Create(string.Format("{0}/toc.xml", TempDirectory)))
                {
                    Rendering.DocumentMapXmlRenderer map = new Rendering.DocumentMapXmlRenderer(Document.Map);
                    map.Render(writer);
                }

                // export the index page
                IndexXmlRenderer indexPage = new IndexXmlRenderer(Document.Map);
                using(XmlWriter writer = XmlWriter.Create(string.Format("{0}/index.xml", TempDirectory)))
                {
                    indexPage.Render(writer);
                }

                // export each of the members
                foreach(Entry current in Document.Map)
                {
                    RecursiveEntryExport(current);
                    OnExportStep(new ExportStepEventArgs("Export as XML...", ++CurrentExportStep));
                    if(IsCancelled) break;
                }
            }
            Task.WaitAll(conversionTasks.ToArray());
            GC.Collect();
        }

        /// <summary>
        /// Returns a collection of messages that describe any issues that this exporter has with
        /// running.
        /// </summary>
        /// <returns>The issues.</returns>
        public override List<Issue> GetIssues() => new List<Issue>();
    }
}