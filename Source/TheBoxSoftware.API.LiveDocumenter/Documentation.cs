
namespace TheBoxSoftware.API.LiveDocumenter
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Xml;
    using TheBoxSoftware.Documentation;
    using TheBoxSoftware.Documentation.Exporting;
    using TheBoxSoftware.Documentation.Exporting.Rendering;
    using Reflection;

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

    /// <summary>
    /// The Documentation class provides access to methods which allow you to load and obtain information and 
    /// documentation from a documentation file.
    /// </summary>
    /// <include file='Documentation\documentation.xml' path='members/member[@name="Documentation"]/*'/>
    public sealed class Documentation
    {
        private string _forDocument = string.Empty;
        private Document _baseDocument;
        private bool _isLoaded = false;
        private XmlWriterSettings _outputSettings;

        private Documentation() { } // do not allow them to instatiate this without providing details

        /// <include file='Documentation\documentation.xml' path='members/member[@name="Documentation.ctor"]/*'/>
        public Documentation(string forDocument)
        {
            if (string.IsNullOrEmpty(forDocument))
                throw new ArgumentNullException("forDocument");
            _forDocument = forDocument;
            _isLoaded = false;

            // set any xml output details for producing xml in the documentation
            _outputSettings = new XmlWriterSettings();
            _outputSettings.Indent = true;
            _outputSettings.IndentChars = "\t";
        }

        /// <summary>
        /// Initialises the state of the documentation class and prepares it so <see cref="Documentation" /> can be accessed.
        /// </summary>
        /// <include file='Documentation\documentation.xml' path='members/member[@name="Load"]/*'/>
        // sets default settings for the documentation and loads it to memory, this is generally
        // a slow step.
        public void Load()
        {
            List<DocumentedAssembly> files = new List<DocumentedAssembly>();
            Project project = null;
            ExportSettings settings = new ExportSettings();
            settings.Settings = new DocumentSettings();

            if (!File.Exists(this._forDocument))
                throw new InvalidOperationException(string.Format("The file {0} does not exist.", this._forDocument));

            // initialise the assemblies, ldproj file will detail all assemblies, we are only working
            // with ldproj, vs projects/solutions and dll files
            if (Path.GetExtension(_forDocument) == ".ldproj")
            {
                project = Project.Deserialize(_forDocument);
                foreach (string file in project.Files)
                {
                    files.Add(new DocumentedAssembly(file));
                }
                settings.Settings.VisibilityFilters = project.VisibilityFilters;
            }
            else if (Path.GetExtension(_forDocument) == ".dll")
            {
                files.Add(new DocumentedAssembly(_forDocument));
            }
            else
            {
                try
                {
                    files.AddRange(
                        new InputFileReader().Read(
                        _forDocument,
                        "Release"
                        ));
                }
                catch (ArgumentException)
                {
                    throw new DocumentationException(
                        string.Format("The provided file [{0}] and extension is not supported", this._forDocument)
                        );
                }
            }

            settings.Settings.VisibilityFilters = new List<Visibility>() { Visibility.Public, Visibility.Protected }; // we will always default to public/protected

            // initialise the document
            EntryCreator entryCreator = new EntryCreator();
            Document d = new Document(files, Mappers.NamespaceFirst, false, entryCreator);
            d.Settings = settings.Settings;
            d.UpdateDocumentMap();

            _baseDocument = d; // store it for future references
            _isLoaded = true; // if we are here we have loaded successfully
        }

        /// <summary>
        /// Obtains the details of the Entries for the currently loaded documentation.
        /// </summary>
        /// <include file='Documentation\documentation.xml' path='members/member[@name="GetTableOfContents"]/*'/>       
        public TableOfContents GetTableOfContents()
        {
            //  NOTE: a TOC does not necessarily need to be delivered via XML. There is probably better ways...
            if (!_isLoaded)
                throw new InvalidOperationException("The documentation is not loaded, call Load first");

            return new TableOfContents(_baseDocument);
        }

        /// <summary>
        /// Retrieves the XmlDocument for the provided <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The unique Entry.Key to retrieve the documentation for.</param>
        /// <include file='Documentation\documentation.xml' path='members/member[@name="GetDocumentationFor.key"]/*'/>
        public string GetDocumentationFor(long key)
        {
            // this is the quickest way to retrieve documentation
            if (!_isLoaded)
                throw new InvalidOperationException("The documentation is not loaded, call Load first");

            Entry entry = _baseDocument.Find(key, string.Empty);
            if (entry == null)
                throw new EntryNotFoundException("The provided key {0} did not resolve to a member.");

            return this.GetDocumentationFor(entry);
        }

        /// <summary>
        /// Retrieves the XmlDocument for the provided <paramref name="crefPath"/>.
        /// </summary>
        /// <param name="crefPath">The CRefPath to retrieve the documentation for.</param>
        /// <include file='Documentation\documentation.xml' path='members/member[@name="GetDocumentationFor.cref"]/*'/>
        public string GetDocumentationFor(string crefPath)
        {
            if (string.IsNullOrEmpty(crefPath))
                throw new ArgumentNullException("crefPath");
            if (!_isLoaded)
                throw new InvalidOperationException("The documentation is not loaded, call Load first");

            Reflection.Comments.CRefPath path = Reflection.Comments.CRefPath.Parse(crefPath);
            if (path.PathType == Reflection.Comments.CRefTypes.Error)
                throw new DocumentationException("The provided cref path {0} did not parse correctly.");

            Entry entry = _baseDocument.Find(path);
            if (entry == null)
                throw new EntryNotFoundException("The provided path {0} did not resolve to a member.");

            return GetDocumentationFor(entry);
        }

        /// <summary>
        /// Retrieves the XmlDocument for the provided <paramref name="entry"/>.
        /// </summary>
        /// <param name="entry">The ContentEntry to get the documentation for.</param>
        /// <include file='Documentation\documentation.xml' path='members/member[@name="GetDocumentationFor.entry"]/*'/>
        public string GetDocumentationFor(ContentEntry entry)
        {
            if (entry == null)
                throw new ArgumentNullException("entry");
            if (!_isLoaded)
                throw new InvalidOperationException("The documentation is not loaded, call Load first");

            return GetDocumentationFor(entry.Entry);
        }

        /// <summary>
        /// Retrieves an XML representation of the documentation specified by <paramref name="entry"/>.
        /// </summary>
        /// <param name="entry">The Entry to obtain documentation for.</param>
        /// <returns>An XmlDocument containing the documentation.</returns>
        private string GetDocumentationFor(Entry entry)
        {
            if (null == entry)
                throw new ArgumentNullException("The provided Entry was null.");

            string stringOutput = string.Empty;

            using (MemoryStream output = new MemoryStream())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = Encoding.UTF8;

                using (XmlWriter writer = XmlWriter.Create(output, settings))
                {
                    XmlRenderer r = XmlRenderer.Create(entry, _baseDocument);
                    if (r == null)
                    {
                        return null;    // simply return a null reference if we cant find the renderer for the entry
                    }

                    r.Render(writer);

                    writer.Flush();
                    writer.Close();

                    // get memory stream contents as a string
                    output.Seek(0, SeekOrigin.Begin);
                    StreamReader s = new StreamReader(output);
                    stringOutput = s.ReadToEnd();
                }
            }

            return stringOutput;
        }

        /// <summary>
        /// Indicates if the documentation is loaded and is ready to be used.
        /// </summary>
        internal bool IsLoaded
        {
            get { return _isLoaded; }
        }
    }
}