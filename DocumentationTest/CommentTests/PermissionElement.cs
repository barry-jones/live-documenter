using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest.CommentTests {
	/// <summary>
	/// Tests the various uses and basterdisations of hte permission element.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// <example>
	/// <code>
	/// &lt;permission cref="System.Security.PermissionSet"&gt;Everyone can access this method.&lt/permission&gt;
	/// class Test {
	/// }
	/// </code>
	/// </example>
	public class PermissionElement {
		/// <summary>
		/// 
		/// </summary>
		/// <permission cref="System.Security.PermissionSet">Everyone can access this class.</permission>
		public class PermissionOnClass { }

		/// <summary>
		/// Tests a single permission elment on a method.
		/// </summary>
		/// <permission cref="System.Security.PermissionSet">Everyone can access this class.</permission>
		public void SinglePermissionMethod() {}

		/// <summary>
		/// Tests the use of multiple permissions entries
		/// </summary>
		/// <permission cref="System.Security.PermissionSet">Everyone can access this class.</permission>
		/// <permission cref="System.Security.ReadOnlyPermissionSet">Not everyone can access this class.</permission>
		public void MultiplePermissions() {}

		/// <summary>
		/// Tests permission on properties
		/// </summary>
		/// <permission cref="System.Security.PermissionSet">Everyone can access this class.</permission>
		public string Property { get; set; }

		/// <summary>
		/// Tests permissions on fields
		/// </summary>
		/// <permission cref="System.Security.PermissionSet">Everyone can access this class.</permission>
		public string Field;
	}
}
