using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Documentation.Exporting.HtmlHelp2
{
    /// <summary>
    /// Renders the contents of the HxC file for HTML Help 2.
    /// </summary>
    internal class CollectionXmlRenderer : Rendering.XmlRenderer
    {
        private DocumentMap documentMap;
        private string outputFileName;
        private const string documentation = "Documentation";

        /// <summary>
        /// Initialises a new instance of the CollectionXmlRenderer.
        /// </summary>
        /// <param name="documentMap">The document map to render.</param>
        /// <param name="outputFileName">The filename to output the HxC file as.</param>
        public CollectionXmlRenderer(DocumentMap documentMap, string outputFileName)
        {
            this.documentMap = documentMap;
            this.outputFileName = outputFileName;
        }

        /// <summary>
        /// Renders the contents of the HxC file that describes the HTML Help 2 documentation.
        /// </summary>
        /// <param name="writer">The XML writer.</param>
        public override void Render(System.Xml.XmlWriter writer)
        {
            writer.WriteStartDocument();
            writer.WriteRaw("<!DOCTYPE HelpCollection SYSTEM \"MS-Help://Hx/Resources/HelpCollection.DTD\">");

            writer.WriteStartElement("HelpCollection");
            writer.WriteAttributeString("DTDVersion", "1.0");
            writer.WriteAttributeString("FileVersion", "1.0");
            writer.WriteAttributeString("Title", "Specified by the user");

            writer.WriteStartElement("CompilerOptions");
            writer.WriteAttributeString("CompileResult", "Hxs");
            if (!string.IsNullOrEmpty(this.outputFileName))
            {
                writer.WriteAttributeString("OutputFile", this.outputFileName);
            }
            writer.WriteAttributeString("StopWordFile", "stopwords.txt");

            // pointer to the include file .HxF (requried)
            writer.WriteStartElement("IncludeFile");
            writer.WriteAttributeString("File", string.Format("{0}.HxF", CollectionXmlRenderer.documentation));
            writer.WriteEndElement(); // IncludeFile

            writer.WriteEndElement(); // CompilerOptions

            //// pointer to the attribute definition file .HxA (requried)
            //writer.WriteStartElement("AttributeDef");	// zero or more times
            //writer.WriteAttributeString("File", string.Empty);	// required
            //writer.WriteAttributeString("Owner", string.Empty);
            //writer.WriteEndElement();

            //// pointer to the virtual topic defintion file .HxV (requried)
            //writer.WriteStartElement("VTopicDef");	// zero or more times
            //writer.WriteAttributeString("File", string.Empty);
            //writer.WriteEndElement();

            // pointer to the TOC file .HxT (requried)
            writer.WriteStartElement("TOCDef");     // zero or more times
            writer.WriteAttributeString("File", string.Format("{0}.HxT", CollectionXmlRenderer.documentation));
            writer.WriteEndElement();

            string[] indexes = {
                                   "_A.HxK",
                                   "_B.HxK",
                                   "_F.HxK",
                                   "_K.HxK",
                                   "_NamedUrl.HxK",
                                   "_S.HxK"
                               };
            foreach (string index in indexes)
            {
                // pointer to the keyword-index file .HxK (requried)
                writer.WriteStartElement("KeywordIndexDef"); // zero or more times
                writer.WriteAttributeString("File", string.Format("{0}{1}", CollectionXmlRenderer.documentation, index));
                writer.WriteEndElement();
            }

            //// pointer to the sample definition .HxE (requried)
            //writer.WriteStartElement("SampleDef"); // zero or more times
            //writer.WriteAttributeString("File", "sampledef.HxE");
            //writer.WriteEndElement();

            // apparently these are required, but I dont really know why? http://helpware.net/mshelp2/h2tutorial.htm
            writer.WriteRaw("<ItemMoniker Name=\"!DefaultTOC\" ProgId=\"HxDs.HxHierarchy\" InitData=\"" + CollectionXmlRenderer.documentation + "\" />");
            writer.WriteRaw("<ItemMoniker Name=\"!DefaultFullTextSearch\" ProgId=\"HxDs.HxFullTextSearch\" InitData=\"FTS\" />");
            writer.WriteRaw("<ItemMoniker Name=\"!DefaultAssociativeIndex\" ProgId=\"HxDs.HxIndex\" InitData=\"A\" />");
            writer.WriteRaw("<ItemMoniker Name=\"!DefaultDynamicLinkIndex\" ProgId=\"HxDs.HxIndex\" InitData=\"B\" />");
            writer.WriteRaw("<ItemMoniker Name=\"!DefaultContextWindowIndex\" ProgId=\"HxDs.HxIndex\" InitData=\"F\" />");
            writer.WriteRaw("<ItemMoniker Name=\"!DefaultKeywordIndex\" ProgId=\"HxDs.HxIndex\" InitData=\"K\" />");
            writer.WriteRaw("<ItemMoniker Name=\"!DefaultNamedUrlIndex\" ProgId=\"HxDs.HxIndex\" InitData=\"NamedUrl\" />");
            writer.WriteRaw("<ItemMoniker Name=\"!DefaultSearchWindowIndex\" ProgId=\"HxDs.HxIndex\" InitData=\"S\" />");

            //writer.WriteStartElement("ToolData"); // zero or more times
            //writer.WriteAttributeString("Name", string.Empty); // requried
            //writer.WriteAttributeString("Value", string.Empty); // required
            //writer.WriteEndElement();

            writer.WriteEndElement(); // HelpCollection
        }
    }
}