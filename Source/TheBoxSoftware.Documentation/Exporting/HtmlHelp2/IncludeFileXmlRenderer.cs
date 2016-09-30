using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Documentation.Exporting.HtmlHelp2
{
    /// <summary>
    /// Renders the XML for the HTML Help 2 IncludeFile specified in the Collection Defenition (Hxc)
    /// file.
    /// </summary>
    /// <remarks>
    /// <para>See http://msdn.microsoft.com/en-us/library/bb165026(v=VS.80).aspx for full details of the file
    /// format and structure.</para>
    /// <para>
    /// As detailed this file contains references to all the important help colleciton files and the main content
    /// for the html help documentation.
    /// </para>
    /// </remarks>
    internal class IncludeFileXmlRenderer : Rendering.XmlRenderer
    {
        private ExportConfigFile configFile;
        private string baseDirectory;

        /// <summary>
        /// Initialises a new instance of the IncludeFileXmlRenderer.
        /// </summary>
        /// <param name="configFile">A reference to the export config file for this export.</param>
        /// <param name="baseDirectory">The base output directory where all the files exported to.</param>
        public IncludeFileXmlRenderer(ExportConfigFile configFile)
        {
            this.configFile = configFile;
            this.baseDirectory = baseDirectory;
        }

        /// <summary>
        /// Renders the XML for the Include File (HxF)
        /// </summary>
        /// <param name="writer"></param>
        public override void Render(System.Xml.XmlWriter writer)
        {
            writer.WriteStartDocument();
            writer.WriteRaw("<!DOCTYPE HelpFileList SYSTEM \"ms-help://hx/resources/HelpFileList.DTD\">");

            writer.WriteStartElement("HelpFileList");
            writer.WriteAttributeString("DTDVersion", "1.0");

            foreach (string file in this.BuildFileList())
            {
                writer.WriteStartElement("File");
                writer.WriteAttributeString("Url", file);
                writer.WriteEndElement();
            }

            writer.WriteEndElement(); // HelpFileList
        }

        /// <summary>
        /// Returns a list of URLs for all of the files to be included in the compiled help.
        /// </summary>
        /// <returns></returns>
        private List<string> BuildFileList()
        {
            List<string> includes = new List<string>();
            string[] excludes = { "stopwords.txt",
                                    "Documentation_A.HxK",
                                    "Documentation_B.HxK",
                                    "Documentation_F.HxK",
                                    "Documentation_K.HxK",
                                    "Documentation_NamedUrl.HxK",
                                    "Documentation_S.HxK"
                                };

            // include the important files
            // includes.Add(System.IO.Path.Combine(baseDirectory, "index.HxK"));
            // includes.Add(System.IO.Path.Combine(baseDirectory, "toc.HxT"));

            // include all html output
            includes.Add("*.htm");

            // include the files registered in the export config file
            List<string> configUrls = this.configFile.GetOutputFileURLs();
            foreach (string url in configUrls)
            {
                if (excludes.Contains(url))
                    continue;
                includes.Add(url.Replace('/', '\\'));
            }

            return includes;
        }
    }
}