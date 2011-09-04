using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Core.COFF {
	/// <summary>
	/// Manages a map of details required for calculating coded indexes.
	/// </summary>
	/// <remarks>
	/// <para>
	/// There is some detail for coded indexes that requires some processing
	/// storing a map of those details speeds up processing in the application.
	/// </para>
	/// <para>
	/// Each PeCoffFile will require its coded index map as trying to use the
	/// same instance across multiple files will result in coded index resolution
	/// failures.
	/// </para>
	/// <para>
	/// This class is internal and is only used in <see cref="MetadataStream"/>s.
	/// </para>
	/// </remarks>
	/// <seealso cref="MetadataStream"/>
	internal sealed class CodedIndexMap : Dictionary<CodedIndexes, CodedIndex.Details> {
		// No implementation
	}
}
