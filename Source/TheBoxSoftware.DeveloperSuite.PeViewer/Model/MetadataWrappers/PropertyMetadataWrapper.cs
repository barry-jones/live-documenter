
namespace TheBoxSoftware.DeveloperSuite.PEViewer.Model.MetadataWrappers
{
    using System.Collections.Generic;
    using TheBoxSoftware.Reflection.Core.COFF;

    internal class PropertyMetadataWrapper
    {
        public PropertyMetadataWrapper(MetadataStream file, List<MetadataRow> methods)
        {
            this.Items = new List<PropertyEntry>();
            foreach(PropertyMetadataTableRow current in methods)
            {
                this.Items.Add(new PropertyEntry(file.OwningFile.GetMetadataDirectory(), current));
            }
        }

        public List<PropertyEntry> Items { get; set; }

        public class PropertyEntry
        {
            public PropertyEntry(MetadataDirectory directory, PropertyMetadataTableRow row)
            {
                FileOffset = string.Format("0x{0:x}", row.FileOffset);
                Name = ((StringStream)directory.Streams[Streams.StringStream]).GetString(row.NameIndex.Value);
                Attributes = string.Format("0x{0:x}", row.Attributes);
                Type = string.Format("0x{0:x}", row.TypeIndex);
            }

            public string FileOffset { get; set; }

            public string Name { get; set; }

            public string Attributes { get; set; }

            public string Type { get; set; }
        }
    }
}
