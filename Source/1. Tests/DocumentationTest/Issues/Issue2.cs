using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest.Issues
{
	/// <summary>
	/// Structure which implements interfaces
	/// </summary>
	public struct Issue2 : IComparable
	{
		#region IComparable Members
		/// <summary>
		/// A documented implementation of an interface method.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public int CompareTo(object obj)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}
