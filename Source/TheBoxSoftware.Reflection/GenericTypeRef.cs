using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection {
	using TheBoxSoftware.Reflection.Core.COFF;

	/// <summary>
	/// A class that represents a generic type; generic types are defined on
	/// types and methods. These types however will never resolve to an actual
	/// type instance because they are determined at call time, not at definition.
	/// </summary>
	/// <seealso cref="TypeDef" />
	/// <seealso cref="MethodDef" />
	public sealed class GenericTypeRef : TypeRef {
		/// <summary>
		/// Initialises and populates a new GenericTypeRef instance based on the details
		/// provided in the metadata row.
		/// </summary>
		/// <param name="assembly">The assembly the type is defined in.</param>
		/// <param name="metadata">The metadata directory</param>
		/// <param name="row">The metadata row.</param>
		/// <returns>The populated instance.</returns>
		internal static GenericTypeRef CreateFromMetadata(AssemblyDef assembly, MetadataDirectory metadata, GenericParamMetadataTableRow row) {
			GenericTypeRef genericType = new GenericTypeRef();
			genericType.UniqueId = assembly.GetUniqueId();
			genericType.Sequence = (Int16)row.Number;
			genericType.Name = assembly.StringStream.GetString(row.Name.Value);
			// this.Flags = FieldReader.ToUInt16(contents, offset.Shift(2));
			return genericType;
		}

		#region Properties
		/// <summary>
		/// A number that represents the location in the generic type sequence
		/// this GenericTypeRef resides.
		/// </summary>
		/// <remarks>
		/// When multiple generic types are defined against a method or type, the
		/// number is used by the metadata to distinguish which type is being called
		/// or passed in signitures.
		/// </remarks>
		public Int16 Sequence { get; set; }
		#endregion
	}
}
