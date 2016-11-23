
namespace TheBoxSoftware.Reflection.Comments
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Xml;

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
            IsBlock = true;

            // The code element is pre-formatted. We need to make it look a little better
            // though by removing the leading whitespace from the lines of code. The indentation
            // needs to be preserved while making sure it starts left aligned at the boundary.
            int charsToTrim;
            string[] strippedStrings;
            
            strippedStrings = StripStartAndEndWhitespace(node);
            charsToTrim = CalculateLeadingSpaceToTrim(strippedStrings);
            TrimLeadingWhiteSpaceFromLines(strippedStrings, charsToTrim);

            // Store the new string
            Text = string.Join("\n", strippedStrings);
        }

        private static void TrimLeadingWhiteSpaceFromLines(string[] strippedStrings, int charsToTrim)
        {
            for(int i = 0; i < strippedStrings.Length; i++)
            {
                strippedStrings[i] = strippedStrings[i].Substring(charsToTrim);
            }
        }

        private static int CalculateLeadingSpaceToTrim(string[] strippedStrings)
        {
            Regex leadRegex = new Regex(@"(^\s*)");
            int charsToTrim = int.MaxValue;
            for(int i = 0; i < strippedStrings.Length; i++)
            {
                foreach(Match currentMatch in leadRegex.Matches(strippedStrings[i]))
                {
                    if(currentMatch.Success)
                    {
                        if(currentMatch.Captures[0].Length < charsToTrim && currentMatch.Captures[0].Length >= 0)
                        {
                            charsToTrim = currentMatch.Captures[0].Length;
                        }
                    }
                }
            }

            return charsToTrim;
        }

        private string[] StripStartAndEndWhitespace(XmlNode node)
        {
            List<string> allLines = new List<string>(node.InnerText.Split('\n'));

            for(int i = 0; i < allLines.Count; i++)
            {   // remove blank lines from the front
                if(string.IsNullOrEmpty(RemoveLeadingAndTrailingWhitespace(allLines[i])))
                {
                    allLines.RemoveAt(i);
                    i--;
                }
                else { break; }
            }

            for(int i = allLines.Count - 1; i > 0; i--)
            {   // remove blank lines from the rear
                if(string.IsNullOrEmpty(RemoveLeadingAndTrailingWhitespace(allLines[i])))
                {
                    allLines.RemoveAt(i);
                }
                else { break; }
            }

            string[] strippedStrings = allLines.ToArray();
            return strippedStrings;
        }
    }
}
