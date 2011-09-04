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

		/// <summary>
		/// You can specify the cref path explicitly and the compiler will not attempt to resolve it. This
		/// causes issues with our cref parser.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		///		<item>Type which is a method: <see cref="T:DocumentationTest.CommentTests.InvalidCasing"/>.</item>
		///		<item>Not existent: <see cref="E:InvalidCasing"/>.</item>
		/// </list>
		/// </remarks>
		public void InvalidSelfCreatedCRef() { }

		/// <summary>
		/// You can specify the cref path explicitly and the compiler will not attempt to resolve it. This
		/// causes issues with our cref parser.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		///		<item>Type: <see cref="T:DocumentationTest.CommentTests.SeeElement"/>.</item>
		/// </list>
		/// </remarks>
		public void ValidSelfCreatedCRef() { }

		/// <summary>
		/// Tests the see element against different method visibility modifiers. This is to enable
		/// testing of the filters.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		///		<item>See <see cref="PublicClass.PublicMethod"/>.</item>
		///		<item>See <see cref="PublicClass.TestInternalMethod"/>.</item>
		///		<item>See <see cref="PublicClass.TestProtectedInternalMethod"/>.</item>
		///		<item>See <see cref="PublicClass.TestProtectedMethod"/>.</item>
		///		<item>See <see cref="PublicClass.TestPrivateMethod"/>.</item>
		/// </list>
		/// </remarks>
		public void TestSeeMethodVisibility() { }

		/// <summary>
		/// Tests the see element against different method visibility modifiers. This is to enable
		/// testing of the filters.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		///		<item>See <see cref="PublicClass"/>.</item>
		///		<item>See <see cref="InternalClass"/>.</item>
		///		<item>See <see cref="InternalProtectedClass"/>.</item>
		///		<item>See <see cref="ProtectedClass"/>.</item>
		///		<item>See <see cref="PrivateClass"/>.</item>
		/// </list>
		/// </remarks>
		public void TestSeeTypeVisibility() {}

		/// <summary>
		/// Tests the see element against different method visibility modifiers. This is to enable
		/// testing of the filters.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		///		<item>See <see cref="PublicClass.PublicField"/>.</item>
		///		<item>See <see cref="PublicClass.InternalField"/>.</item>
		///		<item>See <see cref="PublicClass.InternalProtectedField"/>.</item>
		///		<item>See <see cref="PublicClass.ProtectedField"/>.</item>
		///		<item>See <see cref="PublicClass.PrivateField"/>.</item>
		/// </list>
		/// </remarks>
		public void TestSeeFieldVisibility() {}

		/// <summary>
		/// Tests the see element against different method visibility modifiers. This is to enable
		/// testing of the filters.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		///		<item>See <see cref="PublicClass.PublicProperty"/>.</item>
		///		<item>See <see cref="PublicClass.InternalProperty"/>.</item>
		///		<item>See <see cref="PublicClass.InternalProtectedProperty"/>.</item>
		///		<item>See <see cref="PublicClass.ProtectedProperty"/>.</item>
		///		<item>See <see cref="PublicClass.PrivateProperty"/>.</item>
		/// </list>
		/// </remarks>
		public void TestSeePropertyVisibility() {}

		/// <summary>
		/// Tests the see element against different method visibility modifiers. This is to enable
		/// testing of the filters.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		///		<item>See <see cref="PublicClass.PublicEvent"/>.</item>
		///		<item>See <see cref="PublicClass.InternalEvent"/>.</item>
		///		<item>See <see cref="PublicClass.InternalProtectedEvent"/>.</item>
		///		<item>See <see cref="PublicClass.ProtectedEvent"/>.</item>
		///		<item>See <see cref="PublicClass.PrivateEvent"/>.</item>
		/// </list>
		/// </remarks>
		public void TestSeeEventVisibility() { }

		#region Internal Test Class
		public class PublicClass {
			public void PublicMethod() { }
			protected void TestProtectedMethod() {}
			internal void TestInternalMethod() {}
			protected internal void TestProtectedInternalMethod() {}
			private void TestPrivateMethod() {}

			public string PublicField;
			internal string InternalField;
			internal protected string InternalProtectedField;
			protected string ProtectedField;
			private string PrivateField;

			public EventHandler PublicEventHandler;
			internal EventHandler InternalEventHandler;
			internal protected EventHandler InternalProtectedEventHandler;
			protected EventHandler ProtectedEventHandler;
			private EventHandler PrivateEventHandler;

			public event EventHandler PublicEvent {
				add { this.InternalEventHandler += value; }
				remove { this.InternalEventHandler -= value; }
			}
			internal event EventHandler InternalEvent {
				add { this.InternalEventHandler += value; }
				remove { this.InternalEventHandler -= value; }
			}
			internal protected event EventHandler InternalProtectedEvent {
				add { this.InternalEventHandler += value; }
				remove { this.InternalEventHandler -= value; }
			}
			protected event EventHandler ProtectedEvent {
				add { this.InternalEventHandler += value; }
				remove { this.InternalEventHandler -= value; }
			}
			private event EventHandler PrivateEvent {
				add { this.InternalEventHandler += value; }
				remove { this.InternalEventHandler -= value; }
			}

			public string PublicProperty { get; set; }
			internal string InternalProperty { get; set; }
			internal protected string InternalProtectedProperty { get; set; }
			protected string ProtectedProperty { get; set; }
			private string PrivateProperty { get; set; }
		}
		internal class InternalClass { }
		internal protected class InternalProtectedClass { }
		protected class ProtectedClass { }
		private class PrivateClass { }
		#endregion
	}
}
