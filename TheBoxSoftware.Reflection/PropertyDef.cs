using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection {
	using TheBoxSoftware.Reflection.Core.COFF;

	/// <summary>
	/// A class that describes a property that has been defined in an <see cref="TypeDef"/>.
	/// </summary>
	public sealed class PropertyDef : ReflectedMember {
		/// <summary>
		/// Initialises a new instance of the PropertyDef class.
		/// </summary>
		/// <param name="assembly">The assembly the property is defined in.</param>
		/// <param name="typeDef">The containing type definition.</param>
		/// <param name="metadata">The metadata store where the property is defined</param>
		/// <param name="row">The row that defines the details of the property.</param>
		/// <returns>The instantiated property.</returns>
		internal static PropertyDef CreateFromMetadata(AssemblyDef assembly, 
				TypeDef typeDef, 
				MetadataDirectory metadata, 
				PropertyMetadataTableRow row) {
			PropertyDef property = new PropertyDef();
			property.UniqueId = row.FileOffset;
			property.Type = typeDef;
			property.Name = assembly.StringStream.GetString(row.Name.Value);
			property.Assembly = assembly;
			// row.Type; // Contains details of the properties signiture
			return property;
		}

		/// <summary>
		/// 
		/// </summary>
		public TypeDef Type { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="MethodDef"/> which relates to the 
		/// associated get method for the property. This can be null if there is no
		/// getter defined.
		/// </summary>
		public MethodDef GetMethod { get; set; }

		/// <summary>
		/// Gest or sets the <see cref="MethodDef"/> which relates to the
		/// associated set method for the property. This can be null if there is no
		/// setter defined.
		/// </summary>
		public MethodDef SetMethod { get; set; }

		public override Visibility MemberAccess {
			get {
				int setterVisibility = 0;
				int getterVisibility = 0;
				if (this.SetMethod != null) {
					setterVisibility = (int)this.SetMethod.MemberAccess;
				}
				if (this.GetMethod != null) {
					getterVisibility = (int)this.GetMethod.MemberAccess;
				}

				// The more public, the greater the number
				return (setterVisibility > getterVisibility)
					? (Visibility)setterVisibility
					: (Visibility)getterVisibility;
			}
		}

		#region Methods
		/// <summary>
		/// Returns a display name for the Property.
		/// </summary>
		/// <param name="includeNamespace">Indicates if the namespace should be included.</param>
		/// <param name="includeParameters">indicates if the parameters should be included.</param>
		/// <returns>A string representing the display name for the property.</returns>
		public string GetDisplayName(bool includeNamespace, bool includeParameters) {
			DisplayNameSignitureConvertor convertor = new DisplayNameSignitureConvertor(this, includeNamespace, includeParameters);
			return convertor.Convert();
		}

		/// <summary>
		/// Returns a display name for the property.
		/// </summary>
		/// <param name="includeNamespace">Indicates if the namespace should be included.</param>
		/// <returns>A string representing the display name for the property.</returns>
		public string GetDisplayName(bool includeNamespace) {
			DisplayNameSignitureConvertor convertor = new DisplayNameSignitureConvertor(this, includeNamespace, true);
			return convertor.Convert();
		}
		#endregion
	}
}
