using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace TheBoxSoftware.Reflection.Comments
{
	/// <summary>
	/// Represents a pre-formatted code entry as a block level element.
	/// </summary>
	public sealed class CodeXmlCodeElement : XmlCodeElement 
    {
		/// <summary>
		/// Initialises a new instance of the CodeXmlCodeElement class.
		/// </summary>
		/// <param name="node">The node to parse.</param>
		internal CodeXmlCodeElement(XmlNode node)
			: base(XmlCodeElements.Code) 
        {
			this.IsBlock = true;

			// The code element is pre-formatted. We need to make it look a little better
			// though by removing the leading whitespace from the lines of code. However there
			// may be multiple levels of indentation and we do not want to remove it all.
			// 1. Strip the start and end whitespace from the string.
			// 2. Search and capture all leading whitespace on new lines
			// 3. Get the smallest lead (apart from the last)
			// 4. Trim that smallest lead from all other lines in the block.

			// 1
			List<string> allLines = new List<string>(node.InnerText.Split('\n'));
			for (int i = 0; i < allLines.Count; i++) {	// remove blank lines from the front
				if (string.IsNullOrEmpty(this.RemoveLeadingAndTrailingWhitespace(allLines[i])))
                {
					allLines.RemoveAt(i);
					i--;
				}
				else { break; }
			}
			for (int i = allLines.Count - 1; i > 0; i--) {	// remove blank lines from the rear
				if (string.IsNullOrEmpty(this.RemoveLeadingAndTrailingWhitespace(allLines[i]))) 
                {
					allLines.RemoveAt(i);
				}
				else { break; }
			}
			string[] strippedStrings = allLines.ToArray();

			// 2 & 3
			Regex leadRegex = new Regex(@"(^\s*)");
			int charsToTrim = int.MaxValue;
			for (int i = 0; i < strippedStrings.Length; i++)
            {
				foreach (Match currentMatch in leadRegex.Matches(strippedStrings[i]))
                {
					if (currentMatch.Success) 
                    {
						if (currentMatch.Captures[0].Length < charsToTrim && currentMatch.Captures[0].Length >= 0) {
							charsToTrim = currentMatch.Captures[0].Length;
						}
					}
				}
			}

			// 4
			for (int i = 0; i < strippedStrings.Length; i++) 
            {
				strippedStrings[i] = strippedStrings[i].Substring(charsToTrim);
			}

			// Store the new string
			this.Text = string.Join("\n", strippedStrings);
		}
	}
}
