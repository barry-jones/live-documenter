using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection{
	using TheBoxSoftware.Reflection.Core.COFF;

	/// <summary>
	/// Describes a parameter
	/// </summary>
	public class ParamDef : ReflectedMember {
		private TypeRef typeRef;

		#region Methods
		/// <summary>
		/// Initialises a ParamDef from provided metadata
		/// </summary>
		/// <param name="owner">The owning method of the parameter</param>
		/// <param name="metadata">The metadata stream</param>
		/// <param name="row">The row that details the parameter</param>
		/// <returns>The instantiated defenition</returns>
		internal static ParamDef CreateFromMetadata(MethodDef owner, MetadataDirectory metadata, ParamMetadataTableRow row) {
			ParamDef parameter = new ParamDef();

			AssemblyDef assembly = owner.Assembly;
			parameter.UniqueId = assembly.GetUniqueId();
			parameter.Name = assembly.StringStream.GetString(row.Name.Value);
			assembly = null;
			parameter.Method = owner;
			parameter.Sequence = row.Sequence;
			parameter.Assembly = owner.Assembly;

			return parameter;
		}
		#endregion

		#region Methods
		private TypeRef ResolveParameterType() {
			if (this.Sequence == 0) {
				// TODO: Need to understand this more, but it generally
				// refers to information about the return type for the method.
				return null;
			}
			else {
				Reflection.Signitures.ParamSignitureToken token = this.Method.Signiture.GetParameterTokens()[this.Sequence - 1];
				return token.ResolveParameter(this.Method.Assembly, this);
			}
		}

		public TypeRef GetTypeRef() {
			if (this.Sequence == 0) {
				// TODO: Need to understand this more, but it generally
				// refers to information about the return type for the method.
				return null;
			}
			else {
				Reflection.Signitures.ParamSignitureToken token = this.Method.Signiture.GetParameterTokens()[this.Sequence - 1];
				return token.ResolveParameter(this.Method.Assembly, this);
			}
		}
		#endregion

		#region Parameters
		/// <summary>
		/// Reference to the method that owns the parameter
		/// </summary>
		public MethodDef Method {
			get;
			set;
		}

		/// <summary>
		/// The name of the parameter
		/// </summary>
		public string Name {
			get;
			set;
		}

		/// <Summary>
		/// A number that indicates the sequence in the parameter list this ParamDef
		/// refers to on its parent method.
		/// </Summary>
		public int Sequence { get; set; }
		#endregion
	}
}
