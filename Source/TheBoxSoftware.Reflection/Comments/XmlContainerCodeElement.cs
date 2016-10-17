using System.Xml;

namespace TheBoxSoftware.Reflection.Comments
{
    public class XmlContainerCodeElement : XmlCodeElement
    {
        /// <summary>
        /// Initialises a new instance of the XmlContainerCodeElement class.
        /// </summary>
        protected XmlContainerCodeElement(XmlCodeElements element)
            : base(element)
        {
            this.Elements = new XmlCodeElementCollection();
        }

        /// <summary>
        /// Private default constructor
        /// </summary>
        /// <remarks>
        /// It doesnt matter which type is passed through, this method can only be used
        /// internally. And currently helps with the static parse children method.
        /// </remarks>
        internal XmlContainerCodeElement() : base(XmlCodeElements.C) { }

        /// <summary>
        /// Parses the specified <paramref name="parentNode"/> and returns the
        /// parsed elements.
        /// </summary>
        /// <param name="parentNode">The parent node to parse the xml code elements from.</param>
        /// <returns>The collection of elements.</returns>
        internal static XmlCodeElementCollection ParseChildren(XmlNode parentNode)
        {
            XmlContainerCodeElement container = new XmlContainerCodeElement();
            return container.Parse(parentNode);
        }

        /// <summary>
        /// Parses the child elements in to instances of <see cref="XmlCodeElement"/>
        /// instances and returns them as a collection.
        /// </summary>
        /// <param name="parentNode">The block level element to parse.</param>
        /// <returns>The collection of parsed instances.</returns>
        protected XmlCodeElementCollection Parse(XmlNode parentNode)
        {
            XmlCodeElementCollection childElements = new XmlCodeElementCollection();
            XmlNode currentChild;

            int childNodeCount = parentNode.ChildNodes.Count;
            for(int i = 0; i < childNodeCount; i++)
            {
                currentChild = parentNode.ChildNodes[i];
                if(!XmlCodeElement.DefinedElements.ContainsKey(currentChild.Name))
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("The current element type '{0}' is not supported", currentChild.Name));
                }
                else
                {
                    XmlCodeElements type = XmlCodeElement.DefinedElements[currentChild.Name.ToLower()];
                    try
                    {
                        switch(type)
                        {
                            case XmlCodeElements.B: childElements.Add(new BoldXmlCodeElement(currentChild)); break;
                            case XmlCodeElements.C: childElements.Add(new CXmlCodeElement(currentChild)); break;
                            case XmlCodeElements.Code: childElements.Add(new CodeXmlCodeElement(currentChild)); break;
                            case XmlCodeElements.Example: childElements.Add(new ExampleXmlCodeElement(currentChild)); break;
                            case XmlCodeElements.Exception: childElements.Add(new ExceptionXmlCodeElement(currentChild)); break;
                            case XmlCodeElements.I: childElements.Add(new ItalicXmlCodeElement(currentChild)); break;
                            case XmlCodeElements.Para: childElements.Add(new ParaXmlCodeElement(currentChild)); break;
                            case XmlCodeElements.Param: childElements.Add(new ParamXmlCodeElement(currentChild)); break;
                            case XmlCodeElements.ParamRef: childElements.Add(new ParamRefXmlCodeElement(currentChild)); break;
                            case XmlCodeElements.Permission: childElements.Add(new PermissionXmlCodeElement(currentChild)); break;
                            case XmlCodeElements.Remarks: childElements.Add(new RemarksXmlCodeElement(currentChild)); break;
                            case XmlCodeElements.Returns: childElements.Add(new ReturnsXmlCodeElement(currentChild)); break;
                            case XmlCodeElements.See: childElements.Add(new SeeXmlCodeElement(currentChild)); break;
                            case XmlCodeElements.SeeAlso: childElements.Add(new SeeAlsoXmlCodeElement(currentChild)); break;
                            case XmlCodeElements.Summary: childElements.Add(new SummaryXmlCodeElement(currentChild)); break;
                            case XmlCodeElements.Text: childElements.Add(new TextXmlCodeElement(currentChild)); break;
                            case XmlCodeElements.TypeParam: childElements.Add(new TypeParamXmlCodeElement(currentChild)); break;
                            case XmlCodeElements.TypeParamRef: childElements.Add(new TypeParamRefXmlCodeElement(currentChild)); break;
                            case XmlCodeElements.Value: childElements.Add(new ValueXmlCodeElement(currentChild)); break;
                            // List releated code elements (all block level elements)
                            case XmlCodeElements.List: childElements.Add(new ListXmlCodeElement(currentChild)); break;
                            case XmlCodeElements.ListHeader: childElements.Add(new ListHeaderXmlCodeElement(currentChild)); break;
                            case XmlCodeElements.ListItem: childElements.Add(new ListItemXmlCodeElement(currentChild)); break;
                            case XmlCodeElements.Description: childElements.Add(new DescriptionXmlCodeElement(currentChild)); break;
                            case XmlCodeElements.Term: childElements.Add(new TermXmlCodeElement(currentChild)); break;
                            default:
                                break;
                        }
                    }
                    catch(AttributeRequiredException ex)
                    {
                        childElements.Add(new ErrorXmlCodeElement(ex));
                    }
                }
            }
            return childElements;
        }

        /// <summary>
        /// Collection of all child elements for this XmlContainerCodeElement.
        /// </summary>
        public XmlCodeElementCollection Elements { get; set; }
    }
}