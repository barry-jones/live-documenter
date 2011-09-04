using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Model {
	using TheBoxSoftware.Reflection.Core;
	using TheBoxSoftware.Reflection.Core.COFF;
	using TheBoxSoftware.DeveloperSuite.PEViewer.Model.MetadataWrappers;

	/// <summary>
	/// 
	/// </summary>
	internal class MetadataStreamEntry : Entry {
		public MetadataStreamEntry(MetadataStream metadataStream) 
			: base(metadataStream.Name) {
			Entry tables = new Entry("Tables");
			foreach (KeyValuePair<MetadataTables, MetadataRow[]> o in metadataStream.Tables) {
				string name = o.Key.ToString() + " (" + o.Value.Length + ")";
				Entry tableEntry = Entry.Create(name);
				tableEntry.Data = this.ConvertTable(metadataStream, o.Key.ToString(), o.Value.ToList());
				tables.Children.Add(tableEntry);
			}
			this.Children.Add(tables);
		}

		/// <summary>
		/// Converts the item data to one of the metadata wrappers for the data which controls
		/// how the item will be viewed, which properties are viewed and the format for the data.
		/// </summary>
		/// <param name="stream">The metadata stream containing the details</param>
		/// <param name="name">The name of the table</param>
		/// <param name="data">The actual data</param>
		/// <returns></returns>
		private Array ConvertTable(MetadataStream stream, string name, List<MetadataRow> data) {
			switch (name) {
				case "MethodDef": return new MethodDefMetadataWrapper(stream, data).Items.ToArray();
				case "TypeDef": return new TypeDefMetadataWrapper(stream, data).Items.ToArray();
				case "TypeRef": return new TypeRefMetadataWrapper(stream, data).Items.ToArray();
				case "Assembly": return new AssemblyMetadataWrapper(stream, data).Items.ToArray();
				case "AssemblyRef": return new AssemblyRefMetadataWrapper(stream, data).Items.ToArray();
				case "Module": return new ModuleMetadataWrapper(stream, data).Items.ToArray();
				case "Field": return new FieldMetadataWrapper(stream, data).Items.ToArray();
				case "Param": return new ParamMetadataWrapper(stream, data).Items.ToArray();
				case "InterfaceImpl": return new InterfaceImplMetadataWrapper(stream, data).Items.ToArray();
				case "MemberRef": return new MemberRefMetadataWrapper(stream, data).Items.ToArray();
				case "Constant": return new ConstantMetadataWrapper(stream, data).Items.ToArray();
				case "CustomAttribute": return new CustomAttributeMetadataWrapper(stream, data).Items.ToArray();
				case "FieldMarshal": return new FieldMarshalMetadataWrapper(stream, data).Items.ToArray();
				case "DeclSecurity": return new DeclSecurityMetadataWrapper(stream, data).Items.ToArray();
				case "ClassLayout": return new ClassLayoutMetadataWrapper(stream, data).Items.ToArray();
				case "FieldLayout": return new FieldLayoutMetadataWrapper(stream, data).Items.ToArray();
				case "StandAloneSig": return new StandAloneSigMetadataWrapper(stream, data).Items.ToArray();
				case "EventMap": return new EventMapMetadataWrapper(stream, data).Items.ToArray();
				case "Event": return new EventMetadataWrapper(stream, data).Items.ToArray();
				case "PropertyMap": return new PropertyMapMetadataWrapper(stream, data).Items.ToArray();
				case "Property": return new PropertyMetadataWrapper(stream, data).Items.ToArray();
				case "MethodSemantics": return new MethodSemanticsMetadataWrapper(stream, data).Items.ToArray();
				case "MethodImpl": return new MethodImplMetadataWrapper(stream, data).Items.ToArray();
				case "ModuleRef": return new ModuleRefMetadataWrapper(stream, data).Items.ToArray();
				case "TypeSpec": return new TypeSpecMetadataWrapper(stream, data).Items.ToArray();
				case "ImplMap": return new ImplMapMetadataWrapper(stream, data).Items.ToArray();
				case "FieldRVA": return new FieldRVAMetadataWrapper(stream, data).Items.ToArray();
				case "File": return new FileMetadataWrapper(stream, data).Items.ToArray();
				case "ManifestResource": return new ManifestResourceMetadataWrapper(stream, data).Items.ToArray();
				case "NestedClass": return new NestedClassMetadataWrapper(stream, data).Items.ToArray();
				case "GenericParam": return new GenericParamMetadataWrapper(stream, data).Items.ToArray();
				case "MethodSpec": return new MethodSpecMetadataWrapper(stream, data).Items.ToArray();
				case "GenericParamConstraint": return new GenericParamConstraintMetadataWrapper(stream, data).Items.ToArray();
				default:
					return data.ToArray();
			}
		}
	}
}
