using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest.CommentTests {
	/// <summary>
	/// Tests the various uses and basterdisations of the seealso element.
	/// </summary>
	public sealed class SeeAlsoElement {
		/// <summary>
		/// Tests the use of a seealso element on a property.
		/// </summary>
		/// <seealso cref="SeeAlsoElement"/>
		public string SeeAlsoOnProperty { get; set; }

		/// <summary>
		/// Tests the use of a seealso element on a field.
		/// </summary>
		/// <seealso cref="SeeAlsoElement"/>
		public string SeeAlsoOnField;

		/// <summary>
		/// Test the use of the seealso element on a method
		/// </summary>
		/// <seealso cref="SeeAlsoElement"/>
		public void SeeAlsoOnMethod() {}

		/// <summary>
		/// Tests the use of the seealso element when referencing generic types and methods
		/// defined in the current assembly.
		/// </summary>
		/// <seealso cref="DocumentationTest.GenericClass{T}" />
		/// <seealso cref="DocumentationTest.GenericClass{T}.GenericMethod{N}(T,N)"/>
		public void ReferenceInternalGeneric() {}

		/// <summary>
		/// Tests the use of the seealso element when referencing generic types and methods
		/// defined in an external assembly.
		/// </summary>
		/// <seealso cref="System.Collections.Generic.List{T}" />
		/// <seealso cref="System.Linq.Enumerable.Any{TSource}()"/>
		public void ReferenceExternalGeneric() {}

		/// <summary>
		/// Tests references from seealso elements to members in assemblies that are not referenced
		/// by this assembly.
		/// </summary>
		/// <seealso cref="TheBoxSoftware.Documentation.ExportSettings" />
		/// <seealso cref="TheBoxSoftware.Documentation.Rendering.IRenderer{T}" />
		public void ReferenceANonReferencedElements() {}
	}
}
