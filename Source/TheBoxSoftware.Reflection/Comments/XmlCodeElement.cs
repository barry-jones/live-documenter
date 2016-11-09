
namespace TheBoxSoftware.Reflection.Comments
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Base class for all the implementations of the XmlCodeComments.
    /// </summary>
    public abstract class XmlCodeElement
    {
        private static Regex _removeNewLines = new Regex(@"\n");
        /// <summary>
        /// Dictionary of all available XmlCodeElements and thier associated string
        /// representations.
        /// </summary>
        /// <remarks>
        /// This was created because parsing enumerations uses reflection, which is slow,
        /// and also it may throw exceptions for types not supported; which could happen
        /// a lot.
        /// </remarks>
        internal static Dictionary<string, XmlCodeElements> DefinedElements = new Dictionary<string, XmlCodeElements>() {
            {"b", XmlCodeElements.B},
            {"c", XmlCodeElements.C},
            {"code", XmlCodeElements.Code},
            {"example", XmlCodeElements.Example},
            {"exception", XmlCodeElements.Exception},
            {"i", XmlCodeElements.I},
            {"include", XmlCodeElements.Include},
            {"list", XmlCodeElements.List},
            {"listheader", XmlCodeElements.ListHeader},
            {"item", XmlCodeElements.ListItem},
            {"term", XmlCodeElements.Term},
            {"description", XmlCodeElements.Description},
            {"para", XmlCodeElements.Para},
            {"param", XmlCodeElements.Param},
            {"paramref", XmlCodeElements.ParamRef},
            {"permission", XmlCodeElements.Permission},
            {"remarks", XmlCodeElements.Remarks},
            {"returns", XmlCodeElements.Returns},
            {"see", XmlCodeElements.See},
            {"seealso", XmlCodeElements.SeeAlso},
            {"summary", XmlCodeElements.Summary},
            {"typeparam", XmlCodeElements.TypeParam},
            {"typeparamref", XmlCodeElements.TypeParamRef},
            {"value", XmlCodeElements.Value},
            {"#text", XmlCodeElements.Text}
            };

        private XmlCodeElements _element;
        private bool _isBlock;
        private bool _isInline;
        private string _text;

        /// <summary>
        /// Initialises a new instance of the base class, to be called from all derived
        /// classes so the element type is populated.
        /// </summary>
        /// <param name="element">The type of element this instance represents.</param>
        protected XmlCodeElement(XmlCodeElements element)
        {
            _element = element;
        }

        /// <summary>
        /// Removes the new lines from all the areas in the string.
        /// </summary>
        /// <param name="content">The content to remove the new lines from.</param>
        /// <returns>The new string without new lines.</returns>
        protected string RemoveNewLines(string content)
        {
            return _removeNewLines.Replace(content, " ");
        }

        /// <summary>
        /// Removes all the whitespace characters from the beginning and end of the
        /// provided <paramref name="content"/> string.
        /// </summary>
        /// <param name="content">The content to trim.</param>
        /// <returns>The trimmed string.</returns>
        protected string RemoveLeadingAndTrailingWhitespace(string content)
        {
            return content.Trim();
        }

        /// <summary>
        /// Obtains a string value that contains the textual portion of the
        /// XmlCodeElement.
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        /// <summary>
        /// Indicates if this is an inline level element.
        /// </summary>
        public bool IsInline
        {
            get { return _isInline; }
            set { _isInline = value; }
        }

        /// <summary>
        /// Indicates if this is a block level element.
        /// </summary>
        public bool IsBlock
        {
            get { return _isBlock; }
            set { _isBlock = value; }
        }

        /// <summary>
        /// Gets the type of element this instance refers to.
        /// </summary>
        public XmlCodeElements Element
        {
            get { return _element; }
        }
    }
}