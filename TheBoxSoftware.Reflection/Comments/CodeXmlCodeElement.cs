using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace TheBoxSoftware.Reflection.Comments {
	/// <summary>
	/// Represents a pre-formatted code entry as a block level element.
	/// </summary>
	public sealed class CodeXmlCodeElement : XmlCodeElement {
		/// <summary>
		/// Initialises a new instance of the CodeXmlCodeElement class.
		/// </summary>
		/// <param name="node">The node to parse.</param>
		internal CodeXmlCodeElement(XmlNode node)
			: base(XmlCodeElements.Code) {
			this.IsBlock = true;

			// The code element is pre-formatted. We need to make it look a little better
			// though by removing the leading whitespace from the lines of code. However there
			// may be multiple levels of indentation and we do not want to remove it all.
			// 1. Strip the start and end whitespace from the string.
			// 2. Search and capture all leading whitespace on new lines
			// 3. Get the smallest lead (apart from the last)
			// 4. Trim that smallest lead from all other lines in the block.

			// 1
			string strippedString = this.RemoveLeadingAndTrailingWhitespace(node.InnerText);

			// 2 & 3
			Regex leadRegex = new Regex(@"(^\s*)", RegexOptions.Multiline);
			int charsToTrim = int.MaxValue;
			foreach (Match currentMatch in leadRegex.Matches(strippedString)) {
				if (currentMatch.Success) {
					if (currentMatch.Captures[0].Length < charsToTrim && currentMatch.Captures[0].Length > 0) {
						charsToTrim = currentMatch.Captures[0].Length;
					}
				}
			}

			// 4
			string[] lines = strippedString.Split('\n');
			for (int i = 1; i < lines.Length; i++) {	// 1 because we have already trimmed the first entry
				lines[i] = lines[i].Substring(charsToTrim);
			}

			// Store the new string
			this.Text = string.Join("\n", lines);
		}
	}
}
