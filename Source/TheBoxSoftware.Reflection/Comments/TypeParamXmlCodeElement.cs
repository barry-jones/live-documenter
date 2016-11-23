
namespace TheBoxSoftware.Reflection.Comments
{
    using System.Xml;

    public sealed class TypeParamXmlCodeElement : XmlCodeElement
    {
        private string _name;

        internal TypeParamXmlCodeElement(XmlNode node)
            : base(XmlCodeElements.TypeParam)
        {
            if(node.Attributes["name"] == null)
                throw new AttributeRequiredException("name", XmlCodeElements.TypeParam);

            _name = node.Attributes["name"].Value;
            Text = RemoveNewLines(node.InnerText);
        }

        /// <summary>
        /// The name this xml code element refers to.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}