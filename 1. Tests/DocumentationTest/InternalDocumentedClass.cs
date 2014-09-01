using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest
{
	/// <summary>
	/// Internal documed class to test filters.
	/// </summary>
	internal class InternalDocumentedClass
	{
		public string PublicProperty { get; set; }
		public string PublicField;
		public void PublicMethod() { }

		protected string ProtectedProperty { get; set; }
		protected string ProtectedField;
		protected void ProtectedMethod() { }

		protected internal string ProtectedInternalProperty { get; set; }
		protected internal string ProtectedInternalField;
		protected internal void ProtectedInternalMethod() { }

		internal string InternalProperty { get; set; }
		internal string InternalField;
		internal void InternalMethod() { }

		private string PrivateProperty { get; set; }
		private string PrivateField;
		private void PrivateMethod() { }
	}
}
