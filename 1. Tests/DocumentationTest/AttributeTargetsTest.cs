using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest
{
	[AttributeUsage(AttributeTargets.All)]
	class ReturnAttribute : Attribute {
	}

	/// <summary>
	/// Tests attribute targets and how these types are handled by our reflection library.
	/// </summary>
	/// <remarks>
	/// Attribute target removes ambiguity see http://msdn.microsoft.com/en-us/library/b3787ac0(v=vs.80).aspx 
	/// for more info.
	/// </remarks>
	[type: Serializable]
	class AttributeTargetsTest
	{
		/// <summary>
		/// Atribute sepecific to method/.
		/// </summary>
		[method: ReturnAttribute]
		void AMethod() { }

		/// <summary>
		/// Attribute specific to return type.
		/// </summary>
		/// <returns></returns>
		[return: ReturnAttribute]
		bool ARetMethod() { return true; }
	}
}
