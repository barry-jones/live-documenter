using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest.Issues
{
	/// <summary>
	/// <para><seealso cref="Issue9"/> That see also block will cause an exception.</para>
	/// <para>Some text <seealso cref="Issue9"/> This will be invisible.</para>
	/// </summary>
	class Issue135 {
	}
}
