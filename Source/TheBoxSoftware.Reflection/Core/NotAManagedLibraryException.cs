
namespace TheBoxSoftware.Reflection.Core
{
    using System;

    /// <summary>
    /// Represents an error in the application where someone has attempted
    /// to load a native or non-managed library.
    /// </summary>
    public sealed class NotAManagedLibraryException : ApplicationException
    {
        /// <summary>
        /// Initialises a new instance of the NotAManagedLibraryException.
        /// </summary>
        /// <param name="message">The message describing the error.</param>
        public NotAManagedLibraryException(string message)
            : base(message)
        {
        }
    }
}