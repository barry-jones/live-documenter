using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest {
	/// <summary>
	/// If this class is displayed as part of its solution we can test if cross
	/// loaded library references work.
	/// </summary>
	public class ExternalInheritanceTest : TheBoxSoftware.Reflection.ReflectedMember {
		public ExternalInheritanceTest() : base() { }

		public override TheBoxSoftware.Reflection.Visibility MemberAccess
		{
			get { throw new NotImplementedException(); }
		}
	}
}
