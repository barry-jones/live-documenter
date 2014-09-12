using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Signitures {
	/// <summary>
	/// Enumeration of available major signiture types that can be defined
	/// in a .net assembly.
	/// </summary>
	public enum Signitures : byte {
		/// <summary>
		/// Signiture which describes the signiture or of a method or
		/// global function.
		/// </summary>
		MethodDef,
		/// <summary>
		/// Signiture which describes a call signiture of a method. Normally,
		/// this call signiture shall match exactly the Signiture specified
		/// in the definition of the target method.
		/// </summary>
		MethodRef,
		/// <summary>
		/// Signiture which captures a fields definition.
		/// </summary>
		Field,
		/// <summary>
		/// Signiture which captures a property definition. This includes details
		/// of the getter and setter methods.
		/// </summary>
		Property,
		/// <summary>
		/// Signiture which captures the types of all the local variables in a
		/// method.
		/// </summary>
		LocalVariable,
		/// <summary>
		/// Signiture which captures a desription for a type.
		/// </summary>
		TypeSpecification,
		/// <summary>
		/// Signiture which captures a description of a generic method.
		/// </summary>
		MethodSpecification,
		/// <summary>
		/// Signiture which captures the values defined against an attribute
		/// decleration.
		/// </summary>
		CustomAttribute
	}
}
