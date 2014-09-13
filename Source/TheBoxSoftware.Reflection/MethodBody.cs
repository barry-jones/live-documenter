using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection {
	/// <summary>
	/// Contains the details and MSIL for a method definition.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The internal constructor is so that these classes can be instantiated from
	/// a valid MethodDef instance. Further this is the only way these objects should
	/// be created.
	/// </para>
	/// </remarks>
	/// <seealso cref="MethodDef.GetMethodBody()"/>
	/// <seealso cref="MethodDef"/>
	public sealed class MethodBody {
		/// <summary>
		/// Initialsies a new instance of the MethodBody class.
		/// </summary>
		/// <param name="instructions">The instructions that make up the methods body.</param>
		/// <param name="maxStack">The maximum size of the stack for this method.</param>
		internal MethodBody(List<ILInstruction> instructions, Int32 maxStack) {
			this.Instructions = instructions;
		}

		#region Properties
		/// <summary>
		/// Obtains the MSIL instructions for this method.
		/// </summary>
		public List<ILInstruction> Instructions {
			get;
			private set;
		}

		/// <summary>
		/// Indicates the maximum number of items that appear on the stack in this
		/// method.
		/// </summary>
		public Int32 MaxStack {
			get;
			private set;
		}
		#endregion
	}
}
