using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest {
	public class InheritanceTest {
		public abstract class AbstractClass {
		}

		public sealed class SealedClass {
		}

		public class NormalClass {
		}

		public static class StaticClass {
		}
	}

	public class InheritanceTest_FirstChild : InheritanceTest {
	}

	public class InheritanceTest_SecondChild : InheritanceTest {
	}
}
