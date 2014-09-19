using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest.Issues {
	/// <summary>
	/// When members inherit from or have children derive from them the inheritance
	/// tree shows all parents and children with links regardless of if the type
	/// has been filtered out.
	/// </summary>
	/// <remarks>
	/// Because the derived type being displayed will always be atleast the same
	/// visibility as its parents we don't need to test that. We only need to test
	/// children.
	/// </remarks>
	public class Issue160 {
		public class PublicDerivedClass : Issue160 { }
		internal class InternalDerivedClass : Issue160 { }
		internal protected class InternalProtectedClass : Issue160 { }
		protected class ProtectedDerivedClass : Issue160 { }
		private class PrivateDerivedClass : Issue160 { }
	}
}
