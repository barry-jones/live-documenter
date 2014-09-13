using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection {
	using TheBoxSoftware.Reflection.Core.COFF;

	/// <summary>
	/// Test
	/// </summary>
	public class ConstantInfo {
		public static ConstantInfo CreateFromMetadata(AssemblyDef assembly, MetadataStream stream, ConstantMetadataTableRow row) {
			ConstantInfo constant = new ConstantInfo();
			constant.Value = (int)row.Value.Value;
			return constant;
		}

		public int Value { get; set; }
	}
}
