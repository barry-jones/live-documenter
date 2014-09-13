using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest {
	public static class ExtensionMethodTest {
		/// <summary>
		/// Summary for the ExtensionMethod.
		/// </summary>
		/// <param name="c">The type being extended.</param>
		/// <returns>The number 1.</returns>
		public static int ExtensionMethod(this AllOutputTypesClass c) {
			return 1;
		}

		/// <summary>
		/// Testing the use of a generic extension method.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="c"></param>
		/// <returns></returns>
		public static int GenericExtensionMethod<T>(this T c) {
			return 1;
		}
	}
}
