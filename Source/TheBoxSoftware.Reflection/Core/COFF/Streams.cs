using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Core.COFF {
	/// <summary>
	/// Enumeration of all the streams that can be located in a PE/COFF
	/// file.
	/// </summary>
	public enum Streams : byte {
		/// <summary>
		/// The stream containing the .net metadata.
		/// </summary>
		MetadataStream,
		/// <summary>
		/// The stream containing the strings referenced by the metadata.
		/// </summary>
		StringStream,
		/// <summary>
		/// The stream which contains the signiuture and other information
		/// for the metadata.
		/// </summary>
		BlobStream,
		/// <summary>
		/// The stream which contains the user strings referenced by the
		/// metadata.
		/// </summary>
		USStream,
		/// <summary>
		/// The stream which contains any referenced GUIDs in the metadata.
		/// </summary>
		GuidStream
	}
}
