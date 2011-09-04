using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest {
	/// <summary>
    /// Enter description here for class X. 
    /// ID string generated is "T:N.X". 
    /// </summary>
	public unsafe class AllOutputTypesClass {
        /// <summary>
        /// Enter description here for the first constructor.
        /// ID string generated is "M:N.X.#ctor".
        /// </summary>
        public AllOutputTypesClass() { }


        /// <summary>
        /// Enter description here for the second constructor.
        /// ID string generated is "M:N.X.#ctor(System.Int32)".
        /// </summary>
        /// <param name="i">Describe parameter.</param>
		public AllOutputTypesClass(int i) { }


        /// <summary>
        /// Enter description here for field q.
        /// ID string generated is "F:N.X.q".
        /// </summary>
        public string q;


        /// <summary>
        /// Enter description for constant PI.
        /// ID string generated is "F:N.X.PI".
        /// </summary>
        public const double PI = 3.14;


        /// <summary>
        /// Enter description for method f.
        /// ID string generated is "M:N.X.f".
        /// </summary>
        /// <returns>Describe return value.</returns>
        public int f() { return 1; }


        /// <summary>
        /// Enter description for method bb.
        /// ID string generated is "M:N.X.bb(System.String,System.Int32@,System.Void*)".
        /// </summary>
        /// <param name="s">Describe parameter.</param>
        /// <param name="y">Describe parameter.</param>
        /// <param name="z">Describe parameter.</param>
        /// <returns>Describe return value.</returns>
        public int bb(string s, ref int y, void* z) { return 1; }

		/// <summary>
		/// Enter description for method bb.
		/// ID string generated is "M:N.X.bb(System.String,System.Int32@,DocumentationTest.TestStructure*)".
		/// </summary>
		/// <param name="s">Describe parameter.</param>
		/// <param name="y">Describe parameter.</param>
		/// <param name="z">Describe parameter.</param>
		/// <returns>Describe return value.</returns>
		public int bb(string s, ref int y, TestStructure* z) { return 1; }

		/// <summary>
		/// ID generated is "M:N.X.test(System.Collections.Generic.List{System.String[]})".
		/// </summary>
		/// <param name="test">test parameter</param>
		public void test(List<string[]> test) { int n = test.Count; }

		/// <summary>
		/// "M:DocumentationTest.AllOutputTypesClass.inTest(System.Int32@)".
		/// </summary>
		/// <param name="o">Described parameter</param>
		public void inTest(out int o) { o = 3; }

        /// <summary>
        /// Enter description for method gg.
        /// ID string generated is "M:N.X.gg(System.Int16[],System.Int32[0:,0:])". 
        /// </summary>
        /// <param name="array1">Describe parameter.</param>
        /// <param name="array">Describe parameter.</param>
        /// <returns>Describe return value.</returns>
        public int gg(short[] array1, int[,] array) { return 0; }


        /// <summary>
        /// Enter description for operator.
        /// ID string generated is "M:N.X.op_Addition(N.X,N.X)". 
        /// </summary>
        /// <param name="x">Describe parameter.</param>
        /// <param name="xx">Describe parameter.</param>
        /// <returns>Describe return value.</returns>
		public static AllOutputTypesClass operator +(AllOutputTypesClass x, AllOutputTypesClass xx) { return x; }


        /// <summary>
        /// Enter description for property.
        /// ID string generated is "P:N.X.prop".
        /// </summary>
        public int prop { get { return 1; } set { } }


        /// <summary>
        /// Enter description for event.
        /// ID string generated is "E:N.X.d".
        /// </summary>
        public event D d;

		/// <summary>
		/// Enter description for property.
		/// ID string generated is "P:N.X.Item(System.String)".
		/// </summary>
		/// <param name="s">Describe parameter.</param>
		/// <returns></returns>
		public int this[string s] { get { return 1; } }

		/// <summary>
		/// Enter description for property.
		/// ID string generated is "P:N.X.Item(System.String, System.Int32)".
		/// </summary>
		/// <param name="s">Describe parameter.</param>
		/// <returns></returns>
		public int this[string s, int forFun] { get { return 1; } }

        /// <summary>1
        /// Enter description for class Nested.
        /// ID string generated is "T:N.X.Nested".
        /// </summary>
        public class Nested { }


        /// <summary>
        /// Enter description for delegate.
        /// ID string generated is "T:N.X.D". 
        /// </summary>
        /// <param name="i">Describe parameter.</param>
        public delegate void D(int i);


        /// <summary>
        /// Enter description for operator.
        /// ID string generated is "M:N.X.op_Explicit(N.X)~System.Int32".
        /// </summary>
        /// <param name="x">Describe parameter.</param>
        /// <returns>Describe return value.</returns>
		public static explicit operator int(AllOutputTypesClass x) { return 1; }

		/// <summary>
		/// M:DocumentationTest.AllOutputTypesClass.GenericMethod``1(``0)
		/// </summary>
		/// <typeparam name="T">T Type parameter details</typeparam>
		/// <param name="anItem">The parameter anItem</param>
		public void GenericMethod<T>(T anItem) {
			string s = anItem.ToString();
		}

		/// <summary>
		/// Jagged array documentation test
		/// </summary>
		/// <param name="jaggy">Jagged array return type</param>
		/// <returns>Another jagged array</returns>
		public string[][] JaggedReturnArray(string[][] jaggy) {
			return jaggy;
		}

		/// <summary>
		/// PrivateGet
		/// </summary>
		public string[] PrivateGet {
			private get;
			set;
		}

		/// <summary>
		/// PrivateGet
		/// </summary>
		public string[] PrivateSet {
			get;
			private set;
		}
	}

	public struct TestStructure {
	}
}
