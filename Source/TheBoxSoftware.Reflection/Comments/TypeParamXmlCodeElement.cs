using System.Xml;

namespace TheBoxSoftware.Reflection.Comments
{
    public sealed class TypeParamXmlCodeElement : XmlCodeElement
    {
        internal TypeParamXmlCodeElement(XmlNode node)
            : base(XmlCodeElements.TypeParam)
        {
            if(node.Attributes["name"] == null) { throw new AttributeRequiredException("name", XmlCodeElements.TypeParam); }
            this.Name = node.Attributes["name"].Value;
            this.Text = this.RemoveNewLines(node.InnerText);
        }

        /// <summary>
        /// The name this xml code element refers to.
        /// </summary>
        public string Name { get; set; }
    }
}