using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest
{
    /// <summary>
    /// Enter description here for class X. 
    /// ID string generated is "T:N.X". 
    /// </summary>
    public unsafe class AllOutputTypesClass
    {
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
        /// A static field
        /// </summary>
        public static string Field;

        /// <summary>
        /// Enter description here for field q.
        /// ID string generated is "F:N.X.q".
        /// </summary>
        public string q;

        /// <summary>
        /// THis is a readonly field
        /// </summary>
        public readonly int Readonly = 3;

        /// <summary>
        /// Enter description for constant PI.
        /// ID string generated is "F:N.X.PI".
        /// </summary>
        public const double PI = 3.14;

        /// <summary>
        /// A method which uses the default visibility
        /// </summary>
        void BasicDefaultMethod() { }

        /// <summary>
        /// A basic method that returns nothing and has no parameters
        /// </summary>
        public void BasicPublicMethod() { }

        /// <summary>
        /// A basic internal method
        /// </summary>
        internal void BasicInternalMethod() { }

        /// <summary>
        /// A basic protected internal method
        /// </summary>
        protected internal void BasicProtectedInternalMethod() { }

        /// <summary>
        /// A basic protected method
        /// </summary>
        protected void BasicProtectedMethod() { }

        /// <summary>
        /// A basic private method
        /// </summary>
        private void BasicPrivateMethod() { }

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
        /// <param name="a">Normal parameter</param>
        /// <param name="c">An optional parameter</param>
        public void inTest(out int o, int a, int c = 3) { o = 3; }

        /// <summary>
        /// Enter description for method gg.
        /// ID string generated is "M:N.X.gg(System.Int16[],System.Int32[0:,0:])". 
        /// </summary>
        /// <param name="array1">Describe parameter.</param>
        /// <param name="array">Describe parameter.</param>
        /// <returns>Describe return value.</returns>
        public int gg(short[] array1, int[,] array) { return 0; }

        /// <summary>
        /// A method that returns a custom type
        /// </summary>
        /// <returns></returns>
        public AllOutputTypesClass ReturnsOurClass() { return new AllOutputTypesClass(); }

        /// <summary>
        /// A built in long type returned from the method
        /// </summary>
        /// <returns></returns>
        public long BuildInLongTypeReturned() { return 3; }

        /// <summary>
        /// Array return type
        /// </summary>
        /// <returns></returns>
        public int[] ArrayReturnType() { return new int[0]; }

        /// <summary>
        /// Generic return type from method
        /// </summary>
        /// <returns></returns>
        public List<int> GenericReturnType() { return new List<int>(); }

        /// <summary>
        /// GenericMethodOfT
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> GenericMethodOfT<T>() { return new List<T>(); }

        /// <summary>
        /// RefParameters
        /// </summary>
        /// <returns></returns>
        public void RefParameters(ref int first) { }

        /// <summary>
        /// Normal parameters
        /// </summary>
        /// <param name="first"></param>
        public void NormalParameters(int first) { }

        /// <summary>
        /// Multiple parameters test
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="test"></param>
        /// <param name="allOut"></param>
        public void MultipleParameters(int first, ref int second, string test, AllOutputTypesClass allOut) {}

        /// <summary>
        /// An output parameter
        /// </summary>
        /// <param name="first"></param>
        public void OutParameter(out int first) { first = 3; }

        /// <summary>
        /// Default parameter
        /// </summary>
        /// <param name="first"></param>
        public void DefaultParameter(int first = 3) { }

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
        public void GenericMethod<T>(T anItem)
        {
            string s = anItem.ToString();
        }

        /// <summary>
        /// Jagged array documentation test
        /// </summary>
        /// <param name="jaggy">Jagged array return type</param>
        /// <returns>Another jagged array</returns>
        public string[][] JaggedReturnArray(string[][] jaggy)
        {
            return jaggy;
        }

        /// <summary>
        /// PrivateGet
        /// </summary>
        public string[] PrivateGet
        {
            private get;
            set;
        }

        /// <summary>
        /// PrivateGet
        /// </summary>
        public string[] PrivateSet
        {
            get;
            private set;
        }
    }

    public struct TestStructure
    {
    }
}