using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace TheBoxSoftware.Reflection.Comments {
	/// <summary>
	/// Not really an allowed xml code comment element; but this is used
	/// by the system to aid in the parsing of inline textual elements from
	/// other <see cref="XmlCodeElement"/> implementations.
	/// </summary>
	/// <remarks>
	/// This class will only ever treat text as an inline element. Therefore
	/// it will strip multiple new lines and reduce them to single spaces. If
	/// the user wants the text over multiple lines they should use the para
	/// XmlCodeElement in their documentation.
	/// </remarks>
	public sealed class TextXmlCodeElement : XmlCodeElement {
		/// <summary>
		/// Initialises a new instance of the TextXmlCodeElement class.
		/// </summary>
		/// <param name="node">The node containing the text.</param>
		internal TextXmlCodeElement(XmlNode node)
			: base(XmlCodeElements.Text) {
			Regex regex = new Regex(@"( {2,}|^ |^[\n\t\r\v\f]+)", RegexOptions.Multiline);
			this.Text = regex.Replace(node.InnerText, string.Empty);
			regex = new Regex(@"([\n\t\r\v\f]+)", RegexOptions.Multiline);
			this.Text = regex.Replace(this.Text, " ");

			// Make sure we have not removed too much whitespace. Check if the previous elements
			// a non text element and add a space if the current element defines space at the beginning.
			if (node.PreviousSibling != null 
				&& XmlCodeElement.DefinedElements[node.PreviousSibling.Name] != XmlCodeElements.Text 
				&& node.InnerText.StartsWith(" ")) {
				this.Text = " " + this.Text;
			}

			this.IsInline = true;
		}
	}
}
