using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax {
	/// <summary>
	/// Details class which provides access to information necessary
	/// to understand the construction of a parameter.
	/// </summary>
	/// <see cref="Signitures.TypeDetails"/>
	internal class ParameterDetails {
		/// <summary>
		/// Initialises a new instance of the ParameterDetails class.
		/// </summary>
		public ParameterDetails() { }

		/// <summary>
		/// Initialises a new instance of the ParameterDetails class.
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="details">The details of the type for the parameter.</param>
		public ParameterDetails(string name, Signitures.TypeDetails details) {
			this.TypeDetails = details;
			this.Name = name;
		}
		
		/// <summary>
		/// The full details of the type.
		/// </summary>
		public Signitures.TypeDetails TypeDetails { get; set; }

		/// <summary>
		/// The name of the parameter.
		/// </summary>
		public string Name { get; set; }
	}
}