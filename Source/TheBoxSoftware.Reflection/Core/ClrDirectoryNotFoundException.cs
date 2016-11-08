
namespace TheBoxSoftware.Reflection.Core
{
    using System;

    /// <summary>
    /// Thrown when the CLR directory can not be resolved due to the RVA of the 
    /// directory not being correct or not resolving to a section in the file.
    /// </summary>
    public class ClrDirectoryNotFoundException : ApplicationException
    {
        private readonly string _filename;

        public ClrDirectoryNotFoundException(string filename)
        {
            _filename = filename;
        }


        public string Filename
        {
            get { return _filename; }
        }
    }
}
