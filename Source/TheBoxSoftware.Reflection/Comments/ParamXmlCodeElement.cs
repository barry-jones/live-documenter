
namespace TheBoxSoftware.Reflection.Comments
{
    using System.Xml;

    public sealed class ParamXmlCodeElement : XmlContainerCodeElement
    {
        internal ParamXmlCodeElement(XmlNode node)
            : base(XmlCodeElements.Param)
        {
            if(node.Attributes["name"] == null) { throw new AttributeRequiredException("name", XmlCodeElements.Param); }
            this.IsBlock = true;
            this.Elements = this.Parse(node);
            this.Name = node.Attributes["name"].Value;
        }

        /// <summary>
        /// The name of the parameter this code comment refers to.
        /// </summary>
        public string Name { get; set; }
    }
}