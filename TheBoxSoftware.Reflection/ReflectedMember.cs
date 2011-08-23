using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection {
	/// <summary>
	/// Base class for all elements that are reflected from a PeCoffFile.
	/// </summary>
	public abstract class ReflectedMember {
		private List<CustomAttribute> attributes = new List<CustomAttribute>();

		/// <summary>
		/// Represents an identifier that uniquly identifies this reflected element inside 
		/// its containing <see cref="AssemblyDef"/>.
		/// </summary>
		public virtual int UniqueId { get; set; }

		/// <summary>
		/// A reference to the assembly which defines this member.
		/// </summary>
		public AssemblyDef Assembly { get; set; }

		public virtual Visibility MemberAccess {
			get { return Visibility.NotApplicable; }
		}

		/// <summary>
		/// The attributes associated with this member.
		/// </summary>
		public List<CustomAttribute> Attributes {
			get { return this.attributes; }
			set { this.attributes = value; }
		}

		/// <summary>
		/// Returns an identifier which can uniquely identify this member across many <see cref="AssemblyDef"/>s.
		/// </summary>
		/// <returns>A globally unique identifier</returns>
		public virtual long GetGloballyUniqueId() {
			long id = ((long)this.Assembly.UniqueId) << 32;
			id += this.UniqueId;
			return id;
		}
	}
}
