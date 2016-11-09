/*
 * Handles the XML code comment implementation of the list element as defined 
 * at: http://msdn.microsoft.com/en-us/library/vstudio/y3ww3c7e(v=vs.100).aspx
 */

namespace TheBoxSoftware.Reflection.Comments
{
    using System.Xml;

    /// <summary>
    /// An internal representation of the list XML element.
    /// </summary>
    /// <remarks>
    /// A list element can represent a numbered or bulletted list or a table by way of a type
    /// attribute. If no type attribute is specified it will default to a bulleted list.
    /// </remarks>
    public sealed class ListXmlCodeElement : XmlContainerCodeElement
    {
        private ListTypes _listType;

        /// <summary>
        /// Initialises a new instance of the ListXmlCodeElement class.
        /// </summary>
        /// <param name="node">The node the list is based on.</param>
        internal ListXmlCodeElement(XmlNode node)
            : base(XmlCodeElements.List)
        {
            Elements = Parse(node);
            IsBlock = true;
            _listType = ListTypes.Bullet; // default

            // the node should have a type attribute, if not default to bullet list
            XmlAttribute typeAttribute = node.Attributes["type"];
            if(typeAttribute != null)
            {
                switch(typeAttribute.Value.ToLower())
                {
                    case "table":
                        _listType = ListTypes.Table;
                        break;
                    case "number":
                        _listType = ListTypes.Number;
                        break;
                }
            }
        }

        /// <summary>
        /// Checks the contents of the list to determine if it should be handled
        /// as a table or a list.
        /// </summary>
        /// <returns>True if the displayer should display a table.</returns>
        public bool IsTable()
        {
            return _listType == ListTypes.Table;
        }

        /// <summary>
        /// Gets or sets the style for bullets displayed in this list.
        /// </summary>
        public ListTypes ListType
        {
            get { return _listType; }
        }
    }
}