using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest.Issues {
	/// <summary>
	/// When a method has a generic parameter types they are displayed without the type details.
	/// E.g. List&lt;string&gt; is displayed as List&lt;&gt;.
	/// </summary>
	public class Issue149 {
		/// <summary>
		/// Tests the display of a external generic type with an external parameter.
		/// <see cref="List{T}"/>.
		/// <seealso cref="List{T}" />
		/// </summary>
		/// <param name="test"></param>
		public void EnteralGenericTypeExternalParameter(List<string> test) { }

		/// <summary>
		/// Tests the display of an external generic type with an internal parameter.
		/// </summary>
		/// <param name="test"></param>
		public void EnteralGenericTypeInternalParameter(List<TestStructure> test) { }

		/// <summary>
		/// Tests the display of internal type external parameter
		/// </summary>
		/// <param name="test"></param>
		public void InternalGenericTypeExternalParameter(Gah<string> test) { }

		/// <summary>
		/// Tests the display of an internal type and parameter.
		/// </summary>
		/// <param name="test"></param>
		public void InternalGenericTypeInternalParameter(Gah<TestStructure> test) { }
	}
}
