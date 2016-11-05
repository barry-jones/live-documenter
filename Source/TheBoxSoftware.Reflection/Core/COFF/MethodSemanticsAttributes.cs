
namespace TheBoxSoftware.Reflection.Core.COFF
{
    using System;

    /// <summary>
    /// An enumeration of available relationship references
    /// for methods to properties, events etc.
    /// </summary>
    /// <seealso cref="MethodSemanticsMetadataTableRow" />
    [Flags]
    public enum MethodSemanticsAttributes
    {
        /// <summary>
        /// Getter for a <see cref="PropertyMetadataTableRow"/>.
        /// </summary>
        Setter = 0x0001,

        /// <summary>
        /// Setter for a <see cref="PropertyMetadataTableRow"/>.
        /// </summary>
        Getter = 0x0002,

        /// <summary>
        /// Other method for a <see cref="PropertyMetadataTableRow"/> or
        /// <see cref="EventMetadataTableRow"/>.
        /// </summary>
        Other = 0x0004,

        /// <summary>
        /// Add method for a <see cref="EventMetadataTableRow"/>.
        /// </summary>
        AddOn = 0x0008,

        /// <summary>
        /// Remove method for a <see cref="EventMetadataTableRow"/>.
        /// </summary>
        RemoveOn = 0x0016,

        /// <summary>
        /// Fire method for a <see cref="EventMetadataTableRow"/>.
        /// </summary>
        Fire = 0x0032
    }
}