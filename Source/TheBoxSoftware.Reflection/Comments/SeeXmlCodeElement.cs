
namespace TheBoxSoftware.Reflection.Comments
{
    using System.Xml;

    public sealed class SeeXmlCodeElement : XmlCodeElement
    {
        private CRefPath _member;

        internal SeeXmlCodeElement(XmlNode node)
            : base(XmlCodeElements.See)
        {
            if(node.Attributes["cref"] == null)
                throw new AttributeRequiredException("cref", XmlCodeElements.See);

            _member = CRefPath.Parse(node.Attributes["cref"].Value);
            switch(Member.PathType)
            {
                case CRefTypes.Type:
                    Text = _member.TypeName;
                    break;
                case CRefTypes.Namespace:
                    Text = _member.Namespace;
                    break;
                default:
                    Text = _member.ElementName;
                    break;
            }
            IsInline = true;
        }

        /// <summary>
        /// The member this elements points to.
        /// </summary>
        public CRefPath Member
        {
            get { return _member; }
            set { _member = value; }
        }
    }
}