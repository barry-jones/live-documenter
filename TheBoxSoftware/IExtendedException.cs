using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware {
	/// <summary>
	/// Interface to extend exceptions to provide more details than the basic message.
	/// </summary>
	public interface IExtendedException {
		/// <summary>
		/// When implemented by a base type should return formatted text which attempts
		/// to utilise the internal information to return more helpful information
		/// than is noramlly provided via message.
		/// </summary>
		/// <returns>The formatted extended details</returns>
		string GetExtendedInformation();
	}
}
