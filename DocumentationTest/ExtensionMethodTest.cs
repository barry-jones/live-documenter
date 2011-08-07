using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest {
	public static class ExtensionMethodTest {
		public static int ExtensionMethod(this AllOutputTypesClass c) {
			return 1;
		}
	}
}
