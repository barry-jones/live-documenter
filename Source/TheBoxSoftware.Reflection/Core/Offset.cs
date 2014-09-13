using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Core {
	/// <summary>
	/// Represents a cursor through a large file, the current value
	/// represents the current offset.
	/// </summary>
	public sealed class Offset {
		private int current;

		/// <summary>
		/// Initialises a new instance of the Offset structure
		/// </summary>
		/// <param name="start">The starting offset</param>
		public Offset(int start) {
			this.current = start;
		}

		/// <summary>
		/// Shifts the offset by the specified 'by' number, but returns the value
		/// before the shift operation.
		/// </summary>
		/// <param name="by">The number to shift the offset by</param>
		/// <returns>The current shift value before the offset is shifted</returns>
		/// <remarks>
		/// This method allows the shift of the offset at the same time as its use
		/// allowing the current offset and its progression to be linked more closely
		/// making it more readable.
		/// </remarks>
		public int Shift(int by) {
			int now = this.current;
			this.current += by;
			return now;
		}

		/// <summary>
		/// Implicit conversion to an integer
		/// </summary>
		/// <param name="offset">The offset that needs conversion</param>
		/// <returns>The converted integer value</returns>
		public static implicit operator int(Offset offset) {
			return offset.Current;
		}

		/// <summary>
		/// Implicit conversion from an integer
		/// </summary>
		/// <param name="offset">The offset to initialise the offset to</param>
		/// <returns>The initialised offset</returns>
		public static implicit operator Offset(int offset) {
			return new Offset(offset);
		}

		/// <summary>
		/// The current value of the offset
		/// </summary>
		public int Current {
			get { return this.current; }
			set { this.current = value; }
		}
	}
}
