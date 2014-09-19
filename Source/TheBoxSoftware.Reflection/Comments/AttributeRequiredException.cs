using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Comments 
{
	/// <summary>
	/// Exception which details that an attribute on one of the XmlCodeElements
	/// is missing when it is required.
	/// </summary>
	public sealed class AttributeRequiredException : ApplicationException
    {
		/// <summary>
		/// Initialises a new instance of the AttributeRequiredException class.
		/// </summary>
		/// <param name="attribute">The attribute that was missing</param>
		/// <param name="fromElement">The element the attribute is missing from</param>
		public AttributeRequiredException(string attribute, XmlCodeElements fromElement)
			: base(string.Format("Required attribute '{0}' not found on '{1}' node.", attribute, fromElement.ToString().ToLower())) 
        {
			this.Attribute = attribute;
		}

		/// <summary>
		/// The attribute that was missing from the <see cref="XmlCodeElement"/>.
		/// </summary>
		public string Attribute { get; set; }
	}
}
