
namespace TheBoxSoftware.Reflection.Comments
{
    using System.Xml;

    public sealed class TypeParamRefXmlCodeElement : XmlCodeElement
    {
        private string _name;

        internal TypeParamRefXmlCodeElement(XmlNode node)
            : base(XmlCodeElements.TypeParamRef)
        {
            if(node.Attributes["name"] == null)
                throw new AttributeRequiredException("name", XmlCodeElements.TypeParamRef);

            _name = node.Attributes["name"].Value;
            Text = Name;
            IsInline = true;
        }

        /// <summary>
        /// The name this parameter reference refers to.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}