using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection{
	/// <summary>
	/// Creates a nice searchable and iterable version of the
	/// OpCodes class.
	/// </summary>
	public sealed class OpCodesMap : List<OpCode> {
		#region Fields
		private Dictionary<short, int> map = new Dictionary<short, int>();
		private static OpCodesMap singleton;
		#endregion

		#region Constructors
		/// <summary>
		/// Initialises a new instance of the OpCodesMap class
		/// </summary>
		private OpCodesMap() {
			Type t = typeof(OpCodes);
			foreach (System.Reflection.FieldInfo info in t.GetFields()) {
				OpCode code = (OpCode)info.GetValue(null);
				this.Add(code);
				map.Add(code.Value, this.IndexOf(code));
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Obtains a reference to the singleton
		/// </summary>
		/// <returns>The reference</returns>
		public static OpCodesMap GetSingleton() {
			if (OpCodesMap.singleton == null) {
				OpCodesMap.singleton = new OpCodesMap();
			}

			return OpCodesMap.singleton;
		}

		/// <summary>
		/// Returns the description of the instruction represented
		/// by 'instruction'
		/// </summary>
		/// <param name="instruction">The instruction</param>
		/// <returns>The OpCode details</returns>
		public OpCode GetCode(short instruction) {
			if (!this.map.ContainsKey(instruction)) {
				return OpCodes.Nop;
			}

			return this[this.map[instruction]];
		}
		#endregion
	}
}
