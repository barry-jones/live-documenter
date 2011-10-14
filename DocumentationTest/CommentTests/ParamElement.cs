using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest.CommentTests {
	/// <summary>
	/// Tests the various uses and basterdisations of the param element.
	/// </summary>
	/// <remarks>
	/// <para>The param element should be used in the comment for a method declaration to describe
	/// one of the parmaeters for the method. To document multiple parameters use multiple param 
	/// tags.</para>
	/// <para>The param tag name property is used to point to the correct parameter.</para>
	/// </remarks>
	/// <example>
	/// <code>
	/// &lt;param name="first"&gt;The first parameter.&lt;\param&gt;
	/// void MyMethod(int first) {
	/// }
	/// </code>
	/// </example>
	public class ParamElement {
		/// <summary>
		/// Defines a single parameter
		/// </summary>
		/// <param name="first"></param>
		public void SingleParameter(int first) {}

		/// <summary>
		/// Multiple parameters defined.
		/// </summary>
		/// <param name="first">First</param>
		/// <param name="second">Second</param>
		public void MultipleParameters(int first, int second) {}

		/// <summary>
		/// Param with an invalid name.
		/// </summary>
		/// <param name="second">Invalid parameter name.</param>
		public void InvalidName(int first) {}
		
		/// <summary>
		/// Test the param element defined on a method with no parameters.
		/// </summary>
		/// <param name="doesnotexist">Description.</param>
		public void ParamDefinedOnNoParameters() {}
	}
}
