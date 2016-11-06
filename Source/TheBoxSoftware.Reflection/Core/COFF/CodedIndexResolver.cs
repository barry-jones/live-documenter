
namespace TheBoxSoftware.Reflection.Core.COFF
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    internal class CodedIndexResolver : ICodedIndexResolver
    {
        private readonly Dictionary<CodedIndexes, IndexDetails> _detailsMap;

        public CodedIndexResolver(Dictionary<MetadataTables, int> rowsPerTableData)
        {
            _detailsMap = BuildDetailsMap(rowsPerTableData);
        }

        public CodedIndex Resolve(CodedIndexes indexType, uint value)
        {
            return DecodeIndex(value, _detailsMap[indexType]);
        }

        public int GetSizeOfIndex(CodedIndexes indexType)
        {
            return _detailsMap[indexType].RequiredNumberOfBytes();
        }

        private Dictionary<CodedIndexes, IndexDetails> BuildDetailsMap(Dictionary<MetadataTables, int> rowsPerTableData)
        {
            IEnumerable<CodedIndexes> values = Enum.GetValues(typeof(CodedIndexes)).Cast<CodedIndexes>();
            Dictionary<CodedIndexes, IndexDetails> map = new Dictionary<CodedIndexes, IndexDetails>();

            foreach(CodedIndexes current in values)
            {
                IndexDetails details = DetermineDetailsForIndexType(current);
                foreach(MetadataTables table in details.Tables)
                {
                    if(rowsPerTableData.Keys.Contains(table))
                    {
                        if(details.MaxRowsInTables < rowsPerTableData[table])
                        {
                            details.MaxRowsInTables = (uint)rowsPerTableData[table];
                        }
                    }
                }
                map.Add(current, details);
            }

            return map;
        }

        private IndexDetails DetermineDetailsForIndexType(CodedIndexes codedIndex)
        {
            IndexDetails details = new IndexDetails();
            details.Tables = new List<MetadataTables>();

            switch(codedIndex)
            {
                case CodedIndexes.HasFieldMarshall:
                    details.Tables.Add(MetadataTables.Field);
                    details.Tables.Add(MetadataTables.Param);
                    details.Mask = 0x01;
                    break;
                case CodedIndexes.HasSemantics:
                    details.Tables.Add(MetadataTables.Event);
                    details.Tables.Add(MetadataTables.Property);
                    details.Mask = 0x01;
                    break;
                case CodedIndexes.MethodDefOrRef:
                    details.Tables.Add(MetadataTables.MethodDef);
                    details.Tables.Add(MetadataTables.MemberRef);
                    details.Mask = 0x01;
                    break;
                case CodedIndexes.MemberForwarded:
                    details.Tables.Add(MetadataTables.Field);
                    details.Tables.Add(MetadataTables.MethodDef);
                    details.Mask = 0x01;
                    break;
                case CodedIndexes.TypeOrMethodDef:
                    details.Tables.Add(MetadataTables.TypeDef);
                    details.Tables.Add(MetadataTables.MethodDef);
                    details.Mask = 0x01;
                    break;
                case CodedIndexes.TypeDefOrRef:
                    details.Tables.Add(MetadataTables.TypeDef);
                    details.Tables.Add(MetadataTables.TypeRef);
                    details.Tables.Add(MetadataTables.TypeSpec);
                    details.Mask = 0x03;
                    break;
                case CodedIndexes.HasConstant:
                    details.Tables.Add(MetadataTables.Field);
                    details.Tables.Add(MetadataTables.Param);
                    details.Tables.Add(MetadataTables.Property);
                    details.Mask = 0x03;
                    break;
                case CodedIndexes.HasDeclSecurity:
                    details.Tables.Add(MetadataTables.TypeDef);
                    details.Tables.Add(MetadataTables.MethodDef);
                    details.Tables.Add(MetadataTables.Assembly);
                    details.Mask = 0x03;
                    break;
                case CodedIndexes.Implementation:
                    details.Tables.Add(MetadataTables.File);
                    details.Tables.Add(MetadataTables.AssemblyRef);
                    details.Tables.Add(MetadataTables.ExportedType);
                    details.Mask = 0x03;
                    break;
                case CodedIndexes.ResolutionScope:
                    details.Tables.Add(MetadataTables.Module);
                    details.Tables.Add(MetadataTables.ModuleRef);
                    details.Tables.Add(MetadataTables.AssemblyRef);
                    details.Tables.Add(MetadataTables.TypeRef);
                    details.Mask = 0x03;
                    break;
                case CodedIndexes.MemberRefParent:
                    details.Tables.Add(MetadataTables.TypeDef);
                    details.Tables.Add(MetadataTables.TypeRef);
                    details.Tables.Add(MetadataTables.ModuleRef);
                    details.Tables.Add(MetadataTables.MethodDef);
                    details.Tables.Add(MetadataTables.TypeSpec);
                    details.Mask = 0x07;
                    break;
                case CodedIndexes.CustomAttributeType:
                    details.Tables.Add(MetadataTables.Unused1);
                    details.Tables.Add(MetadataTables.Unused1);
                    details.Tables.Add(MetadataTables.MethodDef);
                    details.Tables.Add(MetadataTables.MemberRef);
                    details.Tables.Add(MetadataTables.Unused1);
                    details.Mask = 0x07;
                    break;
                case CodedIndexes.HasCustomAttribute:
                    details.Tables.Add(MetadataTables.MethodDef);
                    details.Tables.Add(MetadataTables.Field);
                    details.Tables.Add(MetadataTables.TypeRef);
                    details.Tables.Add(MetadataTables.TypeDef);
                    details.Tables.Add(MetadataTables.Param);
                    details.Tables.Add(MetadataTables.InterfaceImpl);
                    details.Tables.Add(MetadataTables.MemberRef);
                    details.Tables.Add(MetadataTables.Module);
                    details.Tables.Add(MetadataTables.DeclSecurity);
                    details.Tables.Add(MetadataTables.Property);
                    details.Tables.Add(MetadataTables.Event);
                    details.Tables.Add(MetadataTables.StandAloneSig);
                    details.Tables.Add(MetadataTables.ModuleRef);
                    details.Tables.Add(MetadataTables.TypeSpec);
                    details.Tables.Add(MetadataTables.Assembly);
                    details.Tables.Add(MetadataTables.AssemblyRef);
                    details.Tables.Add(MetadataTables.File);
                    details.Tables.Add(MetadataTables.ExportedType);
                    details.Tables.Add(MetadataTables.ManifestResource);
                    details.Tables.Add(MetadataTables.GenericParam);
                    details.Tables.Add(MetadataTables.GenericParamConstraint);
                    details.Tables.Add(MetadataTables.MethodSpec);
                    details.Mask = 0x1F;
                    break;
            }
            details.BitsToRepresentTag = details.Mask == 1
                ? (byte)1
                : (byte)Math.Round(Math.Log(details.Mask, 2), MidpointRounding.AwayFromZero);

            return details;
        }

        private CodedIndex DecodeIndex(uint value, IndexDetails withDetails)
        {
            byte code = 0;
            uint index = 0;

            if(withDetails.IsLarge())
            {
                code = (byte)(value & withDetails.Mask);
                index = (value & ~withDetails.Mask) >> withDetails.BitsToRepresentTag;
            }
            else
            {
                ushort inMask = (ushort)withDetails.Mask;
                ushort shortValue = (ushort)value;
                code = (byte)(value & inMask);
                index = (ushort)((shortValue & ~inMask) >> withDetails.BitsToRepresentTag); // shift to drop the masked bits
            }

            return new CodedIndex(withDetails.Tables[code], index);
        }

        public class IndexDetails
        {
            public uint Mask;
            public List<MetadataTables> Tables;
            public byte BitsToRepresentTag;
            public uint MaxRowsInTables;

            /// <summary>
            /// Indicates if the coded index is larger than a 2byte value (16 bits)
            /// </summary>
            /// <returns>A boolean</returns>
            public bool IsLarge()
            {
                return this.MaxRowsInTables > System.Math.Pow(2, 16 - this.BitsToRepresentTag);
            }

            /// <summary>
            /// Returns the number of bytes required to hold this coded index
            /// </summary>
            /// <returns>The number of bytes</returns>
            public int RequiredNumberOfBytes()
            {
                return this.IsLarge() ? 4 : 2;
            }
        }
    }
}
