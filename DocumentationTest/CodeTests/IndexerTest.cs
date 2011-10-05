using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest.CodeTests{
	/// <summary>
	/// Tests verious uses and basterdisations of indexers in types.
	/// </summary>
	/// <remarks>
	/// Because of the way indexers work we need to define a new child class
	/// for each test.
	/// </remarks>
	public class IndexerTest {
		/// <summary>
		/// Tests the display and use of return types in Indexers
		/// </summary>
		public class ReturnTypeTests {
			/// <summary>
			/// Integer return type
			/// </summary>
			/// <param name="one">One</param>
			/// <returns>Int return type</returns>
			public int this[int one] { 
				get { return 0; }
				set { }
			}

			/// <summary>
			/// String return type
			/// </summary>
			/// <param name="one">One</param>
			/// <param name="two">Two</param>
			/// <returns>String return type</returns>
			public string this[int one, int two] { 
				get { return string.Empty; }
				set { }
			}
		}

		/// <summary>
		/// Tests the different setter and getter variations
		/// </summary>
		public class SettersAndGetters {
			/// <summary>
			/// No setter
			/// </summary>
			/// <param name="one">One</param>
			/// <returns>Return</returns>
			public int this[int one]  {
				get { return 0; }
			}

			/// <summary>
			/// No getter
			/// </summary>
			/// <param name="one">One</param>
			/// <param name="two">Two</param>
			/// <returns>Return</returns>
			public int this[int one, int two] {
				set { }
			}

			public void set_Item(int one, int two, int three, int value) {}
		}
	}
}
