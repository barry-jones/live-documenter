using System;
using System.Collections.Generic;
using System.Text;

namespace TheBoxSoftware.Reflection.Core.COFF
{
    /// <summary>
    /// The string stream which stores the strings for all metadata names and text.
    /// </summary>
    public sealed class StringStream : Stream, IStringStream
    {
        private const char TerminatingChar = '\0';

        private byte[] _streamContents; // underlying stream data from file

        /// <summary>
        /// Initialises a new instance of the StringStream class.
        /// </summary>
        /// <param name="fileContents">The file this stream is a part of.</param>
        /// <param name="address">The start address of the string stream.</param>
        /// <param name="size">The size of the stream.</param>
        /// <exception cref="InvalidOperationException">
        /// The application encountered an invalid and unexpected character at the
        /// start of the stream.
        /// </exception>
        internal StringStream(byte[] fileContents, int address, int size)
        {
            // The first entry in the stream should always be a null termination character
            if(fileContents[address] != TerminatingChar)
            {
                InvalidOperationException ex = new InvalidOperationException(
                    Resources.ExceptionMessages.Ex_InvalidStream_StartCharacter
                    );
                ex.Data["address"] = address;
                ex.Data["size"] = size;
                throw ex;
            }

            // Read and store the underlying data for this stream
            _streamContents = new byte[size];
            for(int i = address; i < (address + size); i++)
            {
                _streamContents[i - address] = fileContents[i];
            }
        }

        /// <summary>
        /// Retrieves the string from the stream at the specified index. This index
        /// is retrieved from the <see cref="StringIndex.Index"/> property where 
        /// implemented in the <see cref="MetadataTable"/> classes.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The string at the specified index.</returns>
        public string GetString(int index)
        {
            int lengthOfString = 0;
            int length = _streamContents.Length;

            // calculate the length of the string
            for(int i = index; i < length && _streamContents[i] != TerminatingChar; i++, lengthOfString++) ;

            // Read the string in to an array
            byte[] currentString = new byte[lengthOfString];
            for(int i = index, current = 0; i < (index + lengthOfString); i++, current++)
            {
                currentString[current] = _streamContents[i];
            }

            if(lengthOfString > 0)
            {
                return ASCIIEncoding.UTF8.GetString(currentString);
            }

            return string.Empty;
        }

        /// <summary>
        /// Returns all the strings stored in this stream.
        /// </summary>
        /// <returns>An array of strings in this stream.</returns>
        public Dictionary<int, string> GetAllStrings()
        {
            Dictionary<int, string> strings = new Dictionary<int, string>();
            List<byte> currentString = null;
            int streamLength = _streamContents.Length;

            // Iterate over the full string stream and read the strings and
            // starting indexes.
            bool newString = true;
            int startOffset = 0;
            for(int i = 0; i < streamLength; i++)
            {
                if(currentString == null || newString)
                {
                    newString = false;
                    currentString = new List<byte>();
                    startOffset = i;
                }

                byte current = _streamContents[i];
                if((char)current != TerminatingChar)
                {
                    currentString.Add(current);
                }
                else
                {
                    newString = true;
                }

                if(newString)
                {
                    strings.Add(startOffset, System.Text.ASCIIEncoding.UTF8.GetString(currentString.ToArray()));
                }
            }

            return strings;
        }
    }
}