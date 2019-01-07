
namespace TheBoxSoftware.Documentation
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Class that reads solutions, projects and libraries and converts them in to
    /// DocumentedAssembly lists.
    /// </summary>
    /// <include file='code-documentation\inputfilereader.xml' path='docs/inputfilereader/member[@name="class"]/*' />
    public class InputFileReader
    {
        /// <summary>
        /// Reads and parses the file and returns all of the associated library
        /// references
        /// </summary>
        /// <include file='code-documentation\inputfilereader.xml' path='docs/inputfilereader/member[@name="Read"]/*' />
        public List<DocumentedAssembly> Read(string fileName, string buildConfiguration)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            List<DocumentedAssembly> files = null;
            FileReader reader = null;

            switch (Path.GetExtension(fileName).ToLower())
            {
                case ".sln":
                    reader = new SolutionFileReader(fileName);
                    break;

                case ".csproj":
                case ".vbproj":
                case ".vcproj":
                    reader = ProjectFileReader.Create(fileName);
                    break;

                case ".dll":
                case ".exe":
                    reader = new LibraryFileReader(fileName);
                    break;
                default:
                    throw new ArgumentException("Provided filename is for a non valid file type", fileName);
            }

            reader.BuildConfiguration = string.IsNullOrEmpty(buildConfiguration) ? "Debug" : buildConfiguration;
            files = reader.Read();
            return files;
        }
    }
}