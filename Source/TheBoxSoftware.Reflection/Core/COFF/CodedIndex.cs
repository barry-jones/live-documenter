
namespace TheBoxSoftware.Reflection.Core.COFF
{
    using System;

    /// <summary>
    /// Represents a value which specifies both a table and an index in to that table
    /// from a single UInt16 or UInt32.
    /// </summary>
    public struct CodedIndex
    {
        private MetadataTables _table;
        private Index _index;
        
        /// <summary>
        /// Initialises a already determined instance of a CodedIndex class.
        /// </summary>
        /// <param name="table">The table the coded index is for</param>
        /// <param name="index">The index in the table</param>
        public CodedIndex(MetadataTables table, UInt32 index)
        {
            _table = table;
            _index = index;
        }

        public static bool operator ==(CodedIndex first, CodedIndex second)
        {
            return (first.Table == second.Table && first.Index == second.Index);
        }

        public static bool operator !=(CodedIndex first, CodedIndex second)
        {
            return (first.Table != second.Table || first.Index != second.Index);
        }

        /// <summary>
        /// Returns a string representation of this coded index
        /// </summary>
        /// <returns>A string</returns>
        public override string ToString()
        {
            return $"Table: {_table.ToString()}, Index:{_index.ToString()}";
        }

        /// <field>
        /// The MetadataTables value indicating the table this index is for.
        /// </field>
        public MetadataTables Table
        {
            get { return _table; }
            set { _table = value; }
        }

        /// <field>
        /// The index in to the MetadataTable.
        /// </field>
        public Index Index
        {
            get { return _index; }
            set { _index = value; }
        }
    }
}