using System;
using System.Collections.Generic;
using System.Text;
using TheBoxSoftware.Documentation;
using System.IO;
using System.Xml;
using TheBoxSoftware.Documentation.Exporting;
using TheBoxSoftware.Documentation.Exporting.Rendering;
using TheBoxSoftware.Reflection;

namespace TheBoxSoftware.API.LiveDocumentor
{
    // Things people are going to need to be able to do with an API
    //  * read the contents of a single entry (class/method/parameter) which constitues a page
    //  * recieve that information in a format that can be searched/modified converted (xml)
    //  * that format should be described and static - perhaps the same as the XML conversion code I use currently
    //  * read the list of available types/members for the loaded documentation
    //      * maybe make this list searchable? 
    //  * search for specific types members and return information about all of the found entries

    // Uses
    //  * website that injects information in to the already created documentation and presents that
    //  * website that shows an always up to date version of the documentation (has it changed? yes then provide new details else show old)
    //  * 

    // Proposed API
    //public abstract Stream GetDocumentationFor(string member);
    //public abstract bool HasChanged();
    //public abstract Stream GetTableOfContents();
    //public abstract Stream Search(string member);

    // main API class for users to manage documentation
    /// <include file='Documentation\documentation.xml' path='members/member[@name="Documentation"]/*'/>
    public sealed class Documentation  {

        private string forDocument = string.Empty;
        private Document baseDocument;
        private bool isLoaded = false;
        private XmlWriterSettings outputSettings;

        private Documentation() { } // do not allow them to instatiate this without providing details

        /// <include file='Documentation\documentation.xml' path='members/member[@name="Documentation.ctor"]/*'/>
        public Documentation(string forDocument) {
            if (string.IsNullOrEmpty(forDocument))
                throw new ArgumentNullException("forDocument");
            this.forDocument = forDocument;
            this.isLoaded = false;

            // set any xml output details for producing xml in the documentation
            this.outputSettings = new XmlWriterSettings();
            this.outputSettings.Indent = true;
            this.outputSettings.IndentChars = "\t";
        }

        /// <include file='Documentation\documentation.xml' path='members/member[@name="Load"]/*'/>
        // sets default settings for the documentation and loads it to memory, this is generally
        // a slow step.
        public void Load() {
            List<DocumentedAssembly> files = new List<DocumentedAssembly>();
            Project project = null;
            ExportSettings settings = new ExportSettings();
            settings.Settings = new DocumentSettings();

            // initialise the assemblies, ldproj file will detail all assemblies, we are only working
            // with ldproj, vs projects/solutions and dll files
            if (Path.GetExtension(this.forDocument) == ".ldproj") {
                project = Project.Deserialize(this.forDocument);
                foreach (string file in project.Files) {
                    files.Add(new DocumentedAssembly(file));
                }
                settings.Settings.VisibilityFilters = project.VisibilityFilters;
            }
            else if (Path.GetExtension(this.forDocument) == ".dll") {
                files.Add(new DocumentedAssembly(this.forDocument));
            }
            else {
                try {
                    files.AddRange(
                        InputFileReader.Read(
                        this.forDocument,
                        "Release"
                        ));
                }
                catch (ArgumentException) {
                    throw new DocumentationException(
                        string.Format("The provided file [{0}] and extension is not supported", this.forDocument)
                        );
                }
            }

            settings.Settings.VisibilityFilters = new List<Visibility>() { Visibility.Public }; // we will always default to public

            // initialise the document
            EntryCreator entryCreator = new EntryCreator();
            Document d = new Document(files, Mappers.GroupedNamespaceFirst, false, entryCreator);
            d.Settings = settings.Settings;
            d.UpdateDocumentMap();

            this.baseDocument = d; // store it for future references
            this.isLoaded = true; // if we are here we have loaded successfully
        }

        /// <include file='Documentation\documentation.xml' path='members/member[@name="GetTableOfContents"]/*'/>
        // iterate over the document map and return a stream that gives people details of all the
        // members available in the documentaiton - provided in XML format
        //  NOTE: a TOC does not necessarily need to be delivered via XML. There is probably better ways...
        public XmlDocument GetTableOfContents() {
            if (!this.isLoaded)
                throw new InvalidOperationException("The documentation is not loaded, call Load first");

            XmlDocument document = new XmlDocument();
            using (MemoryStream ms = new MemoryStream()) {
                using (XmlWriter writer = XmlWriter.Create(ms, this.outputSettings)) {
                    DocumentMapXmlRenderer map = new DocumentMapXmlRenderer(this.baseDocument.Map, false);
                    map.Render(writer);
                    writer.Flush();
                    writer.Close();

                    ms.Seek(0, SeekOrigin.Begin); // jump back to the start of the stream so we can read it
                    document.Load(ms);
                }
            }

            return document;
        }

        public XmlDocument Search(string member) {
            List<Entry> results = this.baseDocument.Search(member);

            // need to render the contents of the results list to xml

            return null;
        }

        /// <include file='Documentation\documentation.xml' path='members/member[@name="GetDocumentationFor.key"]/*'/>
        // simple key lookup for documentation, this is the quickest way to retrieve documentation content
        public XmlDocument GetDocumentationFor(long key) {
            if (!this.isLoaded)
                throw new InvalidOperationException("The documentation is not loaded, call Load first");

            Entry entry = this.baseDocument.Find(key, string.Empty);
            if (entry == null)
                throw new EntryNotFoundException("The provided key {0} did not resolve to a member.");

            return this.GetDocumentationFor(entry);
        }

        /// <include file='Documentation\documentation.xml' path='members/member[@name="GetDocumentationFor.cref"]/*'/>
        public XmlDocument GetDocumentationFor(string crefPath) {
            if (!this.isLoaded)
                throw new InvalidOperationException("The documentation is not loaded, call Load first");

            Reflection.Comments.CRefPath path = Reflection.Comments.CRefPath.Parse(crefPath);
            if(path.PathType == Reflection.Comments.CRefTypes.Error)
                throw new DocumentationException("The provided cref path {0} did not parse correctly.");

            Entry entry = this.baseDocument.Find(path);
            if(entry == null)
                throw new EntryNotFoundException("The provided path {0} did not resolve to a member.");

            return this.GetDocumentationFor(entry);
        }

        // generic method for returning documentation for single methods, resolve every search type to an Entry
        // and return the parsed XML comments through this method.
        private XmlDocument GetDocumentationFor(Entry entry) {
            if (null == entry)
                throw new ArgumentNullException("The provided Entry was null.");

            XmlDocument document = new XmlDocument();
            using (MemoryStream ms = new MemoryStream()) {
                using (XmlWriter writer = XmlWriter.Create(ms, this.outputSettings)) {
                    XmlRenderer r = XmlRenderer.Create(entry, this.baseDocument);
                    if (r == null) {
                        return null;    // simply return a null reference if we cant find the renderer for the entry
                    }

                    r.Render(writer);

                    writer.Flush();
                    writer.Close();

                    ms.Seek(0, SeekOrigin.Begin); // jump back to the start of the stream so we can read it
                    document.Load(ms);
                }
            }

            return document;
        }
    }
}
