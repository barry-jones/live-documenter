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
	}
}
