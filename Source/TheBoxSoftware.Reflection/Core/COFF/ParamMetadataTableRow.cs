using System;

namespace TheBoxSoftware.Reflection.Core.COFF
{
    // TODO: When resolving these metadata rows, we should also look in to the constant metadata
    //  table and resolve optional parameters to the constant defenitions this will enable us
    //  to correctly output the syntax for these parameters.

    /// <summary>
    /// Represents a single entry in the Param Metadata table.
    /// </summary>
    /// <remarks>
    /// The In, Out and Optional modifiers are specified in the <see cref="Flags"/> property. Further,
    /// if it is defined as optional the constant values supplied are recorded in the Constants
    /// Metadata directory p209 of .NET IL Assembler book.
    /// 
    /// The constant can be accessed but the value is only available on the signature blob which makes
    /// it a little more difficult to access the value.
    /// </remarks>
    public class ParamMetadataTableRow : MetadataRow
    {
        /// <summary>
        /// Initialises a new instance of the ParamMetadataTableRow class
        /// </summary>
        /// <param name="stream">The stream containing the metadata</param>
        /// <param name="contents">The contents of the file</param>
        /// <param name="offset">The offset of the current row</param>
        public ParamMetadataTableRow(MetadataStream stream, byte[] contents, Offset offset)
        {
            this.FileOffset = offset;
            this.Flags = (ParamAttributeFlags)FieldReader.ToUInt16(contents, offset.Shift(2));
            this.Sequence = FieldReader.ToUInt16(contents, offset.Shift(2));
            this.Name = new StringIndex(stream, offset);
        }

        /// <summary>
        /// A 2-byte bitmask of ParamAttributes
        /// </summary>
        public ParamAttributeFlags Flags { get; set; }

        /// <summary>
        /// The sequence of the parameter
        /// </summary>
        public UInt16 Sequence { get; set; }

        /// <summary>
        /// Index in to the string heap
        /// </summary>
        public StringIndex Name { get; set; }
    }
}