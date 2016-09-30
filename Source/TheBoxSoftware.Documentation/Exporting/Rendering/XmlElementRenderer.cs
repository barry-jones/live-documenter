using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheBoxSoftware.Reflection.Comments;

namespace TheBoxSoftware.Documentation.Exporting.Rendering
{
    /// <summary>
    /// Base class for XmlRenderers that are designed to handle specific
    /// XmlCodeComment elements.
    /// </summary>
    internal abstract class XmlElementRenderer : XmlRenderer
    {
        /// <summary>
        /// Checks if the <paramref name="element"/> is handled by an XmlElementRenderer.
        /// </summary>
        /// <param name="element">The element to check.</param>
        /// <returns>True if it can be handled else false.</returns>
        public static bool IsHandled(XmlCodeElement element)
        {
            bool isHandled = false;

            switch (element.Element)
            {
                case XmlCodeElements.See:
                case XmlCodeElements.List:
                    isHandled = true;
                    break;
            }

            return isHandled;
        }


        /// <summary>
        /// Factory method for instantiating correct XmlElementRenderers for the specified
        /// <paramref name="element"/>.
        /// </summary>
        /// <param name="associatedEntry">The entry this comment element was taken from.</param>
        /// <param name="element">The XML code comment element to handle.</param>
        /// <returns>A valid XmlRenderer for the <paramref name="element"/>.</returns>
        public static XmlRenderer Create(XmlRenderer from, Entry associatedEntry, XmlCodeElement element)
        {
            XmlElementRenderer renderer = null;

            switch (element.Element)
            {
                case XmlCodeElements.See:
                    renderer = new SeeXmlElementRenderer(associatedEntry, (SeeXmlCodeElement)element);
                    break;
                case XmlCodeElements.List:
                    renderer = new ListXmlElementRenderer(associatedEntry, (ListXmlCodeElement)element);
                    break;
            }
            renderer.Document = from.Document; // need to pass the reference over

            return renderer;
        }
    }
}