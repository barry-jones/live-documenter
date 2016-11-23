
namespace TheBoxSoftware.Reflection.Comments
{
    using System.Text.RegularExpressions;
    using System.Xml;

    /// <summary>
    /// Not really an allowed xml code comment element; but this is used by the system to aid in the 
    /// parsing of inline textual elements from other <see cref="XmlCodeElement"/> implementations.
    /// </summary>
    /// <remarks>
    /// This class will only ever treat text as an inline element. Therefore it will strip multiple 
    /// new lines and reduce them to single spaces. If the user wants the text over multiple lines 
    /// they should use the para XmlCodeElement in their documentation.
    /// </remarks>
    public sealed class TextXmlCodeElement : XmlCodeElement
    {
        private static Regex innerTextReplacement = new Regex(@"( {2,}|^ |^[\n\t\r\v\f]+)", RegexOptions.Multiline);
        private static Regex titleTextReplacement = new Regex(@"([\n\t\r\v\f]+)", RegexOptions.Multiline);

        /// <summary>
        /// Initialises a new instance of the TextXmlCodeElement class.
        /// </summary>
        /// <param name="node">The node containing the text.</param>
        internal TextXmlCodeElement(XmlNode node)
            : base(XmlCodeElements.Text)
        {
            Text = innerTextReplacement.Replace(node.InnerText, string.Empty);
            Text = titleTextReplacement.Replace(Text, " ");

            // Make sure we have not removed too much whitespace. Check if the previous elements
            // a non text element and add a space if the current element defines space at the beginning.
            if(node.PreviousSibling != null
                && XmlCodeComment.DefinedElements.ContainsKey(node.PreviousSibling.Name)
                && XmlCodeElement.DefinedElements[node.PreviousSibling.Name] != XmlCodeElements.Text
                && node.InnerText.StartsWith(" "))
            {
                Text = " " + Text;
            }

            IsInline = true;
        }
    }
}