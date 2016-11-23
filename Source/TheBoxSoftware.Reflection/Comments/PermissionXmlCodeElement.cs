
namespace TheBoxSoftware.Reflection.Comments
{
    using System.Xml;

    /// <summary>
    /// Represents the permission XML code element.
    /// </summary>
    public sealed class PermissionXmlCodeElement : XmlContainerCodeElement
    {
        private CRefPath _member;

        /// <summary>
        /// Initialises a new instance of the PermissionXmlCodeElement class.
        /// </summary>
        /// <param name="node">The XML node representing the permission details.</param>
        /// <exception cref="AttributeRequiredException">The cref attribute was missing.</exception>
        internal PermissionXmlCodeElement(XmlNode node)
            : base(XmlCodeElements.Permission)
        {
            if(node.Attributes["cref"] == null)
            {
                throw new AttributeRequiredException("cref", XmlCodeElements.Permission);
            }

            _member = CRefPath.Parse(node.Attributes["cref"].Value);
            Elements = Parse(node);
            IsBlock = true;
        }

        /// <summary>
        /// The member (i.e. permission set) this permission points to.
        /// </summary>
        public CRefPath Member
        {
            get { return _member; }
            set { _member = value; }
        }
    }
}