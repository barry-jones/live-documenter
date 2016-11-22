
namespace TheBoxSoftware.Reflection.Comments
{
    using System.Xml;

    /// <summary>
    /// CodeElement that describes an exception that can be thrown.
    /// </summary>
    public sealed class ExceptionXmlCodeElement : XmlContainerCodeElement
    {
        private CRefPath _member;

        /// <summary>
        /// Initialises a new instance of the ExceptionXmlCodeElement class.
        /// </summary>
        /// <param name="node">The node describing the exception.</param>
        public ExceptionXmlCodeElement(XmlNode node)
            : base(XmlCodeElements.Exception)
        {
            if(node.Attributes["cref"] == null)
            {
                throw new AttributeRequiredException("cref", XmlCodeElements.Exception);
            }

            Elements = Parse(node);
            _member = CRefPath.Parse(node.Attributes["cref"].Value);
        }

        /// <summary>
        /// The path to the exception this exception element refers to.
        /// </summary>
        public CRefPath Member
        {
            get { return _member; }
            set { _member = value; }
        }
    }
}