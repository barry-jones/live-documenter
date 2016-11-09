
namespace TheBoxSoftware.Reflection.Comments
{
    public interface ICommentSource
    {
        /// <summary>
        /// Indicates if the underlying source exists and can be used.
        /// </summary>
        /// <returns></returns>
        bool Exists();

        /// <summary>
        /// Gets the comment associated with the <see cref="CRefPath"/>.
        /// </summary>
        /// <param name="crefPath">The identifier for the element to get the summary information.</param>
        /// <returns>The XmlCodeComment if found XmlCodeComment.Empty</returns>
        /// <remarks>
        /// Implementers should return XmlCodeComment.Empty when the source is unable
        /// to resolve the path a comment in the source.
        /// </remarks>
        XmlCodeComment GetComment(CRefPath crefPath);

        /// <summary>
        /// Returns just the summary element associated with the <see cref="CRefPath"/>.
        /// </summary>
        /// <param name="crefPath">The identifier for the element to get the summary information.</param>
        /// <returns>The XmlCodeComment if found otherwise XmlCodeComment.Empty</returns>
        /// <remarks>
        /// Implementers should return XmlCodeComment.Empty when the source is unable
        /// to resolve the path a comment in the source.
        /// </remarks>
        XmlCodeComment GetSummary(CRefPath crefPath);

        string GetXml(CRefPath crefPath);
    }
}
