using System;
using System.Xml;

namespace TheBoxSoftware.Reflection.Comments
{
    /// <summary>
    /// The details of the comments associated with a single member.
    /// </summary>
    public sealed class XmlCodeComment : XmlContainerCodeElement
    {
        private CRefPath _member;
        private static XmlCodeComment _empty;

        /// <summary>
        /// Static constructor
        /// </summary>
        static XmlCodeComment()
        {
            XmlCodeComment.Empty = new XmlCodeComment();
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        private XmlCodeComment()
        {
        }

        /// <summary>
        /// Initialises a new XmlCodeComment instance.
        /// </summary>
        /// <param name="node">The node to parse the comment details from.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the <paramref name="node"/> is null.
        /// </exception>
        internal XmlCodeComment(XmlNode node)
        {
            if(node == null)
                throw new ArgumentNullException("node");
            this.Elements = XmlContainerCodeElement.ParseChildren(node);
        }

        /// <summary>
        /// The member which this XmlCodeComment is for.
        /// </summary>
        public CRefPath Member
        {
            get { return this._member; }
            set { this._member = value; }
        }

        /// <summary>
        /// Gets a valid but empty XmlCodeComment reference.
        /// </summary>
        public static XmlCodeComment Empty
        {
            get { return XmlCodeComment._empty; }
            private set { XmlCodeComment._empty = value; }
        }
    }
}