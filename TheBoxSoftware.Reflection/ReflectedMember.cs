using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection {
	/// <summary>
	/// Base class for all elements that are reflected from a PeCoffFile.
	/// </summary>
	public abstract class ReflectedMember {
		/// <summary>
		/// Represents an identifier that uniquly identifies this reflected element.
		/// </summary>
		public virtual int UniqueId { get; set; }

		/// <summary>
		/// A reference to the assembly which defines this member.
		/// </summary>
		public AssemblyDef Assembly { get; set; }
	}
}
