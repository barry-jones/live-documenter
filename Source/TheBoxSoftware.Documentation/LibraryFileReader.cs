
namespace TheBoxSoftware.Documentation
{
    using System.Collections.Generic;

    /// <summary>
    /// Reads a single file, dll or exe and returns the filename
    /// </summary>
    internal class LibraryFileReader : FileReader
    {
        /// <summary>
        /// Initialises a new instance of the LibraryFileReader class.
        /// </summary>
        /// <param name="fileName">The full path and name of the assembly.</param>
        public LibraryFileReader(string fileName) : base(fileName) { }

        /// <summary>
        /// Reads the details of the assembly and its associated comment file
        /// form the FileName.
        /// </summary>
        /// <returns>The list of assemblies that are to be read by the application.</returns>
        public override List<DocumentedAssembly> Read()
        {
            return new List<DocumentedAssembly>() { new DocumentedAssembly(this.FileName) };
        }
    }
}