using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Model.MetadataWrappers
{
    using TheBoxSoftware.Reflection.Core.COFF;
    using TheBoxSoftware.Reflection.Core;

    public class AssemblyMetadataWrapper
    {
        public AssemblyMetadataWrapper(MetadataStream stream, List<MetadataRow> methods)
        {
            this.Items = new List<AssemblyEntry>();
            foreach(AssemblyMetadataTableRow current in methods)
            {
                this.Items.Add(new AssemblyEntry(stream.OwningFile.GetMetadataDirectory(), current));
            }
        }
        public List<AssemblyEntry> Items { get; set; }

        public class AssemblyEntry
        {
            public AssemblyEntry(MetadataDirectory directory, AssemblyMetadataTableRow row)
            {
                this.FileOffset = string.Format("0x{0:x}", row.FileOffset);
                this.Name = ((StringStream)directory.Streams[Streams.StringStream]).GetString(row.Name.Value);
                this.Version = row.GetVersion().ToString();
                this.Culture = ((StringStream)directory.Streams[Streams.StringStream]).GetString(row.Culture.Value);
                this.HashAlgId = row.HashAlgId.ToString();
                this.Flags = row.Flags.ToString();
                this.PublicKey = row.PublicKey.ToString();
            }

            public string FileOffset { get; set; }

            public string Name { get; set; }

            public string Version { get; set; }

            public string Culture { get; set; }

            public string HashAlgId { get; set; }

            public string Flags { get; set; }

            public string PublicKey { get; set; }
        }
    }
}