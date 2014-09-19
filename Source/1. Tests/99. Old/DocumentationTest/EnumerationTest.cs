using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest {
	public class EnumerationTest {
		public enum NoBase {
			AValue
		}
		public enum Bytebase : byte {
			AValue
		}

		public enum SBytebase : sbyte { }
		public enum Shortbase : short {}
		public enum UShortbase : ushort {}
		public enum Intbase : int {}
		public enum UIntbase : uint {}
		public enum Longbase : long {}

		public enum NoEntries { }
	}
}
