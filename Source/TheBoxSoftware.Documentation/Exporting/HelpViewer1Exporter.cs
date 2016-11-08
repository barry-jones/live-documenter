using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using TheBoxSoftware.Documentation;
using Saxon.Api;

namespace TheBoxSoftware.Documentation.Exporting
{
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

                    Website.IndexXmlRenderer indexPage = new Website.IndexXmlRenderer(this.Document.Map);
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

                // perform the transform of the full XML files produced to the Help Viewer 1 format.
                if (!this.IsCancelled)
                {
                    Processor p = new Processor();
                    using (Stream xsltStream = this.Config.GetXslt())
                    {
                        XsltTransformer transform = p.NewXsltCompiler().Compile(xsltStream).Load();
                        transform.SetParameter(new QName(new XmlQualifiedName("directory")), new XdmAtomicValue(System.IO.Path.GetFullPath(this.TempDirectory)));

                        // set output files
                        this.OnExportStep(new ExportStepEventArgs("Saving output files...", ++this.CurrentExportStep));
                        this.Config.SaveOutputFilesTo(this.OutputDirectory);

                        this.OnExportStep(new ExportStepEventArgs("Transforming XML...", ++this.CurrentExportStep));

                        // export the content files
                        int counter = 0;
                        foreach (string current in Directory.GetFiles(this.TempDirectory))
                        {
                            if (current.Substring(this.TempDirectory.Length) == "toc.xml")
                                continue;
                            using (FileStream fs = File.OpenRead(current))
                            {
                                Serializer s = new Serializer();
                                s.SetOutputFile(this.OutputDirectory + Path.GetFileNameWithoutExtension(current) + ".htm");
                                transform.SetInputStream(fs, new Uri(new Uri(System.Reflection.Assembly.GetExecutingAssembly().Location), this.OutputDirectory));
                                transform.Run(s);
                                s.Close();
                            }
                            counter++;
                            if (counter % this.XmlExportStep == 0)
                            {
                                this.OnExportStep(new ExportStepEventArgs("Transforming XML...", this.CurrentExportStep += 3));
                            }
                            if (this.IsCancelled) break;
                        }
                    }
                }

                if (!this.IsCancelled)
                {
                    // compile the html help file
                    this.OnExportStep(new ExportStepEventArgs("Compiling help...", ++this.CurrentExportStep));
                    this.CompileHelp(this.OutputDirectory + "\\Documentation.mshc");
                    File.Copy(this.ApplicationDirectory + "\\ApplicationData\\Documentation.msha", this.OutputDirectory + "\\Documentation.msha");

                    // publish the documentation
                    this.OnExportStep(new ExportStepEventArgs("Publishing help...", ++this.CurrentExportStep));
                    string[] files = { "Documentation.mshc", "Documentation.msha" };
                    for (int i = 0; i < files.Length; i++)
                    {
                        File.Move(
                            Path.Combine(this.OutputDirectory, files[i]),
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
            // zip up the content director and rename it
            Ionic.Zip.ZipFile outputFile = new Ionic.Zip.ZipFile();
            outputFile.AddDirectory(this.OutputDirectory);
            outputFile.Save(projectFile);
        }
    }
}