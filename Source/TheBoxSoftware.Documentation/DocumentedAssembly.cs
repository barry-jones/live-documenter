
namespace TheBoxSoftware.Documentation
{
    using System;
    using TheBoxSoftware.Reflection;

    /// <summary>
    /// An assembly that is being documented by the application. Acts as a wrapper to the file
    /// and associated xml file.
    /// </summary>
    /// <seealso cref="AssemblyDef" />
    /// <seealso cref="Comments.XmlCodeCommentFile" />
    public sealed class DocumentedAssembly
    {
        private AssemblyDef _assembly;
        private string _filename;
        private DateTime _timeLoaded;
        private string _name;
        private string _xmlFilename;
        private long _uniqueId;

        public DocumentedAssembly()
        {
        }

        /// <summary>
        /// Initialises a new instance of the DocumentedAssembly class where the FileName
        /// of the assembly is known.
        /// </summary>
        /// <param name="fileName">The full filepath and name of the assembly.</param>
        /// <exception cref="ArgumentNullException">The FileName is null or empty.</exception>
        /// <remarks>
        /// When only the fileName is provided the xml comment file is assumed to be the same
        /// file (i.e. path and name) as the assembly with the .xml extension instead of dll
        /// for example.
        /// </remarks>
        public DocumentedAssembly(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException("fileName");
            FileName = fileName;
            XmlFileName = System.IO.Path.ChangeExtension(fileName, "xml");
            Name = System.IO.Path.GetFileNameWithoutExtension(fileName);
        }

        /// <summary>
        /// Initialises a new instance of the DocumentedAssembly class where the FileName
        /// of the assembly is known.
        /// </summary>
        /// <param name="fileName">The full filepath and name of the assembly.</param>
        /// <param name="documentationFile">The full filepath and name of the associated xml documentation file.</param>
        /// <exception cref="ArgumentNullException">The FileName is null or empty.</exception>
        /// <remarks>
        /// When only the fileName is provided the xml comment file is assumed to be the same
        /// file (i.e. path and name) as the assembly with the .xml extension instead of dll
        /// for example.
        /// </remarks>
        public DocumentedAssembly(string fileName, string documentationFile)
        {
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentNullException("fileName");

            // no doc file, assume output in default location
            if (string.IsNullOrEmpty(documentationFile))
            {
                documentationFile = System.IO.Path.ChangeExtension(fileName, "xml");
            }

            FileName = fileName;
            XmlFileName = documentationFile;
            Name = System.IO.Path.GetFileNameWithoutExtension(fileName);
        }

        /// <summary>
        /// Checks if this documented assembly has been modified since we last loaded
        /// it.
        /// </summary>
        /// <returns>True if it has changed else false.</returns>
        public bool HasAssemblyBeenModified()
        {
            DateTime lastWriteTime = System.IO.File.GetLastWriteTime(FileName);
            return IsCompiled && TimeLoaded < lastWriteTime;
        }

        /// <summary>
        /// Gets the full name and path of the assembly being documented.
        /// </summary>
        public string FileName
        {
            get { return _filename; }
            set { _filename = value; }
        }

        /// <summary>
        /// Gets or sets the date and time the file was last loaded.
        /// </summary>
        public DateTime TimeLoaded
        {
            get { return _timeLoaded; }
            private set { _timeLoaded = value; }
        }

        /// <summary>
        /// The name of the assembly as it would appear in assembly reference metadata entries.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets the full path and filename for the associated XML code comments file.
        /// </summary>
        public string XmlFileName
        {
            get { return _xmlFilename; }
            set { _xmlFilename = value; }
        }

        /// <summary>
        /// Indicates that the file exists on disk and is compiled.
        /// </summary>
        public bool IsCompiled
        {
            get { return System.IO.File.Exists(FileName); }
        }

        /// <summary>
        /// The unique id for this Assembly
        /// </summary>
        public long UniqueId
        {
            get { return _uniqueId; }
            set { _uniqueId = value; }
        }

        /// <summary>
        /// A reference to the assembly after it has been loaded.
        /// </summary>
        public AssemblyDef LoadedAssembly
        {
            get { return _assembly; }
            set
            {
                _assembly = value;
                TimeLoaded = DateTime.Now;
            }
        }
    }
}