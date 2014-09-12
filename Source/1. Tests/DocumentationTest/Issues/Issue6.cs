using System;
using System.Collections.Generic;
using System.Linq;

namespace DocumentationTest.Issues {
	public class Issue6 {
		public void TestMethod() {
			List<int> testList = new List<int>();
			testList.Select(item => item < 5);
		}
	}
}
