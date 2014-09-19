using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest {
	public struct OpTest {
		// Comparison operators
		/// <summary>
		/// Comparison operator test
		/// </summary>
		/// <param name="one">The first comparison parmeter</param>
		/// <param name="two">The second comparison parameter</param>
		/// <returns>The result of the comparison</returns>
		public static bool operator ==(OpTest one, OpTest two) { return true; }
		public static bool operator !=(OpTest one, OpTest two) { return true; }
		public static bool operator >(OpTest one, OpTest two) { return true; }
		public static bool operator <(OpTest one, OpTest two) { return true; }
		public static bool operator >=(OpTest one, OpTest two) { return true; }
		public static bool operator <=(OpTest one, OpTest two) { return true; }

		// Unary operators
		/// <summary>The false operator</summary>
		/// <param name="one">The unary operator parameter</param>
		/// <returns>A unary operator conversion</returns>
		public static bool operator false(OpTest one) { return false; }
		public static bool operator true(OpTest one) { return false; }
		public static OpTest operator ++(OpTest one) { return one; }
		public static OpTest operator --(OpTest one) { return one; }
		public static OpTest operator -(OpTest one) { return one; }
		public static OpTest operator +(OpTest one) { return one; }
		public static OpTest operator !(OpTest one) { return one; }
		public static OpTest operator ~(OpTest one) { return one; }

		// Binary operators
		/// <summary>
		/// A binary operator test
		/// </summary>
		/// <param name="one">The first binary parameter</param>
		/// <param name="two">The second binary parameter</param>
		/// <returns>The result of the binary operation</returns>
		public static OpTest operator +(OpTest one, OpTest two) { return one; }
		public static OpTest operator -(OpTest one, OpTest two) { return one; }
		public static OpTest operator *(OpTest one, OpTest two) { return one; }
		public static OpTest operator /(OpTest one, OpTest two) { return one; }
		public static OpTest operator %(OpTest one, OpTest two) { return one; }
		// The & symbol is also the concatenation symbol in VB
		public static OpTest operator &(OpTest one, OpTest two) { return one; }
		public static OpTest operator |(OpTest one, OpTest two) { return one; }
		public static OpTest operator ^(OpTest one, OpTest two) { return one; }
		public static OpTest operator <<(OpTest one, int two) { return one; }
		public static OpTest operator >>(OpTest one, int two) { return one; }

		/// <summary>
		/// An implicit conversion
		/// </summary>
		/// <param name="o">The paramtere</param>
		/// <returns>The converted value</returns>
		public static implicit operator Int32(OpTest o) { return 0; }
		public static implicit operator ConvTest(OpTest o) { return new ConvTest(); }

		/// <summary>
		/// Explicit convertion
		/// </summary>
		/// <param name="o">OpTest</param>
		/// <returns>Int32</returns>
		public static explicit operator UInt32(OpTest o) { return 0; }
		/// <summary>
		/// Another test explicit
		/// </summary>
		/// <param name="o">Parmatere</param>
		/// <returns>Return explicit OpTest ExConvTest</returns>
		public static explicit operator ExConvTest(OpTest o) { return new ExConvTest(); }
	}

	public struct ConvTest {}
	public struct ExConvTest { }
}
