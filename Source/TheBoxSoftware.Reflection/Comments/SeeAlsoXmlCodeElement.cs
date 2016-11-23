
namespace TheBoxSoftware.Reflection.Comments
{
    using System.Xml;

    /// <summary>
    /// This XML element allows people to associate other useful elements to the
    /// current documentation. I.e. for further reading.
    /// </summary>
	public sealed class SeeAlsoXmlCodeElement : XmlCodeElement
    {
        private CRefPath _member;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="node">The node describing the seealso xml element.</param>
        internal SeeAlsoXmlCodeElement(XmlNode node)
            : base(XmlCodeElements.SeeAlso)
        {
            if(node.Attributes["cref"] == null)
                throw new AttributeRequiredException("cref", XmlCodeElements.SeeAlso);

            _member = CRefPath.Parse(node.Attributes["cref"].Value);
            switch(_member.PathType)
            {
                case CRefTypes.Type:
                    Text = _member.TypeName;
                    break;
                case CRefTypes.Namespace:
                    Text = _member.Namespace;
                    break;
                default:
                    if(!string.IsNullOrEmpty(Member.ElementName))
                    {
                        Text = _member.ElementName;
                    }
                    else if(!string.IsNullOrEmpty(node.Attributes["cref"].Value))
                    {
                        Text = node.Attributes["cref"].Value.Substring(2);
                    }
                    break;
            }
            IsInline = true;
        }

        /// <summary>
        /// Obtains the member this see also element refers to.
        /// </summary>
        public CRefPath Member
        {
            get { return _member; }
            set { _member = value; }
        }
    }
}