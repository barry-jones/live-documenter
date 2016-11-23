
namespace TheBoxSoftware.Reflection.Comments
{
    using System.Xml;

    public sealed class ParamXmlCodeElement : XmlContainerCodeElement
    {
        private string _name;

        internal ParamXmlCodeElement(XmlNode node)
            : base(XmlCodeElements.Param)
        {
            if(node.Attributes["name"] == null)
                throw new AttributeRequiredException("name", XmlCodeElements.Param);

            IsBlock = true;
            Elements = Parse(node);
            _name = node.Attributes["name"].Value;
        }

        /// <summary>
        /// The name of the parameter this code comment refers to.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}