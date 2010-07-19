﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection{
	using TheBoxSoftware.Reflection.Core.COFF;

	/// <summary>
	/// Describes the construction of a field in the CLR metadata.
	/// </summary>
	public class FieldDef : MemberRef {
		#region Methods
		/// <summary>
		/// Initialises a new instance of the FieldDef class based on the metadata provided
		/// </summary>
		/// <param name="assembly">The assembly that the field resides in</param>
		/// <param name="container">The type this field is contained in</param>
		/// <param name="row">The metadata row describing the field</param>
		/// <returns>The initialised field</returns>
		internal static FieldDef CreateFromMetadata(AssemblyDef assembly, TypeDef container, FieldMetadataTableRow row) {
			FieldDef field = new FieldDef();

			field.UniqueId = assembly.GetUniqueId();
			field.Assembly = assembly;
			field.Type = container;
			field.Name = assembly.StringStream.GetString(row.Name.Value);
			field.SignitureBlob = row.Signiture;
			field.Flags = row.Flags;
			field.Constants = new List<ConstantInfo>();

			return field;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Indicates if this field is system generated. .NET creates backing fields
		/// in certain situations.
		/// </summary>
		public bool IsSystemGenerated {
			get { return this.Name == "value__"; }
		}

		/// <summary>
		/// The flags detailing information about the field
		/// </summary>
		public FieldAttributes Flags { get; set; }

		/// <summary>
		/// TODO: Document
		/// </summary>
		public List<ConstantInfo> Constants { get; set; }

		/// <summary>
		/// Indicates if the field is declared as a constant.
		/// </summary>
		public bool IsConstant {
			get {
				return this.Constants.Count > 0;
			}
		}
		#endregion
	}
}
