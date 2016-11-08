
namespace TheBoxSoftware.Documentation.Exporting
{
    using System;
    using System.IO;
    using System.Xml;
    using TheBoxSoftware.Documentation.Exporting.Website;
    using System.Collections.Generic;

    /// <summary>
    /// Exporter that exports the docuementation to XML.
    /// </summary>
    public class XmlExporter : Exporter
    {
        private XmlWriterSettings outputSettings;

        /// <summary>
        /// Initializes a new instance of the XmlExporter class.
        /// </summary>
        /// <param name="document">The document to be exported.</param>
        /// <param name="settings">Any settings for the export.</param>
        /// <param name="config">The export configuration.</param>
        public XmlExporter(Document document, ExportSettings settings, ExportConfigFile config)
            : base(document, settings, config, new FileSystem())
        {
            this.outputSettings = new XmlWriterSettings();
            this.outputSettings.Indent = true;
            this.outputSettings.IndentChars = "\t";
        }

        public XmlExporter(Document document, ExportSettings settings, ExportConfigFile config, IFileSystem fileSystem)
            : base(document, settings, config, fileSystem)
        {
            this.outputSettings = new XmlWriterSettings();
            this.outputSettings.Indent = true;
            this.outputSettings.IndentChars = "\t";
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
                numberOfSteps += this.Document.Map.NumberOfEntries; // xml export stage
                numberOfSteps += 1; // cleanup files

                this.OnExportCalculated(new ExportCalculatedEventArgs(numberOfSteps));
                this.CurrentExportStep = 1;

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = "\t";

                // export the document map
                if (!this.IsCancelled)
                {
                    this.OnExportStep(new ExportStepEventArgs("Export as XML...", ++this.CurrentExportStep));
                    using (XmlWriter writer = XmlWriter.Create(string.Format("{0}/toc.xml", this.TempDirectory), this.outputSettings))
                    {
                        Rendering.DocumentMapXmlRenderer map = new Rendering.DocumentMapXmlRenderer(this.Document.Map);
                        map.Render(writer);
                    }

                    // export the index page
                    IndexXmlRenderer indexPage = new IndexXmlRenderer(this.Document.Map);
                    using (XmlWriter writer = XmlWriter.Create(string.Format("{0}/index.xml", this.TempDirectory), this.outputSettings))
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

                if (!this.IsCancelled)
                {
                    // set output files
                    this.OnExportStep(new ExportStepEventArgs("Saving output files...", ++this.CurrentExportStep));
                    this.Config.SaveOutputFilesTo(this.PublishDirectory);

                    // move all the temporary export files to the publish directory
                    foreach (string file in Directory.GetFiles(this.TempDirectory))
                    {
                        File.Copy(file, Path.Combine(this.PublishDirectory, Path.GetFileName(file)));
                        this.OnExportStep(new ExportStepEventArgs("Publishing XML files...", ++this.CurrentExportStep));
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
        /// Returns a collection of messages that describe any issues that this exporter has with
        /// running.
        /// </summary>
        /// <returns>The issues.</returns>
        public override List<Issue> GetIssues()
        {
            return new List<Issue>();
        }

        /// <summary>
        /// A method that recursively, through the documentation tree, exports all of the
        /// found pages for each of the entries in the documentation.
        /// </summary>
        /// <param name="currentEntry">The current entry to export</param>
        protected override void RecursiveEntryExport(Entry currentEntry)
        {
            this.Export(currentEntry);
            for (int i = 0; i < currentEntry.Children.Count; i++)
            {
                this.RecursiveEntryExport(currentEntry.Children[i]);
            }
        }

        /// <summary>
        /// Exports the current entry.
        /// </summary>
        /// <param name="current">The current entry to export.</param>
        /// <returns>The name of the rendered XML file</returns>
        protected override string Export(Entry current)
        {
            string filename = string.Format("{0}{1}{2}.xml",
                this.TempDirectory,
                current.Key,
                string.IsNullOrEmpty(current.SubKey) ? string.Empty : "-" + this.IllegalFileCharacters.Replace(current.SubKey, string.Empty)
                );

            try
            {
                Rendering.XmlRenderer r = Rendering.XmlRenderer.Create(current, this.Document);
                if (r != null)
                {
                    using (System.Xml.XmlWriter writer = XmlWriter.Create(filename, this.outputSettings))
                    {
                        r.Render(writer);
                    }
                }
            }
            catch (Exception ex)
            {
                if (System.IO.File.Exists(filename))
                {
                    System.IO.File.Delete(filename);
                }
                // we will deal with it later
                this.ExportExceptions.Add(ex);
            }

            return filename;
        }
    }
}