using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest {
	/// <summary>
	/// T:DocumentationTest.GenericClass`1
	/// </summary>
	/// <typeparam name="T">T</typeparam>
	public class GenericClass<T> {
		/// <summary>
		/// M:DocumentationTest.GenericClass`1.GenericMethod``1(`0,``0)
		/// </summary>
		/// <typeparam name="U">U</typeparam>
		/// <param name="anItem"></param>
		/// <param name="secondItem"></param>
		/// <returns></returns>
		public string GenericMethod<N>(T anItem, N secondItem) {
			return anItem.ToString();
		}

        /// <summary>
        /// A test to check if child classes of generic types causes problems.
        /// </summary>
        public class ChildClassTest {
        }

        public class ChildGenericClass<S> {
			public void Add(T test) {
			}

			public class ChildChildGenericClass<U, J>
			{
				public void Add(U one, S parent, T parentParent)
				{
				}

				public void Add<Z>(Z gah) { }
			}
        }

        public class AnotherChildGenericClass<P> {
        }
	}

	/// <summary>
	/// T:DocumentationTest.TwoGenericClass`2
	/// </summary>
	/// <typeparam name="T">T</typeparam>
	/// <typeparam name="Z">Z</typeparam>
	public class TwoGenericClass<T, Z> {
		/// <summary>
		/// M:DocumentationTest.TwoGenericClass`2.Method(`0,`1)
		/// </summary>
		/// <param name="one"></param>
		/// <param name="two"></param>
		/// <returns></returns>
		public string Method(T one, Z two) {
			return one.ToString();
		}

		/// <summary>
		/// M:DocumentationTest.TwoGenericClass`2.GenericMethod``1(`0,`1,``0)
		/// </summary>
		/// <typeparam name="Y">Y</typeparam>
		/// <param name="one">one</param>
		/// <param name="two">two</param>
		/// <param name="three">three</param>
		/// <returns>Stuff</returns>
		public string GenericMethod<Y>(T one, Z two, Y three) {
			return one.ToString();
		}
	}

	/// <summary>
	/// This class inherits from a genric class to make sure the documentation
	/// is output correctly. The see and seealso links also test refering to
	/// generic classes in documentation. <see cref="GenericClass{T}" />
	/// </summary>
	/// <seealso cref="GenericClass{T}" />
	public class InheritedGenericClass : GenericClass<string> {
	}

    /// <summary>
    /// Summary Test 2
    /// </summary>
    public class InheritedGenericClassTest2 : GenericClass<int>
    {
    }

    /// <summary>
    /// Summary Test 3
    /// </summary>
    public class InheritedGenericClassTest3 : GenericClass<List<int>>
    {
    }

    /// <summary>
    /// Test summary Test 4.
    /// </summary>
    public class InheritedGenericClassTest4 : GenericClass<List<GenericClass<string>>>
    {
    }
}
