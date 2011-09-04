using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest {
    internal class Test {
    }

	/// <summary>
	/// Public external class
	/// </summary>
	public class VisibilityTest {
		/// <summary>
		/// Public Nested Class
		/// </summary>
		public class PublicNestedClass {
		}

		/// <summary>
		/// Protected nested class
		/// </summary>
		protected class ProtectedNestedClass {
		}

		/// <summary>
		/// Internal protected class
		/// </summary>
		internal protected class InternalProtectedClass {
		}

		/// <summary>
		/// Internal class
		/// </summary>
		internal class InternalClass {
		}

		/// <summary>
		/// Private class
		/// </summary>
		private class PrivateClass {
		}

        private void PrivateMethod() {
        }

        protected void ProtectedMethod() {
        }

        internal void InternalMethod() {
        }
        public void PublicMethod() {
        }

        private string privateField;
        protected string protectedField;
        public string publicField;
        internal string internalField;

        public event EventHandler PublicEvent;
        private event EventHandler PrivateEvent;
        protected event EventHandler ProtectedEvent;
        internal event EventHandler InternalEvent;

        public string PublicProperty { get; set; }
        protected string ProtectedProperty { get; set; }
        internal string InternalProperty { get; set; }
        private string Property { get; set; }
	}
}
