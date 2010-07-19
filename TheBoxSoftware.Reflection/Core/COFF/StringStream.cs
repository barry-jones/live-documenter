using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Core.COFF {
	/// <summary>
	/// The string stream which stores the strings for all metadata names
	/// and text.
	/// </summary>
	public sealed class StringStream : Stream {
		/// <summary>
		/// The underlying data for the string stream.
		/// </summary>
		private byte[] streamContents;

		/// <summary>
		/// Initialises a new instance of the StringStream class.
		/// </summary>
		/// <param name="file">The file this stream is a part of.</param>
		/// <param name="address">The start address of the string stream.</param>
		/// <param name="size">The size of the stream.</param>
		/// <exception cref="InvalidOperationException">
		/// The application encountered an invalid and unexpected character at the
		/// start of the stream.
		/// </exception>
		internal StringStream(PeCoffFile file, int address, int size) {
			// The first entry in the stream should always be a null termination character
			if (file.FileContents[address] != '\0') {
				InvalidOperationException ex = new InvalidOperationException(
					Resources.ExceptionMessages.Ex_InvalidStream_StartCharacter
					);
				ex.Data["address"] = address;
				ex.Data["size"] = size;
				throw ex;
			}

			// Read and store the underlying data for this stream
			this.streamContents = new byte[size];
			for (int i = address; i < (address + size); i++) {
				this.streamContents[i - address] = file.FileContents[i];
			}
		}

		/// <summary>
		/// Retrieves the string from the stream at the specified index. This index
		/// is retrieved from the <see cref="StringIndex.Index"/> property where 
		/// implemented in the <see cref="MetadataTable"/> classes.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns>The string at the specified index.</returns>
		public string GetString(int index) {
			// Calculate the size of the string
			int size = 0;
			int length = this.streamContents.Length;
			for (int i = index; i < length && ((char)this.streamContents[i]) != '\0'; i++, size++) ;

			// Read the string in to an array
			byte[] currentString = new byte[size];
			for (int i = index, current = 0; i < (index + size); i++, current++) {
				currentString[current] = this.streamContents[i];
			}

			// Convert bytes to a string
			if (size > 0) {
				return System.Text.ASCIIEncoding.UTF8.GetString(currentString);
			}
			else {
				return string.Empty;
			}
		}

		/// <summary>
		/// Returns all the strings stored in this stream.
		/// </summary>
		/// <returns>An array of strings in this stream.</returns>
		public Dictionary<int, string> GetAllStrings() {
			Dictionary<int, string> strings = new Dictionary<int, string>();
			List<byte> currentString = null;

			// Iterate over the full string stream and read the strings and
			// starting indexes.
			bool newString = true;
			int startOffset = 0;
			for (int i = 0; i < this.streamContents.Length; i++) {
				if (currentString == null || newString) {
					newString = false;
					currentString = new List<byte>();
					startOffset = i;
				}
				byte current = this.streamContents[i];
				if ((char)current != '\0') {
					currentString.Add(current);
				}
				else {
					newString = true;
				}

				if (newString) {
					strings.Add(startOffset, System.Text.ASCIIEncoding.UTF8.GetString(currentString.ToArray()));
				}
			}

			return strings;
		}
	}
}
