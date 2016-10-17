namespace TheBoxSoftware.Reflection.Comments
{
    /// <summary>
    /// An enumeration of the available types that a <see cref="ListXmlCodeElement"/>
    /// can be.
    /// </summary>
    public enum ListTypes
    {
        /// <summary>
        /// The list had a type of table and should be handled as a table.
        /// </summary>
        Table,

        /// <summary>
        /// The list had a type of bullet and should be handled as
        /// a bulletted list.
        /// </summary>
        Bullet,

        /// <summary>
        /// The list had a type of number and should be handled as
        /// a numbered list.
        /// </summary>
        Number
    }
}