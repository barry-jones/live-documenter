
namespace TheBoxSoftware.Documentation
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Reads a solution and returns the solution and its associated projects
    /// for referenced libraries and returns them.
    /// </summary>
    internal class SolutionFileReader : FileReader
    {
        private const string VersionPattern = @"Microsoft Visual Studio Solution File, Format Version ([\d\.]*)";
        private const string V10ProjectPattern = "Project.*\\\".*\\\".*\\\".*\\\".*\\\"(.*)\\\".*\\\".*\\\"";
        private readonly IFileSystem _filesystem;
        private string[] ValidExtensions = new string[] { ".csproj", ".vbproj", ".vcproj" };

        /// <summary>
        /// Initialises a new instance of the SolutionFileReader class.
        /// </summary>
        /// <param name="fileName">The filenname and path for the solution</param>
        /// <param name="filesystem">The filesystem to use to get the file contents</param>
        public SolutionFileReader(string fileName, IFileSystem filesystem)
            : base(fileName)
        {
            _filesystem = filesystem;
        }

        /// <summary>
        /// Reads the contents of the solution and returns all of the solutions project
        /// file output assemblies.
        /// </summary>
        /// <returns>An array of assemblies output by the solution and its projects.</returns>
        public override List<DocumentedAssembly> Read()
        {
            string solutionFile = _filesystem.ReadAllText(FileName);
            List<string> projectFiles = new List<string>();
            List<DocumentedAssembly> references = new List<DocumentedAssembly>();

            ReadVersionNumber(solutionFile);
            FindAllProjectFiles(solutionFile, projectFiles, references);

            return references;
        }

        private void ReadVersionNumber(string solutionFile)
        {
            Match versionMatch = Regex.Match(solutionFile, VersionPattern);
            Version = versionMatch.Value;
        }

        private void FindAllProjectFiles(string solutionFile, List<string> projectFiles, List<DocumentedAssembly> references)
        {
            MatchCollection projectFileMatches = Regex.Matches(solutionFile, V10ProjectPattern);
            foreach (Match current in projectFileMatches)
            {
                if (current.Groups.Count == 2)
                {
                    string projectFile = current.Groups[1].Value;
                    if (ValidExtensions.Contains(Path.GetExtension(projectFile)))
                    {
                        projectFiles.Add(projectFile);
                    }
                }
            }

            foreach (string project in projectFiles)
            {
                string fullProjectPath = Path.GetDirectoryName(FileName) + "\\" + project;
                if (File.Exists(fullProjectPath))
                {
                    ProjectFileReader reader = ProjectFileReader.Create(fullProjectPath, _filesystem);
                    reader.BuildConfiguration = BuildConfiguration;
                    references.AddRange(reader.Read());
                }
            }
        }

        /// <summary>The visual studio solution file version</summary>
        public string Version
        {
            get;
            set;
        }
    }
}