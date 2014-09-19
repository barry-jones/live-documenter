using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest {
	/// <summary>
	/// These are comments for a delegate DoSomething.
	/// </summary>
	/// <param name="something">The parameter</param>
	/// <returns>An integer</returns>
	public delegate int DoSomething(string something);

	/// <summary>
	/// Documented generic delegate class
	/// </summary>
	/// <typeparam name="J">The generic type parameter</typeparam>
	/// <param name="hoorah">The parameter</param>
	public delegate void DoSomethingElse<J>(J hoorah);
}
