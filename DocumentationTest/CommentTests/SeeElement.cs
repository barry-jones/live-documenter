using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest.CommentTests {
	/// <summary>
	///	This class tests various uses and basterdisations of the see element.
	/// </summary>
	/// <remarks>
	/// The <see cref="SeeElement"/> can be used to link to other elements.
	/// Zero parameter methods: <see cref="SeeElement.InvaldCasing()"/>
	/// Parameterised methods: <see cref="SeeElement.InvalidCasing(string)"/>
	/// Internal methods: <see cref="SeeElement.InternalMethod()"/>
	/// Generic methods: <see cref="SeeElement.GenericMethod{T}()"/>
	/// Parameterised generic methods: <see cref="SeeElement.ParameterisedGenericMethod{T}(T)"/>
	/// </remarks>
	public class SeeElement {
		/// <summary>
		/// This tests the casing of the element and its attributes.
		/// </summary>
		/// <remarks>
		/// We can <see cref="InvalidCasing"/>! <SEE cref="InvalidCasing"/> and <see CREF="InvalidCasing"/>.
		/// </remarks>
		public void InvalidCasing() { }

		public void InvalidCasing(string s) { }

		internal void InternalMethod() { }

		public void GenericMethod<T>() { }

		public void ParameterisedGenericMethod<T>(T s) { }
	}
}
