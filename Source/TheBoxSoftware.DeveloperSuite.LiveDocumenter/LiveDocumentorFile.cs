
namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using TheBoxSoftware.Documentation;

    /// <summary>
    /// Represents a project file, which can be used to save and load the preferences for a LiveDocument.
    /// </summary>
    [Serializable]
    internal sealed class LiveDocumentorFile
    {
        private static LiveDocumentorFile _current;
        private Project _project;
        private LiveDocument _liveDocument;
        private bool _hasChanged;
        private string _filename;

        static LiveDocumentorFile()
        {
            _current = new LiveDocumentorFile();
        }

        private LiveDocumentorFile()
        {
            // a new file is always created with no projects and defaults to C# and the debug configuration
            _project = new Project();
            _project.Language = Reflection.Syntax.Languages.CSharp;
            _project.Configuration = Model.BuildConfigurations.Debug.ToString();

            // default to Public/Protected
            _project.VisibilityFilters.Add(Reflection.Visibility.Public);
            _project.VisibilityFilters.Add(Reflection.Visibility.Protected);
        }

        /// <summary>
        /// Sets the current LiveDocument managed by the LiveDocumentor
        /// </summary>
        /// <param name="current">The LiveDocument to manage.</param>
        public static void SetLiveDocumentorFile(LiveDocumentorFile current)
        {
            LiveDocumentorFile._current = current;
        }

        /// <summary>
        /// Update the list of DocumentedAssembly files and refreshes the document map.
        /// </summary>
        /// <returns>The updated LiveDocument.</returns>
        internal LiveDocument Update()
        {
            _liveDocument = new LiveDocument(_project.GetAssemblies(), _project.VisibilityFilters);
            _liveDocument.Settings.VisibilityFilters = _project.VisibilityFilters;
            _liveDocument.Update();
            return _liveDocument;
        }

        /// <summary>
        /// Opens the specified file in the LiveDocumentorFile.
        /// </summary>
        /// <param name="filename">The filename of the file to open.</param>
        /// <remarks>
        /// When using open, the existing assemblies are cleared. Further opening a
        /// file does not update the document. A call to <see cref="Update"/> is 
        /// required.
        /// </remarks>
        public void Open(string filename)
        {
            _project = new Project();
            _project.Files.Add(filename);

            // default to Public/Protected
            _project.VisibilityFilters.Add(Reflection.Visibility.Public);
            _project.VisibilityFilters.Add(Reflection.Visibility.Protected);

            Filename = string.Empty;
            HasChanged = true;
        }

        /// <summary>
        /// Adds a new file to the LiveDocumentorFile
        /// </summary>
        /// <param name="filename">The file to add.</param>
        public void Add(string filename)
        {
            Add(new string[] { filename });
        }

        /// <summary>
        /// Adds an array of new files to the LiveDocumentorFile.
        /// </summary>
        /// <param name="files">The files to add.</param>
        public void Add(string[] files)
        {
            _project.AddFiles(files);
            HasChanged = true;
        }

        /// <summary>
        /// Removes the assembly with the <paramref name="uniqueId"/>.
        /// </summary>
        /// <param name="uniqueId">The assembly to remove.</param>
        public void Remove(long uniqueId)
        {
            DocumentedAssembly assembly = _project.GetAssemblies().Find(a => a.UniqueId == uniqueId);

            if(assembly != null)
            {
                _project.RemovedAssemblies.Add(assembly.Name);
                HasChanged = true;
            }
        }

        /// <summary>
        /// Removes the assembly with the <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The assembly to remove.</param>
        public void Remove(string name)
        {
            DocumentedAssembly assembly = this._project.GetAssemblies().Find(a => a.Name == name);

            if(assembly != null)
            {
                _project.RemovedAssemblies.Add(assembly.Name);
                HasChanged = true;
            }
        }

        /// <summary>
        /// Saves the changes to this LDF file to its project file on disk.
        /// </summary>
        public void Save()
        {
            SavaAs(this.Filename);
        }

        /// <summary>
        /// Saves the LDF file as a project file to disk
        /// </summary>
        /// <param name="filename">The filename to save this to.</param>
        public void SavaAs(string filename)
        {
            // remove the last recent file which will be this
            if(Model.UserApplicationStore.Store.RecentFiles.Count > 0)
            {
                Model.UserApplicationStore.Store.RecentFiles.RemoveAt(0);
            }

            _project.Serialize(filename);
            HasChanged = false;
            Filename = filename;

            Model.UserApplicationStore.Store.RecentFiles.Insert(0,
                new Model.RecentFile(filename, System.IO.Path.GetFileName(filename))
                );
        }

        /// <summary>
        /// Loads an existing Live Documenter Project file and sets it as the current
        /// project.
        /// </summary>
        /// <param name="filename">The filename of the project file.</param>
        /// <returns>The instantiated LiveDocumenterFile</returns>
        public static LiveDocumentorFile Load(string filename)
        {
            if(System.IO.Path.GetExtension(filename) != ".ldproj")
                throw new ArgumentException("Was only expecting an Live Documenter Project file.");
            if(!System.IO.File.Exists(filename))
                throw new ArgumentException(string.Format("File '{0}' was expected but did not exist.", filename));

            // deserialize it
            Project project = Project.Deserialize(filename);

            // test the files
            string[] missingFiles = project.GetMissingFiles();
            if(missingFiles.Length > 0)
            {
                string message = string.Format("The following files could not be located: \n\n{0}\n\n Do you wish to continue to load the project? The missing files will be ignored.",
                    string.Join("\t\n", missingFiles)
                    );
                if(MessageBox.Show(message, "Could not locate files", MessageBoxButton.YesNo) == MessageBoxResult.No)
                {
                    LiveDocumentorFile.SetLiveDocumentorFile(new LiveDocumentorFile());
                    return LiveDocumentorFile.Singleton;
                }
                else
                {
                    for(int i = 0; i < missingFiles.Length; i++)
                    {
                        project.Files.Remove(missingFiles[i]);
                    }
                }
            }

            // convert it and set the LDF as current
            LiveDocumentorFile ldFile = new LiveDocumentorFile();
            ldFile._project = project;
            ldFile.Filename = filename;

            LiveDocumentorFile.SetLiveDocumentorFile(ldFile);
            return ldFile;
        }

        /// <summary>
        /// Clears all of the assemblies from the Live Documenter File.
        /// </summary>
        public void Clear()
        {
            _project.Files.Clear();
        }

        /// <summary>
        /// Obtains the single instance of the LIveDocumentorFile
        /// </summary>
        public static LiveDocumentorFile Singleton
        {
            get { return LiveDocumentorFile._current; }
        }

        /// <summary>
        /// The filename this LDP file was loaded from.
        /// </summary>
        public string Filename
        {
            get { return _filename; }
            set { _filename = value; }
        }

        /// <summary>
        /// Indicates if the files have changed in this project and require saving.
        /// </summary>
        public bool HasChanged
        {
            get { return _hasChanged; }
            set { _hasChanged = value; }
        }

        /// <summary>
        /// Indicates if this LiveDocumenterFile has assemblies that can be documented.
        /// </summary>
        public bool HasFiles
        {
            get
            {
                return _project.GetAssemblies().Count + _project.RemovedAssemblies.Count > 0;
            }
        }

        /// <summary>
        /// Gets a reference to the underlying project.
        /// </summary>
        internal Project UnerlyingProject
        {
            get { return _project; }
        }

        /// <summary>
        /// The actual document that represents the currently visible
        /// live document.
        /// </summary>
        public LiveDocument LiveDocument
        {
            get { return _liveDocument; }
        }

        /// <summary>
        /// The visibility filters
        /// </summary>
        public List<Reflection.Visibility> Filters
        {
            get { return _project.VisibilityFilters; }
            set
            {
                Reflection.Visibility[] current = this._project.VisibilityFilters.ToArray();
                Reflection.Visibility[] newFilters = value.ToArray();
                if(!current.SequenceEqual(newFilters))
                {
                    HasChanged = true;
                    _project.VisibilityFilters = value;
                }
            }
        }

        /// <summary>
        /// Specified the syntax language
        /// </summary>
        public Reflection.Syntax.Languages Language
        {
            get { return _project.Language; }
            set
            {
                if(_project.Language != value)
                {
                    _project.Language = value;
                    HasChanged = true;
                }
            }
        }

        /// <summary>
        /// Specified the build configuration
        /// </summary>
        public Model.BuildConfigurations Configuration
        {
            get
            {
                if(string.IsNullOrEmpty(this._project.Configuration))
                {
                    return Model.BuildConfigurations.Debug;
                }
                else
                {
                    return (Model.BuildConfigurations)Enum.Parse(typeof(Model.BuildConfigurations), this._project.Configuration);
                }
            }
            set
            {
                if(_project.Configuration != value.ToString())
                {
                    HasChanged = true;
                    _project.Configuration = value.ToString();
                }
            }
        }

        /// <summary>
        /// The users last output configuration for this project.
        /// </summary>
        public string OutputLocation
        {
            get { return _project.OutputLocation; }
            set
            {
                HasChanged = HasChanged || _project.OutputLocation != value;
                _project.OutputLocation = value;
            }
        }
    }
}