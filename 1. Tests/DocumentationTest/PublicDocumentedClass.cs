using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest
{
	/// <summary>
	/// Public class to test filters
	/// </summary>
	public class PublicDocumentedClass
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

		protected class ProtectedClass
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

		internal protected class InternalProtectedClass
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

		/// <summary>
		/// Private documented class to test filters.
		/// </summary>
		private class PrivateDocumentedClass
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
}
