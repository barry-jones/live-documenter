using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest {
	/// <summary>
	/// When a class is defined and has LINQ expressions etc, the comiler
	/// sets the namespace on the generated class as the type (it is a nested
	/// type). However we do not display the system generated types therefor
	/// giving the impression of a class being displayed as a namespace.
	/// 
	/// This is a test class for that bug.
	/// </summary>
	public class ClassAsNamespaceBug {
		public void TestMethod() {
			int[] numbers = new int[10];
			var t = from n in numbers
						  where n > 0
						  select n;
			numbers.First(n => n > 0 && n < 0 && n == 0);
		}
	}
}
