using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Core.COFF {
	/// <summary>
	/// Represents the metadata GUID stream available in .net pe/coff files. And provides
	/// access to those details.
	/// </summary>
	public sealed class GuidStream : Stream {
		private const int sizeOfGuid = 16;
		private byte[] streamContents;

		/// <summary>
		/// Initialises a new instance of the GuidStream class.
		/// </summary>
		/// <param name="file">The file which owns the stream.</param>
		/// <param name="address">The start address of the stream.</param>
		/// <param name="size">The size of the stream.</param>
		internal GuidStream(PeCoffFile file, int address, int size) {
			// Read and store the underlying data for this stream
			this.streamContents = new byte[size];
			for (int i = address; i < (address + size); i++) {
				this.streamContents[i - address] = file.FileContents[i];
			}
		}

		/// <summary>
		/// Obtains the <see cref="Guid"/> at the specified <paramref name="index"/>. The index provided
		/// is obtained from the <see cref="GuidIndex.Value"/> property.
		/// </summary>
		/// <param name="index">The index for the guid to obtain.</param>
		/// <returns>The instantiated Guid</returns>
		/// <exception cref="ArgumentException">
		/// The index was outside the boundaries for the stream, there is no guid available. Check the
		/// data in the exception for more information.
		/// </exception>
		public Guid GetGuid(int index) {
			if (index <= 0 || index > this.streamContents.Length / sizeOfGuid) {
				ArgumentException ex = new ArgumentException("index");
				ex.Data["index"] = index;
				ex.Data["size"] = this.streamContents.Length / sizeOfGuid;
				throw ex;
			}

			int offset = (index * sizeOfGuid) - sizeOfGuid;	// -1 resets the offset to a zero based offset in the array
			byte[] guid = new byte[16];
			for(int i = offset; i < offset + sizeOfGuid; i++) {
				guid[i - offset] = this.streamContents[i];
			}
			return new Guid(guid);
		}

		/// <summary>
		/// Returns a collection of all the GUIDs defined in this stream.
		/// </summary>
		/// <returns>The dictionary of GUIDs and associated indexes.</returns>
		public Dictionary<int, Guid> GetAllGUIDs() {
			Dictionary<int, Guid> guids = new Dictionary<int, Guid>();
			byte[] currentGuid = new byte[16];

			for (int i = 0; i < this.streamContents.Length; i += sizeOfGuid) {
				for (int j = i; j < i + sizeOfGuid; j++) {
					currentGuid[j - i] = this.streamContents[j];
				}
				Guid current = new Guid(currentGuid);
				guids.Add((i + sizeOfGuid) / sizeOfGuid, current);
			}

			return guids;
		}
	}
}
