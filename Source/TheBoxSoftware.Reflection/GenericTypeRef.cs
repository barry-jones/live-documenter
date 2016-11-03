
namespace TheBoxSoftware.Reflection
{
    using Core.COFF;

    /// <summary>
    /// A class that represents a generic type; generic types are defined on
    /// types and methods. These types however will never resolve to an actual
    /// type instance because they are determined at call time, not at definition.
    /// </summary>
    /// <seealso cref="TypeDef" />
    /// <seealso cref="MethodDef" />
    public sealed class GenericTypeRef : TypeRef
    {
        /// <summary>
        /// Initialises and populates a new GenericTypeRef instance based on the details
        /// provided in the metadata row.
        /// </summary>
        /// <param name="references">All the required references the create requires to build the type.</param>
        /// <param name="fromRow">The metadata row.</param>
        /// <returns>The populated instance.</returns>
        internal static GenericTypeRef CreateFromMetadata(BuildReferences references, GenericParamMetadataTableRow fromRow)
        {
            GenericTypeRef genericType = new GenericTypeRef();

            genericType.UniqueId = references.Assembly.CreateUniqueId();
            genericType.Sequence = fromRow.Number;
            genericType.Name = references.Assembly.StringStream.GetString(fromRow.Name.Value);
            // this.Flags = FieldReader.ToUInt16(contents, offset.Shift(2));

            return genericType;
        }

        /// <summary>
        /// A number that represents the location in the generic type sequence
        /// this GenericTypeRef resides.
        /// </summary>
        /// <remarks>
        /// When multiple generic types are defined against a method or type, the
        /// number is used by the metadata to distinguish which type is being called
        /// or passed in signitures.
        /// </remarks>
        public ushort Sequence { get; set; }
    }
}